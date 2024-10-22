using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LineDrawerDemo.MainWindow;
using static System.Net.WebRequestMethods;

namespace LineDrawerDemo
{
    public partial class MainWindow : Form
    {
        //Neccesarry Public Varibles
        public int selectedCanvasLineEnd = 0;
        public int numClicks = 0;
        public bool mouseClicked = false;
        public Point tempMouseStartPos = new Point();
        public Point tempMouseEndPos = new Point();
        public Point selectedPointPos = new Point();
        public int selectedNode;
        public bool lockInToLineEnds = false;
        public bool DebugMode = false;
        public bool editLineMode = false;
        public bool createLineMode = false;
        public bool removeLineMode = false;
        public string canvasLineMode = "";
        public string fileLocation = Application.StartupPath;
        public int minSelectDistance = 10;
        public int lineWidth = 1;
        Dictionary<int, LineObject> LineObjects = new Dictionary<int, LineObject>();
        Dictionary<int, int> selectedLineObjects = new Dictionary<int, int>();

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
        // Basic line functions handling ------------------------------------------
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
        public void resetSelectedLine()
        {
            numClicks = 0;
            selectedLinesTreeView.Nodes[0].Nodes.Clear();
            selectedNodeLabel.Text = "Selected node: [None]";
            selectedNodeLabel2.Text = "Selected node: [None]";
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
        public void setSelectedNodeText(string text) //Only changes the selected node lables' texts
        {
            selectedNodeLabel.Text = "Selected node: [" + text + "]";
            selectedNodeLabel2.Text = "Selected node: [" + text + "]";
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
                        node.Name = "line:" + line.Key.ToString() + ":" + line.Value;
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

        //
        // Canvas drawing handling ------------------------------------------
        //
        private void lineCanvas_Paint(object sender, PaintEventArgs e)
        {
            this.SuspendLayout();
            Graphics g = e.Graphics; //Create drawing canvas

            foreach (var lineObject in LineObjects) //Extract every line from list and draw them
            {
                Point startPoint = new Point(lineObject.Value.Realx1, lineObject.Value.Realy1);
                Point endPoint = new Point(lineObject.Value.Realx2, lineObject.Value.Realy2);

                Pen pen = new Pen(Brushes.Black, lineWidth);

                g.DrawLine(pen, startPoint, endPoint); //Draw Line from start to end points

                if (DebugMode == true)
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
            if (DebugMode == true)
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
        //
        // ------------------------------------------
        //
        private void linesTreeView_AfterSelect(object sender, TreeViewEventArgs e) //After tree view selecting node handling ------------------------------------------
        {
            if (linesTreeView.SelectedNode.Tag.ToString() != "main") {

                setSelectedNode((int)linesTreeView.SelectedNode.Tag);
                lineKeyBox.Value = selectedNode;
                resetSelectedLine();

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

                setSelectedNode((int)selectedLinesTreeView.SelectedNode.Tag);
                lineKeyBox.Value = selectedNode;
                if (LineObjects[selectedNode].Realx1 != 0) { x1Box.Text = LineObjects[selectedNode].Realx1.ToString(); }
                if (LineObjects[selectedNode].Realy1 != 0) { y1Box.Text = LineObjects[selectedNode].Realy1.ToString(); }
                if (LineObjects[selectedNode].Realx2 != 0) { x2Box.Text = LineObjects[selectedNode].Realx2.ToString(); }
                if (LineObjects[selectedNode].Realy2 != 0) { y2Box.Text = LineObjects[selectedNode].Realy2.ToString(); }

                if (canvasLineMode == "edit" || canvasLineMode == "create") // line edit mode
                { 
                    selectedCanvasLineEnd = selectedLineObjects[selectedNode];
                    int[] endPoint = getLineEndCoordinates(selectedNode, selectedCanvasLineEnd);
                    selectedPointPos = new Point(endPoint[0], endPoint[1]);
                    numClicks = 1;
                }
                else if (canvasLineMode == "remove")
                {
                    btnConfirmCanvasLineAction.Enabled = true;
                }

                linesDraw();
            }
            else { generateException("invalid_selected_node"); } // invalid selected node
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

        //
        // Mouse click on canvas handling ------------------------------------------
        //
        public void InitializeTempSelectableLines(List<int[]> lines) // Adds found lines to temporary selectable list, if only one line is found it won't add to list
        {
            if (lines.Count == 1)
            {
                int[] line = lines[0];
                setSelectedNode(line[0]); //Set selected node varible and labels
                if (canvasLineMode == "edit") // line edit mode
                {
                    selectedCanvasLineEnd = line[1]; //Set to selected line end
                    numClicks = 1;

                    int[] endPoint = getLineEndCoordinates(selectedNode, selectedCanvasLineEnd);
                    selectedPointPos = new Point(endPoint[0], endPoint[1]);
                }
                else if (canvasLineMode == "remove") // line remove mode
                {
                    bool tempSuccess;
                    removeLineByDialog(selectedNode, out tempSuccess);
                    if (tempSuccess) { btnCancelCanvasLineAction.Enabled = false; btnConfirmCanvasLineAction.Enabled = false; }
                }

                linesDraw();
            }
            else if (lines.Count > 0)
            {
                selectedLineObjects.Clear();

                foreach (var line in lines)
                {
                    selectedLineObjects.Add(line[0], line[1]);
                }
                selectedLinesTreeView.Nodes[0].Nodes.Clear();
                updateSelectedLineTreeView();
                linesDraw();
                //selectedLinesTreeView.Focus();
            }
            else { generateException("no_line_end_nearby"); } // no close line end
        }
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
                    if (canvasLineMode == "edit") //Line edit mode ------------------------------------------------------------------------------------
                    {
                        List<int[]> lines = getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, minSelectDistance);
                        if (LineObjects.Count > 0)
                        {
                            InitializeTempSelectableLines(lines);
                        }
                        else { generateException("no_existing_lines"); } // no existing lines
                        linesDraw();
                    }
                    else if (canvasLineMode == "create") //Line create mode ------------------------------------------------------------------------------------
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
                    }
                    else if (canvasLineMode == "removeLine") //Line remove mode ------------------------------------------------------------------------------------
                    {
                        List<int[]> lines = getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, minSelectDistance);
                        if (LineObjects.Count > 0)
                        {
                            InitializeTempSelectableLines(lines);
                        }
                        else { generateException("no_existing_lines"); } // no existing lines

                        linesDraw();
                    }
                    btnCancelCanvasLineAction.Enabled = true;
                }
                else if (numClicks == 1) //Second mouse click
                {
                    if (canvasLineMode == "edit") //Line edit mode ------------------------------------------------------------------------------------
                    {
                        if (LineObjects.Count > 0)
                        {
                            List<int[]> lines = getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, minSelectDistance);

                            if (lines.Count > 0 && lockInToLineEnds == true) //Checks if lines count is over zero and if lockInLineEnds is true
                            {
                                int[] endPoint = getClosestLineEndCoordinate(e.Location.X, e.Location.Y, minSelectDistance, out int lineEnd);
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
                                selectedPointPos = new Point(endPoint[0], endPoint[1]); // viewing rectangle coordinates
                                numClicks = 0;
                                linesDraw();
                            }
                            else //Executes if no nearby line is found, if lines <= 0
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
                                selectedPointPos = new Point(e.Location.X, e.Location.Y); // viewing rectangle coordinates
                                numClicks = 0;
                                linesDraw();
                            }
                            resetSelectedLine(); //Maybe add a if statement to a multi after eachother selecting option public varible
                        }
                        else { generateException("no_existing_lines"); } // no existing lines
                    }
                    else if (canvasLineMode == "create") //Line create mode ------------------------------------------------------------------------------------
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
                    }
                    else if (canvasLineMode == "remove") //Line remove mode ------------------------------------------------------------------------------------
                    {

                    }
                }
                //resetSelectedLine();
            }
        }

        //
        // ------------------------------------------
        //
        private void checkBoxLabelLines_CheckedChanged(object sender, EventArgs e)
        {
            DebugMode = checkBoxDebugMode.Checked;
            linesDraw();
        }
        public void setCanvasMode(string modeId)
        {
            if (modeId == "edit") {
                createLineMode = false;
                editLineMode = true;
                removeLineMode = false;

                canvasLineMode = "edit";
            }
            else if (modeId == "create") {
                removeLineMode = true;
                editLineMode = false;
                removeLineMode = false;

                canvasLineMode = "create";
            }
            else if (modeId == "remove") {
                createLineMode = false;
                editLineMode = false;
                removeLineMode = true;

                canvasLineMode = "remove";
            }
            resetSelectedLine();
        }
        private void checkBoxLockInToLineEnds_CheckedChanged(object sender, EventArgs e)
        {
            //lockInToLineEnds = checkBoxDebugMode.Checked;
            if (checkBoxDebugMode.CheckState == CheckState.Checked) { lockInToLineEnds = true; }
            else if (checkBoxDebugMode.CheckState == CheckState.Unchecked) { lockInToLineEnds = false; }
            
        }
        private void radioBtnEditLineMode_CheckedChanged(object sender, EventArgs e)
        {
            setCanvasMode("edit");
        }
        private void radioBtnCreateLineMode_CheckedChanged(object sender, EventArgs e)
        {
            setCanvasMode("create");
        }
        private void radioBtnRemoveLineMode_CheckedChanged(object sender, EventArgs e)
        {
            setCanvasMode("remove");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            generateException("test_id");
        }

        private void minSelectDistanceBox_ValueChanged(object sender, EventArgs e)
        {
            minSelectDistance = (int)minSelectDistanceBox.Value;
            linesDraw();
        }

        private void btnCancelCanvasLineAction_Click(object sender, EventArgs e)
        {
            resetSelectedLine();
            btnCancelCanvasLineAction.Enabled = false;
            selectedPointPos = new Point(0, 0);
            linesDraw();
        }

        private void btnConfirmCanvasLineAction_Click(object sender, EventArgs e)
        {
            removeLineByDialog(selectedNode, out bool tempSuccess);
            btnConfirmCanvasLineAction.Enabled = false;
        }
    }
}
