using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static LineDrawerDemo.MainWindow;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.LinkLabel;

namespace LineDrawerDemo
{
    public partial class MainWindow : Form
    {
        //
        // Neccesarry Public Varibles ------------------------------------------
        //
        public int selectedCanvasLineEnd = 0;
        public int numClicks = 0;
        public bool mouseClicked = false;
        public Point tempMouseStartPos = new Point();
        public Point tempMouseEndPos = new Point();
        public Point selectedPointPos = new Point();
        public int selectedNode;
        public bool lockInToLineEnds = false;
        public bool lineMultiLocking = false;
        public bool DebugMode = false;
        //public bool editLineMode = false;
        //public bool createLineMode = false;
        //public bool removeLineMode = false;
        public string canvasLineMode = "";
        public string fileLocation = Application.StartupPath;
        public int minSelectDistance = 10;
        public int lineWidth = 1;
        public int numPolygonCorners = 4; // Add way to specify this
        public int radiusPolygon = 10;
        Dictionary<int, LineObject> LineObjects = new Dictionary<int, LineObject>();
        Dictionary<int, int> selectedLineObjects = new Dictionary<int, int>();

        //
        // ------------------------------------------
        //

        public class LineObject //LineObject 
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

        //
        // Exception handling
        //
        Dictionary<string, string> exceptionList = new Dictionary<string, string>
        {
            { "test_id", "test_title, test_message" }, //Test message, old
            { "already_occupied_key", "Key is already occupied" },
            { "not_existing_key", "Key does not exist" },
            { "no_line_end_nearby", "Found no line ends near position" },
            { "no_existing_lines", "No lines exist" },
            { "invalid_selected_node", "Selected node is invalid" },
            { "2", "" },
            { "3", "" },
        }; //Exception list with exception id and exception message
        public void generateException(string id)
        {
            //throw new NotImplementedException();

            //string[] ex = exceptionList[id].Split(',');

            //MessageBox.Show(ex[1], ex[0]);
            MessageBox.Show(exceptionList[id], id);
        }

        //
        // Save/load File Handling ------------------------------------------
        //
        public void viewSaveFileDialog()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.InitialDirectory = Application.StartupPath;
            save.Title = "Save line data to file";
            save.Filter = "Line file (*.line)|*.line";
            
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
            open.Filter = "Line file (*.line)|*.line";

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

            string[] lines = System.IO.File.ReadAllLines(location);
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
        
        //
        // Line coordinate fixing handling ------------------------------------------
        //
        public void fixLineObjectCoordinates(int key) //Fixes the coordinates that will be painted onto the control from the "real" input coordinates, a bit broken and buggy
        {
            LineObjects[key].x1 = fixXCoord(LineObjects[key].Realx1);
            LineObjects[key].y1 = fixYCoord(LineObjects[key].Realy1);
            LineObjects[key].x2 = fixXCoord(LineObjects[key].Realx2);
            LineObjects[key].y2 = fixYCoord(LineObjects[key].Realy2);
        }
        public int fixXCoord(int xPos) {  return xPos + Canvas.Location.X; }
        public int fixYCoord(int yPos) { return this.Size.Height - (yPos + 44 + (Canvas.Location.Y + Canvas.Size.Height)); }
    
        //
        // Basic/Advanced line functions handling ------------------------------------------
        //
        public void InitializeLineObject(int key, int StartPosX, int StartPosY, int EndPosX, int EndPosY) //Creates new line and adds it onto the dictionary, --fixes new line's x and y coordinates to duplicates--
        {
            LineObjects.Add(key, new LineObject
            { Realx1 = StartPosX, Realy1 = StartPosY, Realx2 = EndPosX, Realy2 = EndPosY });
            //fixLineObjectCoordinates(key); // Broken and buggy, test it out
        }
        public void createLine(int key, int x1, int y1, int x2, int y2) //Creates new line if key is available
        {
            if (checkKeyAvailability(key))
            {
                InitializeLineObject(key, x1, y1, x2, y2); //Adds new line to list
                updateLineTreeView();
                setSelectedNode(key);
                linesDraw();
                resetSelectedLine();
            }
            else { generateException("already_occupied_key"); } // already occupied key
            
        }
        public void removeLine(int key) //Removes line from dictionary
        {
            if (!checkKeyAvailability(key)) {
                LineObjects.Remove(key);
                updateLineTreeView();
                linesDraw();
                resetSelectedLine();
            }
            else { generateException("not_existing_key"); } // not existing key
        }
        public void removeLineByDialog(int key, out bool success)
        {
            success = false;
            var dialog = MessageBox.Show("Remove line:" + selectedNode, "Confirm remove", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (dialog == DialogResult.OK)
            {
                removeLine(key);
                success = true;
            }
        }
        public void saveLine(int key, int x1, int y1, int x2, int y2) //Edits line properties, if diffrent key it then copies line to new key
        {
            if (LineObjects.Count > 0)
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
                linesDraw();
                resetSelectedLine();
            }
            else { generateException("not_existing_key"); } // not existing key
        }
        public void editLineProperties(int key, int x1, int y1, int x2, int y2) //Edits a specific line's coordinates, --fixes it's x and y coordinates--, updates the TreeView
        {
            if (x1 != 0) { LineObjects[key].Realx1 = x1; }
            if (y1 != 0) { LineObjects[key].Realy1 = y1; }
            if (x2 != 0) { LineObjects[key].Realx2 = x2; }
            if (y2 != 0) { LineObjects[key].Realy2 = y2; }            

            //fixLineObjectCoordinates(key); Broken and buggy, test it out
        }
        public void editSpecificLineCoordinates(int key, int x1, int y1, int x2, int y2) //Setting a property as -1 unaffects it
        {
            int tempX1; int tempY1; int tempX2; int tempY2;

            if (x1 == -1) { tempX1 = LineObjects[key].Realx1; } else { tempX1 = x1; }
            if (y1 == -1) { tempY1 = LineObjects[key].Realy1; } else { tempY1 = y1; }
            if (x2 == -1) { tempX2 = LineObjects[key].Realx2; } else { tempX2 = x2; }
            if (y2 == -1) { tempY2 = LineObjects[key].Realy2; } else { tempY2 = y2; }
            editLineProperties(key, tempX1, tempY1, tempX2, tempY2);
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
        public Dictionary<int, (Point, Point)> createPolygonLines(int xPos, int yPos, int numPolygonCorners, double radius, double angleExtension)
        {
            Dictionary<int, (Point, Point)> polygonLines = new Dictionary<int, (Point, Point)>();
            Dictionary<int, Point> polygonPoints = new Dictionary<int, Point>();

            double angle = 0;
            double radianAngle = 0;
            int i = 0; //Incrementer
            int x = 0;
            int y = 0;

            //First Point
            angle = ((360 / numPolygonCorners) * i) + angleExtension;
            radianAngle = (angle * (Math.PI / 180));

            x = (int)(radius * Math.Cos(radianAngle)) + xPos;
            y = (int)(radius * Math.Sin(radianAngle)) + yPos;

            polygonPoints.Add(i, new Point(x, y));

            //Create polygon points
            for (i = 1; i < 2 * numPolygonCorners + 1; i = i + 2)
            {

                angle = ((360 / numPolygonCorners) * (i + 1) / 2) + angleExtension;
                radianAngle = (angle * (Math.PI / 180));

                x = (int)(radius * Math.Cos(radianAngle)) + xPos;
                y = (int)(radius * Math.Sin(radianAngle)) + yPos;

                //Create double amount of points at same point
                polygonPoints.Add(i, new Point(x, y));
                polygonPoints.Add((i + 1), new Point(x, y));
            }

            //Create lines from polygon points
            for (i = 0; i < 2 * numPolygonCorners; i = i + 2)
            {
                polygonLines.Add(i / 2, (polygonPoints[i], polygonPoints[i + 1]));
            }

            return polygonLines;
        }
        public void InitialisePolygon(Dictionary<int, (Point, Point)> polygonLines)
        { 
            foreach (var polygonLine in polygonLines)
            {
                int key = getLowestAvailableKey();
                int x1 = polygonLine.Value.Item1.X;
                int y1 = polygonLine.Value.Item1.Y;
                int x2 = polygonLine.Value.Item2.X;
                int y2 = polygonLine.Value.Item2.Y;

                InitializeLineObject(key, x1, y1, x2, y2);
            }
        }

        //
        // Retrieve line key/end/coordinate on canvas handling ------------------------------------------
        //
        public bool isWithinDistanceToLineEnd(int key, int posX, int posY, int minDistance, out int lineEnd) //Checks if inputted coordinates is within distance to one of specified line's ends
        {
            int diffX1 = Math.Abs(LineObjects[key].Realx1 - posX);
            int diffY1 = Math.Abs(LineObjects[key].Realy1 - posY);

            int diffX2 = Math.Abs(LineObjects[key].Realx2 - posX);
            int diffY2 = Math.Abs(LineObjects[key].Realy2 - posY);

            bool isClose = false;
            lineEnd = 0;

            if (diffX1 < minDistance && diffY1 < minDistance)
            {
                isClose = true;
                lineEnd = 1;
            }
            else if (diffX2 < minDistance && diffY2 < minDistance)
            {
                isClose = true;
                lineEnd = 2;
            }
            return isClose;
        }
        public int getClosestLineAsKey(int baseXPos, int baseYPos, out int lineEnd) //Maybe implement this system in future, (not used), maybe test
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
        public int[] getClosestLineEndCoordinate(int baseXPos, int baseYPos, int minDistance, out int lineEnd) //Get line keys and line ends that are within a certain area
        {
            List<int[]> lines = getClosestLinesAsArrayWithLineEnds(baseXPos, baseYPos, minDistance);
            int[] line = lines[0];

            int tempXPos = 0;
            int tempYPos = 0;

            if (line[1] == 1)
            {
                tempXPos = LineObjects[line[0]].Realx1;
                tempYPos = LineObjects[line[0]].Realy1;
            }
            else if (line[1] == 2)
            {
                tempXPos = LineObjects[line[0]].Realx2;
                tempYPos = LineObjects[line[0]].Realy2;
            }

            int[] lineCoordinates = new int[2]
            {
                tempXPos, tempYPos
            };
            lineEnd = line[1];
            return lineCoordinates;
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
        public int[] getLineEndCoordinates(int key, int lineEnd) //Retrieves line end coordinates 
        {
            int[] line = new int[2];

            if (lineEnd == 1)
            {
                line[0] = LineObjects[key].Realx1;
                line[1] = LineObjects[key].Realy1;
            }
            else if (lineEnd == 2)
            {
                line[0] = LineObjects[key].Realx2;
                line[1] = LineObjects[key].Realy2;
            }

            return line;
        }
        public int getLowestAvailableKey()
        {
            int key = 0;
            bool available = false;
            int count = 0;

            while (available == false)
            {
                available = checkKeyAvailability(count); 
                if (available == true) // checks if key is occupied, if not retuns true
                {
                    key = count; break;
                }
                count += 1;
            }

            return key;
        }


        //
        // Necessary miscellaneous handling ------------------------------------------
        //
        //Attempt to implement multi selecting and action.ing
        public void InitiateMultiLineMove(List<int[]> lines) // Potential multi line move function
        {
            foreach (var line_ in lines)
            {
                int[] line = line_ as int[];
                setSelectedNode(line[0]);
                selectedCanvasLineEnd = line[1];
                int[] endPoint = getLineEndCoordinates(selectedNode, selectedCanvasLineEnd);
                if (selectedCanvasLineEnd == 1)
                {
                    editLineProperties(selectedNode, //key
                        endPoint[0], //x1
                        endPoint[1], //y1
                        LineObjects[selectedNode].Realx2, //x2
                        LineObjects[selectedNode].Realy2 //y2
                        );
                }
                else if (selectedCanvasLineEnd == 2)
                {
                    editLineProperties(selectedNode, //key
                        LineObjects[selectedNode].Realx1, //x1
                        LineObjects[selectedNode].Realy1, //y1
                        endPoint[0], //x2
                        endPoint[1] //y2
                        );
                }
            }
        }
        public void resetSelectedLine()
        {
            numClicks = 0;
            selectedLinesTreeView.Nodes[0].Nodes.Clear();
            selectedLineObjects.Clear();
            selectedPointPos = new Point(0, 0);
            
            selectedNodeLabel.Text = "Selected node: [None]";
            selectedNodeLabel2.Text = "Selected node: [None]";

            btnCancelCanvasLineAction.Enabled = false;
            btnConfirmCanvasLineAction.Enabled = false;
            selectedLinesTreeView.Nodes[0].Nodes.Clear();
            
        }
        public void InitializeTempSelectableLines(List<int[]> lines) // Adds found lines to temporary selectable list
        {
            selectedLineObjects.Clear();

            foreach (var line in lines)
            {
                selectedLineObjects.Add(line[0], line[1]);
            }
            //updateSelectedLineTreeView();
            //linesDraw();
            //selectedLinesTreeView.Focus();
        }
        public void InitialiseTempSelectedLine(int key, int lineEnd)
        {
            selectedLineObjects.Add(key, lineEnd);
        }
        public void setSelectedNode(int nodeKey) //Changes varible and label to correspont to correct selectedNode
        {
            selectedNode = nodeKey;
            selectedNodeLabel.Text = "Selected node: [" + selectedNode + "]";
            selectedNodeLabel2.Text = "Selected node: [" + selectedNode + "]";
        }
        public void setSelectedNodeAndLineEnd(int nodeKey, int lineEnd) //Changes varible and label to correspont to correct selectedNode
        {
            selectedNode = nodeKey;
            selectedNodeLabel.Text = "Selected node: [" + selectedNode + "]; End: [" + lineEnd + "]";
            selectedNodeLabel2.Text = "Selected node: [" + selectedNode + "]; End: [" + lineEnd + "]";
        }
        public void setSelectedNodeText(string text) //Only changes the selected node lables' texts
        {
            selectedNodeLabel.Text = "Selected node: [" + text + "]";
            selectedNodeLabel2.Text = "Selected node: [" + text + "]";
        }
        public void linesDraw() //Forces canvas to redraw
        {
            if (true) //if (LineObjects.Keys.Count > 0)
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
        public void updateSelectedLineTreeView()
        {
            selectedLinesTreeView.Nodes[0].Nodes.Clear();
            if (selectedLineObjects.Count > 0) {
                foreach (var line in selectedLineObjects)
                {
                    int lineEnd = line.Value;
                    TreeNode node = new TreeNode();
                    node.Tag = line.Key;

                    if (canvasLineMode == "edit") // line edit mode
                    {
                        node.Text = "Line" + line.Key.ToString() + ", End:" + line.Value;
                        node.Name = "line:" + line.Key.ToString() + ";end:" + line.Value;
                    }
                    else if (canvasLineMode == "remove") // line remove mode
                    {
                        node.Text = "Line" + line.Key.ToString();
                        node.Name = "line:" + line.Key.ToString();
                    }

                    selectedLinesTreeView.Nodes[0].Nodes.Add(node);
                }
                selectedLinesTreeView.Nodes[0].Expand();
            }
        }
        public void setCanvasMode(string modeId)
        {
            if (modeId == "editLine") { canvasLineMode = "editLine"; }
            else if (modeId == "createLine") { canvasLineMode = "createLine"; }
            else if (modeId == "removeLine") { canvasLineMode = "removeLine"; }
            else if (modeId == "createPolygon") { canvasLineMode = "createPolygon"; }
            resetSelectedLine();

        }
        
        //
        // Canvas drawing handling ------------------------------------------
        //
        private void lineCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (LineObjects.Keys.Count > 0) 
            {
                this.SuspendLayout();
                Graphics g = e.Graphics; //Create drawing canvas

                foreach (var lineObject in LineObjects) //Extract every line from list and draw them
                {
                    Point startPoint = new Point(lineObject.Value.Realx1, lineObject.Value.Realy1);
                    Point endPoint = new Point(lineObject.Value.Realx2, lineObject.Value.Realy2);

                    Pen pen = new Pen(Brushes.Black, lineWidth);

                    g.DrawLine(pen, startPoint, endPoint); //Draw Line from start to end points

                    if (DebugMode == true) //Draw line key on canvas
                    {
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
                        else if (kValue > -1 && kValue < 1)
                        {
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
                if (DebugMode == true) //Selection Point Handling
                {
                    Rectangle rec = new Rectangle(); // Draws out a rectangle on top of selected line's line end to visualy show selected line end
                    Pen pen = new Pen(Brushes.Gray, 1);
                    Pen pen2 = new Pen(Brushes.Gray, 1);

                    int xPos = selectedPointPos.X - minSelectDistance / 2;
                    int yPos = selectedPointPos.Y - minSelectDistance / 2;
                    int xPos2 = selectedPointPos.X;
                    int yPos2 = selectedPointPos.Y;

                    rec.X = xPos;
                    rec.Y = yPos;
                    rec.Width = minSelectDistance;
                    rec.Height = minSelectDistance;

                    Point temp1StartPos = new Point(0, yPos2);
                    Point temp1EndPos = new Point(Canvas.Width, yPos2);
                    Point temp2StartPos = new Point(xPos2, 0);
                    Point temp2EndPos = new Point(xPos2, Canvas.Height);

                    g.DrawLine(pen2, temp1StartPos, temp1EndPos);
                    g.DrawLine(pen2, temp2StartPos, temp2EndPos);
                    g.DrawRectangle(pen, rec);
                }
            }
        }

        //
        // Selecting line in treeView handling ------------------------------------------
        //
        private void linesTreeView_AfterSelect(object sender, TreeViewEventArgs e) //After tree view selecting node handling ------------------------------------------
        {
            if (linesTreeView.SelectedNode.Tag.ToString() != "main") {
                
                resetSelectedLine();
                setSelectedNode((int)linesTreeView.SelectedNode.Tag);
                lineKeyBox.Value = selectedNode;

                //if (!checkKeyAvailability(selectedNode)) //Checks if selected node exists so it can write to from existing LineObject
                if (true)
                {
                    if (LineObjects[selectedNode].Realx1 != 0) { x1Box.Text = LineObjects[selectedNode].Realx1.ToString(); }
                    if (LineObjects[selectedNode].Realy1 != 0) { y1Box.Text = LineObjects[selectedNode].Realy1.ToString(); }
                    if (LineObjects[selectedNode].Realx2 != 0) { x2Box.Text = LineObjects[selectedNode].Realx2.ToString(); }
                    if (LineObjects[selectedNode].Realy2 != 0) { y2Box.Text = LineObjects[selectedNode].Realy2.ToString(); }
                }
            }
            else { generateException("invalid_selected_node"); } // invalid selected node
        }
        private void selectedLinesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (selectedLinesTreeView.SelectedNode.Tag.ToString() != "main") {
                if (true) //(numClicks == 0)
                {
                    setSelectedNode((int)selectedLinesTreeView.SelectedNode.Tag);
                    lineKeyBox.Value = selectedNode;
                    if (LineObjects[selectedNode].Realx1 != 0) { x1Box.Text = LineObjects[selectedNode].Realx1.ToString(); }
                    if (LineObjects[selectedNode].Realy1 != 0) { y1Box.Text = LineObjects[selectedNode].Realy1.ToString(); }
                    if (LineObjects[selectedNode].Realx2 != 0) { x2Box.Text = LineObjects[selectedNode].Realx2.ToString(); }
                    if (LineObjects[selectedNode].Realy2 != 0) { y2Box.Text = LineObjects[selectedNode].Realy2.ToString(); }

                    if (canvasLineMode == "edit" || canvasLineMode == "create") // line edit mode
                    {
                        if (lineMultiLocking == false)
                        {
                            selectedCanvasLineEnd = selectedLineObjects[selectedNode];
                            int[] endPoint = getLineEndCoordinates(selectedNode, selectedCanvasLineEnd);
                            selectedPointPos = new Point(endPoint[0], endPoint[1]);
                            //Switch with comments to these code bits to require confirm button press, maybe add an if statement here for a public option varible
                            numClicks = 1;
                            //btnConfirmCanvasLineAction.Enabled = true;
                        }
                        //else if (lineMultiLocking == true)
                        //{

                        //}
                    }
                    else if (canvasLineMode == "remove")
                    {
                        btnConfirmCanvasLineAction.Enabled = true;
                    }
                    linesDraw();
                }
            }
            else { generateException("invalid_selected_node"); } // invalid selected node
        }

        //
        // Mouse click on canvas handling ------------------------------------------
        //
       
        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            //onMouseClickGetCoordinates(e.Location.X, e.Location.Y);
            //int temp1 = getClosestLineAsKey(e.Location.X, e.Location.Y, out int LineEnd);
            //List<int[]> temp2 = getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, 10);
            //int[] temp3 = getClosestLinesAsArray(e.Location.X, e.Location.Y, 10);
            if (DebugMode == true)
            {
                if (numClicks == 0) //First mouse click
                {
                    if (canvasLineMode == "editLine") //Line edit mode ------------------------------------------------------------------------------------
                    {
                        List<int[]> lines = getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, minSelectDistance);
                        if (LineObjects.Count > 0)
                        {
                            if (lines.Count == 1) //Skips selected window and goes to numClick 2
                            {
                                if (true)
                                {
                                    int[] line = lines[0];
                                    //setSelectedNode(line[0]);
                                    setSelectedNodeAndLineEnd(line[0], line[1]);

                                    selectedCanvasLineEnd = line[1]; //Set to selected line end
                                    numClicks = 1;

                                    int[] endPoint = getLineEndCoordinates(selectedNode, selectedCanvasLineEnd);
                                    selectedPointPos = new Point(endPoint[0], endPoint[1]);

                                }
                            }
                            else if (lines.Count > 0)
                            {
                                int[] line = lines[0];
                                int[] endPoint = getLineEndCoordinates(line[0], line[1]);
                                selectedPointPos = new Point(endPoint[0], endPoint[1]);

                                InitializeTempSelectableLines(lines);
                                updateSelectedLineTreeView();

                                if (lineMultiLocking == true)
                                {
                                    numClicks = 1;
                                }
                            }
                            else { generateException("no_line_end_nearby"); } // no close line end
                            btnCancelCanvasLineAction.Enabled = true;
                            linesDraw();
                        }
                        else { generateException("no_existing_lines"); } // no existing lines
                    }
                    else if (canvasLineMode == "createLine") //Line create mode ------------------------------------------------------------------------------------
                    {
                        List<int[]> lines = getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, minSelectDistance);
                        if (lines.Count > 0 && lockInToLineEnds == true)
                        {
                            int[] endPoint = getClosestLineEndCoordinate(e.Location.X, e.Location.Y, minSelectDistance, out int lineEnd);
                            tempMouseStartPos = new Point(endPoint[0], endPoint[1]);
                            numClicks = 1;
                            selectedPointPos = new Point(endPoint[0], endPoint[1]);
                            setSelectedNodeText("..."); 
                        }
                        else //Executes if no nearby line is found, if lines <= 0
                        {
                            //generateException("");
                            tempMouseStartPos = new Point(e.Location.X, e.Location.Y);
                            selectedPointPos = new Point(e.Location.X, e.Location.Y);
                            numClicks = 1;
                            setSelectedNodeText("...");
                        }
                        linesDraw();
                        btnCancelCanvasLineAction.Enabled = true;
                    }
                    else if (canvasLineMode == "removeLine") //Line remove mode ------------------------------------------------------------------------------------
                    {
                        List<int[]> lines = getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, minSelectDistance);
                        if (LineObjects.Count > 0)
                        {                            
                            if (lines.Count == 1) //Skips selected window and goes to attempt to remove selected node
                            {
                                int[] line = lines[0];
                                setSelectedNode(line[0]);

                                bool tempSuccess;
                                removeLineByDialog(selectedNode, out tempSuccess);
                                if (tempSuccess) { btnCancelCanvasLineAction.Enabled = false; btnConfirmCanvasLineAction.Enabled = false; }
                            }
                            else if (lines.Count > 0)
                            {
                                InitializeTempSelectableLines(lines);
                                updateSelectedLineTreeView();
                            }
                            else { generateException("no_line_end_nearby"); } // no close line end
                            btnCancelCanvasLineAction.Enabled = true;
                            linesDraw();
                        }
                        else { generateException("no_existing_lines"); } // no existing lines
                    }
                    else if (canvasLineMode == "createPolygon") //Polygon create mode ------------------------------------------------------------------------------------
                    {
                        tempMouseStartPos = new Point(e.Location.X, e.Location.Y);
                        selectedPointPos = new Point(e.Location.X, e.Location.Y);
                        btnCancelCanvasLineAction.Enabled = true;
                        linesDraw();
                        numClicks = 1;
                    }
                    //btnCancelCanvasLineAction.Enabled = true;

                }
                else if (numClicks == 1) //Second mouse click
                {
                    if (canvasLineMode == "editLine") //Line edit mode ------------------------------------------------------------------------------------
                    {
                        if (LineObjects.Count > 0)
                        {
                            List<int[]> lines = getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, minSelectDistance);

                            if (lines.Count > 0 && lockInToLineEnds == true) //Checks if lines count is over zero and if lockInLineEnds is true
                            {
                                int[] endPoint = getClosestLineEndCoordinate(e.Location.X, e.Location.Y, minSelectDistance, out int lineEnd);
                                if (lineMultiLocking == false) //No MultiLineLocking 
                                {
                                    if (selectedCanvasLineEnd == 1)
                                    {
                                        editLineProperties(selectedNode, //key
                                            endPoint[0], //x1
                                            endPoint[1], //y1
                                            LineObjects[selectedNode].Realx2, //x2
                                            LineObjects[selectedNode].Realy2 //y2
                                            );
                                    }
                                    else if (selectedCanvasLineEnd == 2)
                                    {
                                        editLineProperties(selectedNode, //key
                                            LineObjects[selectedNode].Realx1, //x1
                                            LineObjects[selectedNode].Realy1, //y1
                                            endPoint[0], //x2
                                            endPoint[1] //y2
                                            );
                                    }
                                }
                                else if (lineMultiLocking == true) //With MultiLineLocking
                                {
                                    int[] lineEndPoint = { 0 , 0 };
                                    foreach (var line in selectedLineObjects) //Go through the dictionary and edit each line's coordinates to LockedIntoLineEnd's coordinate
                                    {
                                        lineEndPoint = getLineEndCoordinates(line.Key, line.Value);

                                        if (selectedCanvasLineEnd == 1)
                                        {
                                            editLineProperties(selectedNode, //key
                                                lineEndPoint[0], //x1
                                                lineEndPoint[1], //y1
                                                LineObjects[selectedNode].Realx2, //x2
                                                LineObjects[selectedNode].Realy2 //y2
                                                );
                                        }
                                        else if (selectedCanvasLineEnd == 2)
                                        {
                                            editLineProperties(selectedNode, //key
                                                LineObjects[selectedNode].Realx1, //x1
                                                LineObjects[selectedNode].Realy1, //y1
                                                lineEndPoint[0], //x2
                                                lineEndPoint[1] //y2
                                                );
                                        }
                                    }
                                }
                                selectedPointPos = new Point(endPoint[0], endPoint[1]); // viewing rectangle coordinates
                                setSelectedNodeText("...");
                                numClicks = 0;
                                linesDraw();

                            }
                            else //Executes if no nearby line is found, if lines <= 0 or if LoxkIntoLineEnds == false
                            {
                                if (lineMultiLocking == false) //No MultiLineLocking
                                {
                                    if (selectedCanvasLineEnd == 1) //checks if selected line end is first end
                                    {
                                        editLineProperties(
                                            selectedNode, //key
                                            e.Location.X, //x1
                                            e.Location.Y, //y1
                                            LineObjects[selectedNode].Realx2, //x2
                                            LineObjects[selectedNode].Realy2 //y2
                                            );
                                    }
                                    else if (selectedCanvasLineEnd == 2) //checks if selected line end is second end
                                    {
                                        editLineProperties(selectedNode, //key
                                            LineObjects[selectedNode].Realx1, //x1
                                            LineObjects[selectedNode].Realy1, //y1
                                            e.Location.X, //x2
                                            e.Location.Y //y2
                                            );
                                    }
                                }
                                else if (lineMultiLocking == true) //With MultiLineLocking
                                {
                                    foreach (var line in selectedLineObjects) //Go through the dictionary and edit each line's coordinates to LockedIntoLineEnd's coordinate
                                    {
                                        int tempLineEnd = line.Value;
                                        if (tempLineEnd == 1) //checks if event line end is first end
                                        {
                                            editLineProperties(
                                                line.Key, //key
                                                e.Location.X, //x1
                                                e.Location.Y, //y1
                                                LineObjects[line.Key].Realx2, //x2
                                                LineObjects[line.Key].Realy2 //y2
                                                );
                                        }
                                        else if (tempLineEnd == 2) //checks if event line end is second end
                                        {
                                            editLineProperties(line.Key, //key
                                                LineObjects[line.Key].Realx1, //x1
                                                LineObjects[line.Key].Realy1, //y1
                                                e.Location.X, //x2
                                                e.Location.Y //y2
                                                );
                                        }
                                    }
                                }
                                selectedPointPos = new Point(e.Location.X, e.Location.Y); // viewing rectangle coordinates
                                numClicks = 0;
                                linesDraw();
                            }
                            resetSelectedLine(); //Maybe add a if statement to a multi after eachother selecting option public varible
                            btnConfirmCanvasLineAction.Enabled = false;
                            btnCancelCanvasLineAction.Enabled = false;
                        }
                        else { generateException("no_existing_lines"); } // no existing lines
                    }
                    else if (canvasLineMode == "createLine") //Line create mode ------------------------------------------------------------------------------------
                    {
                        int key = getLowestAvailableKey();
                        List<int[]> lines = getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, minSelectDistance);
                        if (lines.Count > 0 && lockInToLineEnds == true) //Checks if lines count is over zero and if lockInLineEnds is true
                        {
                            int lineEnd = 0;
                            int[] endPoint = getClosestLineEndCoordinate(e.Location.X, e.Location.Y, minSelectDistance, out lineEnd);

                            tempMouseEndPos = new Point(endPoint[0], endPoint[1]);
                            createLine(key,
                                    tempMouseStartPos.X,
                                    tempMouseStartPos.Y,
                                    tempMouseEndPos.X,
                                    tempMouseEndPos.Y);
                            
                            selectedPointPos = new Point(endPoint[0], endPoint[1]); // viewing rectangle coordinates
                            numClicks = 0;
                            linesDraw();
                        }
                        else //Executes if no nearby line is found, if lines <= 0
                        {
                            tempMouseEndPos = new Point(e.Location.X, e.Location.Y);

                            createLine(key,
                                tempMouseStartPos.X,
                                tempMouseStartPos.Y,
                                tempMouseEndPos.X,
                                tempMouseEndPos.Y
                                );
                            numClicks = 0;
                            selectedPointPos = new Point(e.Location.X, e.Location.Y); // viewing rectangle coordinates
                            linesDraw();
                        }
                        resetSelectedLine();
                        btnConfirmCanvasLineAction.Enabled = false; //Disables the buttons
                        btnCancelCanvasLineAction.Enabled = false;
                    }
                    else if (canvasLineMode == "removeLine") //Line remove mode ------------------------------------------------------------------------------------
                    {

                    }
                    else if (canvasLineMode == "createPolygon") //Polygon create mode ------------------------------------------------------------------------------------
                    {
                        tempMouseEndPos = new Point(e.Location.X, e.Location.Y);
                        radiusPolygon = (int)Math.Sqrt(
                            Math.Pow((tempMouseEndPos.X - tempMouseStartPos.X), 2) + 
                            Math.Pow((tempMouseEndPos.Y - tempMouseStartPos.Y), 2));
                        double angleExtension = 0; //Implement way to get what angle the mouse is to make the polygon rotated appropriate

                        Dictionary<int, (Point, Point)> polygonLines = createPolygonLines(tempMouseStartPos.X, tempMouseStartPos.Y, numPolygonCorners, radiusPolygon, angleExtension);
                        InitialisePolygon(polygonLines);

                        selectedPointPos = new Point(e.Location.X, e.Location.Y); // viewing rectangle coordinates
                        linesDraw();
                        updateLineTreeView();
                    }
                }
                //resetSelectedLine();
            }
        }

        //
        // Event Handling ------------------------------------------------------------------------------------
        //
        private void MainWindow_Load(object sender, EventArgs e)
        {
            //InitializeLineObject(10, 20, 120, 80);
            //BeginGraphics();
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            int tempX1; int tempY1; int tempX2; int tempY2; int tempLineKeyBox; //Temporary varibles

            if (int.TryParse(x1Box.Text, out tempX1)) { } else { tempX1 = 1; }
            if (int.TryParse(y1Box.Text, out tempY1)) { } else { tempY1 = 1; }
            if (int.TryParse(x2Box.Text, out tempX2)) { } else { tempX2 = 2; }
            if (int.TryParse(y2Box.Text, out tempY2)) { } else { tempY2 = 2; }
            tempLineKeyBox = ((int)lineKeyBox.Value);

            createLine(tempLineKeyBox, tempX1, tempY1, tempX2, tempY2);
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
        }
        private void btnDraw_Click(object sender, EventArgs e)
        {
            //BeginGraphics();
            linesDraw();
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
        private void checkBoxLabelLines_CheckedChanged(object sender, EventArgs e)
        {
            DebugMode = checkBoxDebugMode.Checked;
            linesDraw();
        }
        private void checkBoxLockInToLineEnds_CheckedChanged(object sender, EventArgs e)
        {
            //lockInToLineEnds = checkBoxDebugMode.Checked;
            if (checkBoxDebugMode.CheckState == CheckState.Checked) { lockInToLineEnds = true; }
            else if (checkBoxDebugMode.CheckState == CheckState.Unchecked) { lockInToLineEnds = false; }
            
        }
        private void radioBtnEditLineMode_CheckedChanged(object sender, EventArgs e)
        {
            setCanvasMode("editLine");
        }
        private void radioBtnCreateLineMode_CheckedChanged(object sender, EventArgs e)
        {
            setCanvasMode("createLine");
        }
        private void radioBtnRemoveLineMode_CheckedChanged(object sender, EventArgs e)
        {
            setCanvasMode("removeLine");
        }
        private void minSelectDistanceBox_ValueChanged(object sender, EventArgs e)
        {
            minSelectDistance = (int)minSelectDistanceBox.Value;
            linesDraw();
        }
        private void btnCancelCanvasLineAction_Click(object sender, EventArgs e)
        {
            resetSelectedLine();
            
            linesDraw();
        }
        private void btnConfirmCanvasLineAction_Click(object sender, EventArgs e)
        {
            if (canvasLineMode == "editLine")
            {
                numClicks = 1;

                btnConfirmCanvasLineAction.Enabled = false;
                selectedLinesTreeView.Nodes[0].Nodes.Clear();
            }
            else if (canvasLineMode == "removeLine")
            {
                bool tempSuccess = false;
                removeLineByDialog(selectedNode, out tempSuccess);
                if (tempSuccess) { btnCancelCanvasLineAction.Enabled = false; btnConfirmCanvasLineAction.Enabled = false; }
            }
        }
        private void lineMultiLockingBox_CheckedChanged(object sender, EventArgs e)
        {
            lineMultiLocking = lineMultiLockingBox.Checked;
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            setCanvasMode("createPolygon");
        }
    }
}
