﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LineDrawerDemo.MainWindow;

namespace LineDrawerDemo
{
    public partial class MainWindow : Form
    {
        public bool canvasMouseDown;
        public int selectedCanvasLineEnd;
        public int numClicks = 0;
        public bool mouseClicked = false;
        public Point tempMouseStartPos = new Point();
        public Point tempMouseEndPos = new Point();
        public int selectedNode;
        public bool lockInToLineEnds = false;
        public bool DebugMode = false;
        public string fileLocation = Application.StartupPath;
        Dictionary<int, LineObject> LineObjects = new Dictionary<int, LineObject>();
        public class LineObject
        {
            public int key { get; set; }
            public int x1 { get; set; }
            public int y1 { get; set; }
            public int x2 { get; set; }
            public int y2 { get; set; }

            //Inputed "real" coordinates
            public int Realx1 { get; set; }
            public int Realy1 { get; set; }
            public int Realx2 { get; set; }
            public int Realy2 { get; set; }

        }
        public MainWindow()
        {
            InitializeComponent();
        }
        public void viewSaveFileDialog()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.InitialDirectory = Application.StartupPath;
            save.Title = "Save line data to file";
            
            if (save.ShowDialog() == DialogResult.OK) {
                fileLocation = save.FileName;
                writeLineFile(save.FileName);
            }
        }
        public void viewOpenFileDialog()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = Application.StartupPath;
            open.Title = "Load line data from file";

            if (open.ShowDialog() == DialogResult.OK) {
                fileLocation = open.FileName;
                readLineFile(open.FileName);
            }
        }
        public void writeLineFile(string location) //Saves and writes file, from dictionary
        {
            if (LineObjects.Count != 0)
            {
                StreamWriter write = new StreamWriter(location);
                foreach (var lineObject in LineObjects)
                {
                    write.WriteLine(
                        "line:" + lineObject.Key
                        + ",x1:" + lineObject.Value.Realx1
                        + ",y1:" + lineObject.Value.Realy1
                        + ",x2:" + lineObject.Value.Realx2
                        + ",y2:" + lineObject.Value.Realy2
                        );
                }
                write.Close();
            }
        }
        public void readLineFile(string location) //Reads and loads file, adds file line contents to dictionary
        {
            LineObjects.Clear();

            string[] lines = File.ReadAllLines(location);
            if (lines.Length != 0)
            {
                foreach (string str in lines)
                {
                    string[] linesObject = str.Split(',');

                    int tempKey = 0; int tempX1 = 0; int tempX2 = 0; int tempY1 = 0; int tempY2 = 0;

                    foreach (string Str in linesObject)
                    {
                        string tempStr = Str;
                        string[] linesSubValue = new string[2];
                        linesSubValue = tempStr.Split(':');
                        //string tempString = linesSubValue[0];

                        switch (linesSubValue[0])
                        {
                            case "line":
                                if (int.TryParse(linesSubValue[1], out tempKey)) { } else { tempKey = 0; } //In else check if key available if not set next available
                                break;
                            case "x1":
                                if (int.TryParse(linesSubValue[1], out tempX1)) { } else { tempX1 = 0; }
                                break;
                            case "y1":
                                if (int.TryParse(linesSubValue[1], out tempY1)) { } else { tempY1 = 0; }
                                break;
                            case "x2":
                                if (int.TryParse(linesSubValue[1], out tempX2)) { } else { tempX2 = 0; }
                                break;
                            case "y2":
                                if (int.TryParse(linesSubValue[1], out tempY2)) { } else { tempY2 = 0; }
                                break;
                            default:
                                break;
                        }
                    }
                    if (checkKeyAvailability(tempKey))
                    {
                        InitializeLineObject(tempKey, tempX1, tempY1, tempX2, tempY2);
                    }
                    else
                    {

                    }
                }
                updateLineTreeView();
                linesDraw();
            }
        }
        public void InitializeLineObject(int key, int StartPosX, int StartPosY, int EndPosX, int EndPosY) //Creates new line and adds it onto the dictionary, --fixes new line's x and y coordinates to duplicates--
        {
            LineObjects.Add(key, new LineObject
            {Realx1 = StartPosX, Realy1 = StartPosY, Realx2 = EndPosX, Realy2 = EndPosY });
            //fixLineObjectCoordinates(key); // Broken and buggy, test it out
        }
        public void fixLineObjectCoordinates(int key) //Fixes the coordinates that will be painted onto the control from the "real" input coordinates, a bit broken and buggy
        {
            LineObjects[key].x1 = fixXCoord(LineObjects[key].Realx1);
            LineObjects[key].y1 = fixYCoord(LineObjects[key].Realy1);
            LineObjects[key].x2 = fixXCoord(LineObjects[key].Realx2);
            LineObjects[key].y2 = fixYCoord(LineObjects[key].Realy2);
        }
        public int fixXCoord(int xPos) {  return xPos + Canvas.Location.X; }
        public int fixYCoord(int yPos) { return this.Size.Height - (yPos + 44 + (Canvas.Location.Y + Canvas.Size.Height)); }

        public void BeginGraphics()
        {
            //this.SuspendLayout();
            //Graphics g = this.CreateGraphics(); //Create drawing canvas

            //foreach (var lineobject in LineObjects.ToDictionary()) //Extract every line from list and draw them
            //{
            //    Point startPoint = new Point(lineObject.x1, fixYCoord(lineObject.y1));
            //    Point endPoint = new Point(lineObject.x2, fixYCoord(lineObject.y2));
            //
            //    g.DrawLine(Pens.Black, startPoint, endPoint); //Draw Line from start to end points
            //}
        }
        public void linesDraw() //Forces canvas to redraw
        {
            if (LineObjects.Keys.Count > 0)
            {
                Canvas.Invalidate();
                Canvas.Update();

            }
        }
        public void updateLineTreeView() //Updates the treeView to correspond to the updated dictionary's contents
         {
            linesTreeView.Nodes[0].Nodes.Clear();

            foreach (var lineObject in LineObjects.Keys)
            {
                TreeNode node = new TreeNode();
                node.Text = "Line" + lineObject.ToString();
                node.Name = "line" + lineObject.ToString();
                node.Tag = lineObject;
                linesTreeView.Nodes[0].Nodes.Add(node);

            }
            linesTreeView.Nodes[0].Expand();
        }

        public void editLineProperties(int key, int x1, int y1, int x2, int y2) //Edits a specific line's coordinates, --fixes it's x and y coordinates--, updates the TreeView
        {
            if (x1 != 0) { LineObjects[key].Realx1 = x1; }
            if (y1 != 0) { LineObjects[key].Realy1 = y1; }
            if (x2 != 0) { LineObjects[key].Realx2 = x2; }
            if (y2 != 0) { LineObjects[key].Realy2 = y2; }

            //fixLineObjectCoordinates(key); Broken and buggy, test it out
        }
        public void copyLineObjectWithNewKey(int key, int newKey) //Copies line with new key and removes old line with old key
        {
            InitializeLineObject(newKey, LineObjects[key].Realx1, LineObjects[key].Realy1, LineObjects[key].Realx2, LineObjects[key].Realy2);
            LineObjects.Remove(key);
        }
        public bool checkKeyAvailability(int potenstialKey) //Checks if specified position within dictionary is available
        {
            bool availability = true;

            if (LineObjects.Keys.Count > 0)
            {
                foreach (var lineObject in LineObjects.Keys)
                {
                    if (lineObject == potenstialKey) { availability = false; break; }
                }
            }
            return availability;
        }
        public void setSelectedNode(int nodeKey) //Changes varible and label to correspont to correct selectedNode
        {
            selectedNode = nodeKey;
            selectedNodeLabel.Text = "Selected node: [" + selectedNode + "]";
        }
        public void createLine(int key, int x1, int y1, int x2, int y2) //Creates new line if key is available
        {
            if (checkKeyAvailability(key))
            {
                InitializeLineObject(key, x1, y1, x2, y2); //Adds new line to list
                updateLineTreeView();
                setSelectedNode(key);
            }
        }
        public void removeLine(int key) //Removes line from dictionary
        {
            LineObjects.Remove(key);
            updateLineTreeView();
        }
        public void saveLine(int key, int x1, int y1, int x2, int y2) //Edits line properties, if diffrent key it then copies line to new key
        {
            if (selectedNode == key)
            {
                editLineProperties(selectedNode, x1, y1, x2, y2);
            }
            else if (checkKeyAvailability(key))
            {
                copyLineObjectWithNewKey(selectedNode, key);
                editLineProperties(key, x1, y1, x2, y2);
            }

            updateLineTreeView();
            setSelectedNode(key);
        }
        public bool isWithinDistanceToLineEnd(int key, int posX, int posY, int minDistance, out int lineEnd) //Checks if inputted coordinates is within distance to one of specified line's ends
        {
            int diffX1 = Math.Abs(LineObjects[key].Realx1 - posX);
            int diffY1 = Math.Abs(LineObjects[key].Realy1 - posY);

            int diffX2 = Math.Abs(LineObjects[key].Realx2 - posX);
            int diffY2 = Math.Abs(LineObjects[key].Realy2 - posY);

            bool isClose = false;
            lineEnd = 0;

            if (diffX1 < minDistance && diffY1 < minDistance) {
                isClose = true;
                lineEnd = 1;
            }
            else if (diffX2 < minDistance && diffY2 < minDistance) {
                isClose = true;
                lineEnd = 2;
            }
            return isClose;
        }
        public int getClosestLineAsKey(int baseXPos, int baseYPos, out int lineEnd) //Maybe implement this system in future, not used, maybe test
        {   
            int key = 0;
            int closestLineEnd = 0;
            int closestLineEndDistance = 10000000;
            int baseDistance = (int) (Math.Pow(baseXPos, 2) + Math.Pow(baseYPos, 2));

            foreach (var line in LineObjects)
            {
                int lineDistanceLineEnd1 = (int) Math.Abs((Math.Pow(LineObjects[line.Key].Realx1, 2) + Math.Pow(LineObjects[line.Key].Realy1, 2)) - baseDistance);
                int lineDistanceLineEnd2 = (int) Math.Abs((Math.Pow(LineObjects[line.Key].Realx2, 2) + Math.Pow(LineObjects[line.Key].Realy2, 2)) - baseDistance);

                if (lineDistanceLineEnd1 < closestLineEndDistance) {
                    key = line.Key;
                    closestLineEnd = 1;
                } 
                else if (lineDistanceLineEnd2 < closestLineEndDistance) {
                    key = line.Key;
                    closestLineEnd = 2;
                }
            }
            
            lineEnd = closestLineEnd;
            return key;
        }
        public List<int[]> getClosestLinesAsArrayWithLineEnds(int baseXPos, int baseYPos, int minDistance) //Get line keys and line ends that are within a certain area
        {
            List<int[]> lines = new List<int[]>();
            int tempOutLineEnd;
            int indexCount = 0;

            foreach (var line in LineObjects)
            {
                if (isWithinDistanceToLineEnd(line.Key, baseXPos, baseYPos, minDistance, out tempOutLineEnd))
                {
                    int[] lineValues = new int[2] { line.Key, tempOutLineEnd };
                    lines.Add(lineValues);
                    indexCount += 1;
                }
            }
            return lines;
        }
        public int[] getClosestLinesAsArray(int baseXPos, int baseYPos, int minDistance) //Get line keys that are within a certain area
        {
            List<int> listLines = new List<int>();
            int tempOutLineEnd;
            int indexCount = 0;

            foreach (var line in LineObjects)
            {
                if (isWithinDistanceToLineEnd(line.Key, baseXPos, baseYPos, minDistance, out tempOutLineEnd))
                {
                    listLines.Add(line.Key);
                    indexCount += 1;
                }
            }
            int[] lines = listLines.ToArray();
            return lines;
        }
        public void onMouseClickGetCoordinates(int xPos, int yPos) //Retrieves mouse click coordinates, checks if mouse position is within line end coordinates
        {
            int key = selectedNode;
            int maxDistance = 10;
            int tempLineEnd;

            if (isWithinDistanceToLineEnd(key, xPos, yPos, maxDistance, out tempLineEnd))
            {
                selectedCanvasLineEnd = tempLineEnd;
            }

            linesDraw();
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
            //InitializeLineObject(10, 20, 120, 80);
            //BeginGraphics();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            int tempX1; int tempY1; int tempX2; int tempY2; int tempLineKeyBox; //Temporary varibles

            if (int.TryParse(x1Box.Text, out tempX1)) { } else { tempX1 = 0; }
            if (int.TryParse(y1Box.Text, out tempY1)) { } else { tempY1 = 0; }
            if (int.TryParse(x2Box.Text, out tempX2)) { } else { tempX2 = 1; }
            if (int.TryParse(y2Box.Text, out tempY2)) { } else { tempY2 = 1; }
            tempLineKeyBox = ((int)lineKeyBox.Value);

            createLine(tempLineKeyBox, tempX1, tempY1, tempX2, tempY2);
            linesDraw();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            removeLine(selectedNode);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int tempX1; int tempY1; int tempX2; int tempY2; int tempLineKeyBox; //Temporary varibles

            if (int.TryParse(x1Box.Text, out tempX1)) { } else { tempX1 = 0; }
            if (int.TryParse(y1Box.Text, out tempY1)) { } else { tempY1 = 0; }
            if (int.TryParse(x2Box.Text, out tempX2)) { } else { tempX2 = 1; }
            if (int.TryParse(y2Box.Text, out tempY2)) { } else { tempY2 = 1; }

            tempLineKeyBox = ((int)lineKeyBox.Value);

            saveLine(tempLineKeyBox, tempX1, tempY1, tempX2, tempY2);
            linesDraw();
        }

        private void btnDraw_Click(object sender, EventArgs e)
        {
            //BeginGraphics();
            linesDraw();
        }

        private void lineCanvas_Paint(object sender, PaintEventArgs e)
        {
            this.SuspendLayout();
            Graphics g = e.Graphics; //Create drawing canvas

            foreach (var lineObject in LineObjects) //Extract every line from list and draw them
            {
                Point startPoint = new Point(lineObject.Value.Realx1, lineObject.Value.Realy1);
                Point endPoint = new Point(lineObject.Value.Realx2, lineObject.Value.Realy2);

                g.DrawLine(Pens.Black, startPoint, endPoint); //Draw Line from start to end points

                if (DebugMode == true) {
                    double tempX = lineObject.Value.Realx1;
                    double tempY = lineObject.Value.Realy1;
                    double tempDiffX = (lineObject.Value.Realx2 - lineObject.Value.Realx1);
                    double tempDiffY = (lineObject.Value.Realy2 - lineObject.Value.Realy1);

                    double kValue = tempDiffY / tempDiffX;


                    if (kValue >= 0) // Adjust drawn label to correct place, works well
                    {
                        tempX = tempX + tempDiffX / 2;
                        tempY = tempY + tempDiffY / 2 - 15; // + 10 * kValue;
                    }
                    else if (kValue <= 0)
                    {
                        tempX = tempX + tempDiffX / 2;
                        tempY = tempY + tempDiffY / 2 + 2; // - kValue / 10;
                    }
                    else if (kValue > -1 && kValue < 1) {
                        tempX = tempX + tempDiffX / 2;
                        tempY = tempY - tempDiffY / 2;
                    }

                    int posX = (int)Math.Round(tempX, 1);
                    int posY = (int)Math.Round(tempY, 1);

                    g.DrawString("line" + lineObject.Key, DefaultFont, Brushes.Black, new Point(posX, posY));
                    
                    //Label label = new Label();
                    //label.Name = "line" + lineObject.Key;
                    //label.Text = "line" + lineObject.Key;
                    //label.Location = new Point(tempX, tempY);
                    //label.Tag = "tempLabel";
                    //Canvas.Controls.Add(label);
                }
            }
            if (DebugMode == true) 
            {
                Rectangle rec = new Rectangle(); // Draws out a rectangle on top of selected line's line end to visualy show selected line end
                Pen pen = new Pen(Color.Gray, 2);

                if (selectedCanvasLineEnd == 1)
                {
                    rec.X = LineObjects[selectedNode].Realx1 - 5;
                    rec.Y = LineObjects[selectedNode].Realy1 - 5;

                    rec.Width = 10;
                    rec.Height = 10;

                    g.DrawRectangle(pen, rec);
                }
                else if (selectedCanvasLineEnd == 2)
                {
                    rec.X = LineObjects[selectedNode].Realx2 - 5;
                    rec.Y = LineObjects[selectedNode].Realy2 - 5;

                    rec.Width = 10;
                    rec.Height = 10;
                    
                    

                    g.DrawRectangle(pen, rec);
                }
            }
        }

        private void linesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (linesTreeView.SelectedNode.Tag.ToString() != "main") {

                selectedNode = (int)linesTreeView.SelectedNode.Tag;
                lineKeyBox.Value = selectedNode;
                selectedNodeLabel.Text = "Selected node: [" + selectedNode + "]";

                //if (!checkKeyAvailability(selectedNode)) //Checks if selected node exists so it can write to from existing LineObject
                if (true)
                {
                    if (LineObjects[selectedNode].Realx1 != 0) { x1Box.Text = LineObjects[selectedNode].Realx1.ToString(); }
                    if (LineObjects[selectedNode].Realy1 != 0) { y1Box.Text = LineObjects[selectedNode].Realy1.ToString(); }
                    if (LineObjects[selectedNode].Realx2 != 0) { x2Box.Text = LineObjects[selectedNode].Realx2.ToString(); }
                    if (LineObjects[selectedNode].Realy2 != 0) { y2Box.Text = LineObjects[selectedNode].Realy2.ToString(); }
                }
            }
        }

        private void checkBoxLabelLines_CheckedChanged(object sender, EventArgs e)
        {
            DebugMode = checkBoxDebugMode.Checked;
            lockInToLineEnds = checkBoxDebugMode.Checked; 
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            viewSaveFileDialog();
            fileLocationBox.Text = fileLocation;
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            viewOpenFileDialog();
            fileLocationBox.Text = fileLocation;
            
        }

        private void btnReloadFile_Click(object sender, EventArgs e)
        {
            readLineFile(fileLocation);
        }

        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            onMouseClickGetCoordinates(e.Location.X, e.Location.Y);
            int temp1 = getClosestLineAsKey(e.Location.X, e.Location.Y, out int LineEnd);
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}
