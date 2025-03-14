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

namespace LineDrawerDemo
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        LineDrawerDemo.ExceptionHandling exception = LineDrawerDemo.ExceptionHandling.GetInstance();
        CanvasHandling canvasHandle = new CanvasHandling();

        //
        // Necessary miscellaneous public handling ------------------------------------------
        //
        public void setCanvasMode(CanvasModes canvasMode) //string modeId)
        {
            canvasHandle.canvasLineMode = canvasMode;

            //if (modeId == "editLine") { canvasHandle.canvasLineMode = "editLine"; }
            //else if (modeId == "createLine") { canvasHandle.canvasLineMode = "createLine"; }
            //else if (modeId == "removeLine") { canvasHandle.canvasLineMode = "removeLine"; }
            //else if (modeId == "createPolygon") { canvasHandle.canvasLineMode = "createPolygon"; }
        }
        public void resetSelectedLine()
        {
            canvasHandle.numClicks = 0;

            canvasHandle.selectedPointPos = new Point(-20, -20);

            selectedNodeLabel.Text = "Selected node: [None]";
            selectedNodeLabel2.Text = "Selected node: [None]";

            btnCancelCanvasLineAction.Enabled = false;
            btnConfirmCanvasLineAction.Enabled = false;

            selectedLinesTreeView.Nodes[0].Nodes.Clear();
            canvasHandle.selectedLineObjects.Clear();
        }
        public void clearSelectedLineObjects()
        {
            selectedLinesTreeView.Nodes[0].Nodes.Clear();
            canvasHandle.selectedLineObjects.Clear();
        }
        public void setSelectedNode(int nodeKey) //Changes varible and label to correspont to correct selectedNode
        {
            canvasHandle.selectedNode = nodeKey;
            selectedNodeLabel.Text = "Selected node: [" + canvasHandle.selectedNode + "]";
            selectedNodeLabel2.Text = "Selected node: [" + canvasHandle.selectedNode + "]";
        }
        public void setSelectedNodeAndLineEnd(int nodeKey, int lineEnd) //Changes varible and label to correspont to correct selectedNode
        {
            canvasHandle.selectedNode = nodeKey;
            selectedNodeLabel.Text = "Selected node: [" + canvasHandle.selectedNode + "]; End: [" + lineEnd + "]";
            selectedNodeLabel2.Text = "Selected node: [" + canvasHandle.selectedNode + "]; End: [" + lineEnd + "]";
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

            foreach (var lineObject in canvasHandle.lineHandle.LineObjects.Keys)
            {
                TreeNode node = new TreeNode();
                node.Text = "Line" + lineObject.ToString();
                node.Name = "line" + lineObject.ToString();
                node.Tag = lineObject;
                linesTreeView.Nodes[0].Nodes.Add(node);

            }
            linesTreeView.Nodes[0].Expand();
        }

        //Attempt to implement multi selecting and action.ing
        public void InitiateMultiLineMove(List<int[]> lines) // Potential multi line move function
        {
            foreach (var line_ in lines)
            {
                int[] line = line_ as int[];
                setSelectedNode(line[0]);
                canvasHandle.selectedCanvasLineEnd = line[1];
                int[] endPoint = canvasHandle.getLineEndCoordinates(canvasHandle.selectedNode, canvasHandle.selectedCanvasLineEnd);
                if (canvasHandle.selectedCanvasLineEnd == 1)
                {
                    canvasHandle.lineHandle.editLineProperties(canvasHandle.selectedNode, //key
                        endPoint[0], //x1
                        endPoint[1], //y1
                        canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realx2, //x2
                        canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realy2 //y2
                        );
                }
                else if (canvasHandle.selectedCanvasLineEnd == 2)
                {
                    canvasHandle.lineHandle.editLineProperties(canvasHandle.selectedNode, //key
                        canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realx1, //x1
                        canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realy1, //y1
                        endPoint[0], //x2
                        endPoint[1] //y2
                        );
                }
            }
        }
        
        public void updateSelectedLineTreeView()
        {
            selectedLinesTreeView.Nodes[0].Nodes.Clear();
            if (canvasHandle.selectedLineObjects.Count > 0) {
                foreach (var line in canvasHandle.selectedLineObjects)
                {
                    int lineEnd = line.Value;
                    TreeNode node = new TreeNode();
                    node.Tag = line.Key;

                    if (canvasHandle.canvasLineMode == CanvasModes.editLine) // line edit mode
                    {
                        node.Text = "Line" + line.Key.ToString() + ", End:" + line.Value;
                        node.Name = "line:" + line.Key.ToString() + ";end:" + line.Value;
                    }
                    else if (canvasHandle.canvasLineMode == CanvasModes.removeLine) // line remove mode
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
        // Canvas drawing handling ------------------------------------------
        //
        private void lineCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (canvasHandle.lineHandle.LineObjects.Keys.Count > 0) 
            {
                this.SuspendLayout();
                Graphics g = e.Graphics; //Create drawing canvas

                foreach (var lineObject in canvasHandle.lineHandle.LineObjects) //Extract every line from list and draw them
                {
                    Point startPoint = new Point(lineObject.Value.Realx1, lineObject.Value.Realy1);
                    Point endPoint = new Point(lineObject.Value.Realx2, lineObject.Value.Realy2);

                    Pen pen = new Pen(Brushes.Black, canvasHandle.lineWidth);

                    g.DrawLine(pen, startPoint, endPoint); //Draw Line from start to end points

                    if (canvasHandle.DebugMode == true) //Draw line key on canvas
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
                if (canvasHandle.DebugMode == true) //Selection Point Handling
                {
                    Rectangle rec = new Rectangle(); // Draws out a rectangle on top of selected line's line end to visualy show selected line end
                    Pen pen = new Pen(Brushes.Gray, 1);
                    Pen pen2 = new Pen(Brushes.Gray, 1);

                    int xPos = canvasHandle.selectedPointPos.X - canvasHandle.minSelectDistance / 2;
                    int yPos = canvasHandle.selectedPointPos.Y - canvasHandle.minSelectDistance / 2;
                    int xPos2 = canvasHandle.selectedPointPos.X;
                    int yPos2 = canvasHandle.selectedPointPos.Y;

                    rec.X = xPos;
                    rec.Y = yPos;
                    rec.Width = canvasHandle.minSelectDistance;
                    rec.Height = canvasHandle.minSelectDistance;

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
                clearSelectedLineObjects();
                setSelectedNode((int)linesTreeView.SelectedNode.Tag);
                lineKeyBox.Value = canvasHandle.selectedNode;

                //if (!checkKeyAvailability(selectedNode)) //Checks if selected node exists so it can write to from existing LineObject
                if (true)
                {
                    if (canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realx1 != 0) { x1Box.Text = canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realx1.ToString(); }
                    if (canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realy1 != 0) { y1Box.Text = canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realy1.ToString(); }
                    if (canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realx2 != 0) { x2Box.Text = canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realx2.ToString(); }
                    if (canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realy2 != 0) { y2Box.Text = canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realy2.ToString(); }
                }
            }
            else {exception.generateException(CustomExceptions.no_existing_lines); } // invalid selected node
        }
        private void selectedLinesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (selectedLinesTreeView.SelectedNode.Tag.ToString() != "main") {
                if (true) //(numClicks == 0)
                {
                    setSelectedNode((int)selectedLinesTreeView.SelectedNode.Tag);
                    lineKeyBox.Value = canvasHandle.selectedNode;
                    if (canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realx1 != 0) { x1Box.Text = canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realx1.ToString(); }
                    if (canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realy1 != 0) { y1Box.Text = canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realy1.ToString(); }
                    if (canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realx2 != 0) { x2Box.Text = canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realx2.ToString(); }
                    if (canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realy2 != 0) { y2Box.Text = canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realy2.ToString(); }

                    if (canvasHandle.canvasLineMode == CanvasModes.editLine || canvasHandle.canvasLineMode == CanvasModes.createLine) // line edit mode
                    {
                        if (canvasHandle.lineMultiLocking == false)
                        {
                            canvasHandle.selectedCanvasLineEnd = canvasHandle.selectedLineObjects[canvasHandle.selectedNode];
                            int[] endPoint = canvasHandle.getLineEndCoordinates(canvasHandle.selectedNode, canvasHandle.selectedCanvasLineEnd);
                            canvasHandle.selectedPointPos = new Point(endPoint[0], endPoint[1]);
                            //Switch with comments to these code bits to require confirm button press, maybe add an if statement here for a public option varible
                            //canvasHandle.numClicks = 1;
                            btnConfirmCanvasLineAction.Enabled = true;
                        }
                        //else if (lineMultiLocking == true)
                        //{

                        //}
                    }
                    else if (canvasHandle.canvasLineMode == CanvasModes.removeLine)
                    {
                        btnConfirmCanvasLineAction.Enabled = true;
                        //resetSelectedLine();
                    }
                    linesDraw();
                }
            }
            else { exception.generateException(CustomExceptions.invalid_selected_node); } // invalid selected node
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
            if (canvasHandle.DebugMode == true)
            {
                if (canvasHandle.numClicks == 0) //First mouse click
                {
                    if (canvasHandle.canvasLineMode == CanvasModes.editLine) //Line edit mode ------------------------------------------------------------------------------------
                    {
                        List<int[]> lines = canvasHandle.getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, canvasHandle.minSelectDistance);
                        if (canvasHandle.lineHandle.LineObjects.Count > 0)
                        {
                            if (lines.Count == 1) //Skips selected window and goes to numClick 2
                            {
                                if (true)
                                {
                                    int[] line = lines[0];
                                    //setSelectedNode(line[0]);
                                    setSelectedNodeAndLineEnd(line[0], line[1]);

                                    canvasHandle.selectedCanvasLineEnd = line[1]; //Set to selected line end
                                    canvasHandle.numClicks = 1;

                                    int[] endPoint = canvasHandle.getLineEndCoordinates(canvasHandle.selectedNode, canvasHandle.selectedCanvasLineEnd);
                                    canvasHandle.selectedPointPos = new Point(endPoint[0], endPoint[1]);

                                }
                            }
                            else if (lines.Count > 0)
                            {
                                int[] line = lines[0];
                                int[] endPoint = canvasHandle.getLineEndCoordinates(line[0], line[1]);
                                canvasHandle.selectedPointPos = new Point(endPoint[0], endPoint[1]);

                                canvasHandle.InitializeTempSelectableLines(lines);
                                
                                //
                                // GUI Redraw
                                //
                                updateSelectedLineTreeView();
                                //selectedLinesTreeView.Focus();
                                linesDraw();
                                //

                                if (canvasHandle.lineMultiLocking == true)
                                {
                                    canvasHandle.numClicks = 1;
                                }
                            }
                            else { exception.generateException(CustomExceptions.no_line_end_nearby); } // no close line end

                            btnCancelCanvasLineAction.Enabled = true;

                            //
                            // GUI Redraw
                            //
                            linesDraw();
                            //
                        }
                        else { exception.generateException(CustomExceptions.no_existing_lines); } // no existing lines
                    }
                    else if (canvasHandle.canvasLineMode == CanvasModes.createLine) //Line create mode ------------------------------------------------------------------------------------
                    {
                        List<int[]> lines = canvasHandle.getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, canvasHandle.minSelectDistance);
                        if (lines.Count > 0 && canvasHandle.lockInToLineEnds == true)
                        {
                            int[] endPoint = canvasHandle.getClosestLineEndCoordinate(e.Location.X, e.Location.Y, canvasHandle.minSelectDistance, out int lineEnd);
                            canvasHandle.tempMouseStartPos = new Point(endPoint[0], endPoint[1]);
                            canvasHandle.numClicks = 1;
                            canvasHandle.selectedPointPos = new Point(endPoint[0], endPoint[1]);
                        }
                        else //Executes if no nearby line is found, if lines <= 0
                        {
                            //generateException("");
                            canvasHandle.tempMouseStartPos = new Point(e.Location.X, e.Location.Y);
                            canvasHandle.selectedPointPos = new Point(e.Location.X, e.Location.Y);
                            canvasHandle.numClicks = 1;
                        }

                        btnCancelCanvasLineAction.Enabled = true;

                        //
                        // GUI Redraw
                        //
                        setSelectedNodeText("...");
                        linesDraw();
                        //
                    }
                    else if (canvasHandle.canvasLineMode == CanvasModes.removeLine) //Line remove mode ------------------------------------------------------------------------------------
                    {
                        List<int[]> lines = canvasHandle.getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, canvasHandle.minSelectDistance);
                        if (canvasHandle.lineHandle.LineObjects.Count > 0)
                        {                            
                            if (lines.Count == 1) //Skips selected window and goes to attempt to remove selected node
                            {
                                int[] line = lines[0];
                                setSelectedNode(line[0]);

                                bool tempSuccess;
                                canvasHandle.removeLineByDialog(canvasHandle.selectedNode, out tempSuccess);
                                if (tempSuccess) { btnCancelCanvasLineAction.Enabled = false; btnConfirmCanvasLineAction.Enabled = false; }
                                resetSelectedLine();
                            }
                            else if (lines.Count > 0)
                            {
                                canvasHandle.InitializeTempSelectableLines(lines);
                                updateSelectedLineTreeView();
                            }
                            else { exception.generateException(CustomExceptions.no_line_end_nearby); } // no close line end
                            //btnCancelCanvasLineAction.Enabled = true;

                            //
                            // GUI Redraw
                            //
                            updateLineTreeView();
                            linesDraw();
                            //

                        }
                        else { exception.generateException("no_existing_lines"); } // no existing lines
                    }
                    else if (canvasHandle.canvasLineMode == CanvasModes.createPolygon) //Polygon create mode ------------------------------------------------------------------------------------
                    {
                        canvasHandle.tempMouseStartPos = new Point(e.Location.X, e.Location.Y);
                        canvasHandle.selectedPointPos = new Point(e.Location.X, e.Location.Y);
                        btnCancelCanvasLineAction.Enabled = true;

                        canvasHandle.numClicks = 1;

                        //
                        // GUI Redraw
                        //
                        linesDraw();
                        //
                    }
                    //btnCancelCanvasLineAction.Enabled = true;

                }
                else if (canvasHandle.numClicks == 1) //Second mouse click
                {
                    if (canvasHandle.canvasLineMode == CanvasModes.editLine) //Line edit mode ------------------------------------------------------------------------------------
                    {
                        if (canvasHandle.lineHandle.LineObjects.Count > 0)
                        {
                            List<int[]> lines = canvasHandle.getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, canvasHandle.minSelectDistance);

                            if (lines.Count > 0 && canvasHandle.lockInToLineEnds == true) //Checks if lines count is over zero and if lockInLineEnds is true
                            {
                                int[] endPoint = canvasHandle.getClosestLineEndCoordinate(e.Location.X, e.Location.Y, canvasHandle.minSelectDistance, out int lineEnd);
                                if (canvasHandle.lineMultiLocking == false) //No MultiLineLocking 
                                {
                                    if (canvasHandle.selectedCanvasLineEnd == 1)
                                    {
                                        canvasHandle.lineHandle.editLineProperties(canvasHandle.selectedNode, //key
                                            endPoint[0], //x1
                                            endPoint[1], //y1
                                            canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realx2, //x2
                                            canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realy2 //y2
                                            );
                                    }
                                    else if (canvasHandle.selectedCanvasLineEnd == 2)
                                    {
                                        canvasHandle.lineHandle.editLineProperties(canvasHandle.selectedNode, //key
                                            canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realx1, //x1
                                            canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realy1, //y1
                                            endPoint[0], //x2
                                            endPoint[1] //y2
                                            );
                                    }
                                }
                                else if (canvasHandle.lineMultiLocking == true) //With MultiLineLocking
                                {
                                    int[] lineEndPoint = { 0 , 0 };
                                    foreach (var line in canvasHandle.selectedLineObjects) //Go through the dictionary and edit each line's coordinates to LockedIntoLineEnd's coordinate
                                    {
                                        lineEndPoint = canvasHandle.getLineEndCoordinates(line.Key, line.Value);

                                        if (canvasHandle.selectedCanvasLineEnd == 1)
                                        {
                                            canvasHandle.lineHandle.editLineProperties(canvasHandle.selectedNode, //key
                                                lineEndPoint[0], //x1
                                                lineEndPoint[1], //y1
                                                canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realx2, //x2
                                                canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realy2 //y2
                                                );
                                        }
                                        else if (canvasHandle.selectedCanvasLineEnd == 2)
                                        {
                                            canvasHandle.lineHandle.editLineProperties(canvasHandle.selectedNode, //key
                                                canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realx1, //x1
                                                canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realy1, //y1
                                                lineEndPoint[0], //x2
                                                lineEndPoint[1] //y2
                                                );
                                        }
                                    }
                                }

                                resetSelectedLine(); //Executes before "overriding" code

                                canvasHandle.selectedPointPos = new Point(endPoint[0], endPoint[1]); // viewing rectangle coordinates
                                //canvasHandle.numClicks = 0;

                            }
                            else //Executes if no nearby line is found, if lines <= 0 or if LockIntoLineEnds == false
                            {
                                if (canvasHandle.lineMultiLocking == false) //No MultiLineLocking
                                {
                                    if (canvasHandle.selectedCanvasLineEnd == 1) //checks if selected line end is first end
                                    {
                                        canvasHandle.lineHandle.editLineProperties(
                                            canvasHandle.selectedNode, //key
                                            e.Location.X, //x1
                                            e.Location.Y, //y1
                                            canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realx2, //x2
                                            canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realy2 //y2
                                            );
                                    }
                                    else if (canvasHandle.selectedCanvasLineEnd == 2) //checks if selected line end is second end
                                    {
                                        canvasHandle.lineHandle.editLineProperties(canvasHandle.selectedNode, //key
                                            canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realx1, //x1
                                            canvasHandle.lineHandle.LineObjects[canvasHandle.selectedNode].Realy1, //y1
                                            e.Location.X, //x2
                                            e.Location.Y //y2
                                            );
                                    }
                                }
                                else if (canvasHandle.lineMultiLocking == true) //With MultiLineLocking
                                {
                                    foreach (var line in canvasHandle.selectedLineObjects) //Go through the dictionary and edit each line's coordinates to LockedIntoLineEnd's coordinate
                                    {
                                        int tempLineEnd = line.Value;
                                        if (tempLineEnd == 1) //checks if event line end is first end
                                        {
                                            canvasHandle.lineHandle.editLineProperties(
                                                line.Key, //key
                                                e.Location.X, //x1
                                                e.Location.Y, //y1
                                                canvasHandle.lineHandle.LineObjects[line.Key].Realx2, //x2
                                                canvasHandle.lineHandle.LineObjects[line.Key].Realy2 //y2
                                                );
                                        }
                                        else if (tempLineEnd == 2) //checks if event line end is second end
                                        {
                                            canvasHandle.lineHandle.editLineProperties(line.Key, //key
                                                canvasHandle.lineHandle.LineObjects[line.Key].Realx1, //x1
                                                canvasHandle.lineHandle.LineObjects[line.Key].Realy1, //y1
                                                e.Location.X, //x2
                                                e.Location.Y //y2
                                                );
                                        }
                                    }
                                }

                                resetSelectedLine(); //Executes before "overriding" code

                                canvasHandle.selectedPointPos = new Point(e.Location.X, e.Location.Y); // viewing rectangle coordinates
                                //canvasHandle.numClicks = 1;
                            }

                            //
                            // GUI Redraw
                            //
                            updateLineTreeView();
                            linesDraw();
                            //

                            //resetSelectedLine(); //Maybe add a if statement to a multi after eachother selecting option public varible
                            btnConfirmCanvasLineAction.Enabled = false;
                            btnCancelCanvasLineAction.Enabled = false;
                        }
                        else { exception.generateException(CustomExceptions.no_existing_lines); } // no existing lines
                    }
                    else if (canvasHandle.canvasLineMode == CanvasModes.createLine) //Line create mode ------------------------------------------------------------------------------------
                    {
                        int key = canvasHandle.lineHandle.getLowestAvailableKey();
                        List<int[]> lines = canvasHandle.getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, canvasHandle.minSelectDistance);
                        if (lines.Count > 0 && canvasHandle.lockInToLineEnds == true) //Checks if lines count is over zero and if lockInLineEnds is true
                        {
                            int lineEnd = 0;
                            int[] endPoint = canvasHandle.getClosestLineEndCoordinate(e.Location.X, e.Location.Y, canvasHandle.minSelectDistance, out lineEnd);

                            canvasHandle.tempMouseEndPos = new Point(endPoint[0], endPoint[1]);
                            canvasHandle.createLine(key,
                                    canvasHandle.tempMouseStartPos.X,
                                    canvasHandle.tempMouseStartPos.Y,
                                    canvasHandle.tempMouseEndPos.X,
                                    canvasHandle.tempMouseEndPos.Y);

                            canvasHandle.selectedPointPos = new Point(endPoint[0], endPoint[1]); // viewing rectangle coordinates
                            canvasHandle.numClicks = 0;
                        }
                        else //Executes if no nearby line is found, if lines <= 0
                        {
                            canvasHandle.tempMouseEndPos = new Point(e.Location.X, e.Location.Y);

                            canvasHandle.createLine(key,
                                canvasHandle.tempMouseStartPos.X,
                                canvasHandle.tempMouseStartPos.Y,
                                canvasHandle.tempMouseEndPos.X,
                                canvasHandle.tempMouseEndPos.Y
                                );
                            canvasHandle.numClicks = 0;
                            canvasHandle.selectedPointPos = new Point(e.Location.X, e.Location.Y); // viewing rectangle coordinates
                        }
                        //
                        // GUI Redraw
                        //
                        updateLineTreeView();
                        linesDraw();
                        resetSelectedLine();
                        //

                        btnConfirmCanvasLineAction.Enabled = false; //Disables the buttons
                        btnCancelCanvasLineAction.Enabled = false;
                    }
                    else if (canvasHandle.canvasLineMode == CanvasModes.removeLine) //Line remove mode ------------------------------------------------------------------------------------
                    {
                        
                    }
                    else if (canvasHandle.canvasLineMode == CanvasModes.createPolygon) //Polygon create mode ------------------------------------------------------------------------------------
                    {
                            canvasHandle.tempMouseEndPos = new Point(e.Location.X, e.Location.Y);
                            canvasHandle.radiusPolygon = (int)Math.Sqrt(
                                Math.Pow((canvasHandle.tempMouseEndPos.X - canvasHandle.tempMouseStartPos.X), 2) + 
                                Math.Pow((canvasHandle.tempMouseEndPos.Y - canvasHandle.tempMouseStartPos.Y), 2));
                            //
                            //Implement way to get what angle the mouse is to make the polygon rotated appropriate
                            //
                            double angleExtension = ((180 * Math.Atan2(canvasHandle.tempMouseEndPos.Y - canvasHandle.tempMouseStartPos.Y, 
                                                                       canvasHandle.tempMouseEndPos.X - canvasHandle.tempMouseStartPos.X)
                            )/ Math.PI); 
                            

                            Dictionary<int, (Point, Point)> polygonLines = canvasHandle.createPolygonLines(canvasHandle.tempMouseStartPos.X, canvasHandle.tempMouseStartPos.Y, canvasHandle.numPolygonCorners, canvasHandle.radiusPolygon, angleExtension);
                        canvasHandle.InitialisePolygon(polygonLines);

                        canvasHandle.selectedPointPos = new Point(e.Location.X, e.Location.Y); // viewing rectangle coordinates
                        canvasHandle.numClicks = 0;

                        //
                        // GUI Redraw
                        //
                        updateLineTreeView();
                        linesDraw();
                        //
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

            canvasHandle.createLine(tempLineKeyBox, tempX1, tempY1, tempX2, tempY2);

            //
            // GUI Redraw
            //
            updateLineTreeView();
            linesDraw();
            resetSelectedLine();
            setSelectedNode(tempLineKeyBox);
            //
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            canvasHandle.removeLine(canvasHandle.selectedNode);

            //
            // GUI Redraw
            //
            updateLineTreeView();
            linesDraw();
            resetSelectedLine();
            //
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            int tempX1; int tempY1; int tempX2; int tempY2; int tempLineKeyBox; //Temporary varibles

            if (int.TryParse(x1Box.Text, out tempX1)) { } else { tempX1 = 0; }
            if (int.TryParse(y1Box.Text, out tempY1)) { } else { tempY1 = 0; }
            if (int.TryParse(x2Box.Text, out tempX2)) { } else { tempX2 = 1; }
            if (int.TryParse(y2Box.Text, out tempY2)) { } else { tempY2 = 1; }

            tempLineKeyBox = ((int)lineKeyBox.Value);

            canvasHandle.saveLine(tempLineKeyBox, tempX1, tempY1, tempX2, tempY2);

            //
            // GUI Redraw
            //
            updateLineTreeView();
            setSelectedNode(tempLineKeyBox);
            linesDraw();
            resetSelectedLine();
            //

        }
        private void btnDraw_Click(object sender, EventArgs e)
        {
            //BeginGraphics();
            linesDraw();
        }
        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            canvasHandle.fileHandle.viewSaveFileDialog();
            fileLocationBox.Text = canvasHandle.fileHandle.fileLocation;
        }
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            canvasHandle.fileHandle.viewOpenFileDialog();
            fileLocationBox.Text = canvasHandle.fileHandle.fileLocation;

            //
            // GUI Redraw
            //
            updateLineTreeView();
            linesDraw();
            resetSelectedLine();
            //
        }
        private void btnReloadFile_Click(object sender, EventArgs e)
        {
            canvasHandle.fileHandle.readLineFile(canvasHandle.fileHandle.fileLocation);

            //
            // GUI Redraw
            //
            updateLineTreeView();
            linesDraw();
            resetSelectedLine();
            //
        }
        private void checkBoxLabelLines_CheckedChanged(object sender, EventArgs e)
        {
            canvasHandle.DebugMode = checkBoxDebugMode.Checked;
            linesDraw();
        }
        private void checkBoxLockInToLineEnds_CheckedChanged(object sender, EventArgs e)
        {
            //lockInToLineEnds = checkBoxDebugMode.Checked;
            if (checkBoxDebugMode.CheckState == CheckState.Checked) { canvasHandle.lockInToLineEnds = true; }
            else if (checkBoxDebugMode.CheckState == CheckState.Unchecked) { canvasHandle.lockInToLineEnds = false; }
            
        }
        private void radioBtnEditLineMode_CheckedChanged(object sender, EventArgs e)
        {
            setCanvasMode(CanvasModes.editLine);
        }
        private void radioBtnCreateLineMode_CheckedChanged(object sender, EventArgs e)
        {
            setCanvasMode(CanvasModes.createLine);
        }
        private void radioBtnRemoveLineMode_CheckedChanged(object sender, EventArgs e)
        {
            setCanvasMode(CanvasModes.removeLine);
        }
        private void minSelectDistanceBox_ValueChanged(object sender, EventArgs e)
        {
            canvasHandle.minSelectDistance = (int)minSelectDistanceBox.Value;
            linesDraw();
        }
        private void btnCancelCanvasLineAction_Click(object sender, EventArgs e)
        {
            resetSelectedLine();
            
            linesDraw();
        }
        private void btnConfirmCanvasLineAction_Click(object sender, EventArgs e)
        {
            if (canvasHandle.canvasLineMode == CanvasModes.editLine)
            {
                canvasHandle.numClicks = 1;

                btnConfirmCanvasLineAction.Enabled = false;
                selectedLinesTreeView.Nodes[0].Nodes.Clear();
            }
            else if (canvasHandle.canvasLineMode == CanvasModes.removeLine)
            {
                bool tempSuccess = false;
                canvasHandle.removeLineByDialog(canvasHandle.selectedNode, out tempSuccess);
                if (tempSuccess) { btnCancelCanvasLineAction.Enabled = false; btnConfirmCanvasLineAction.Enabled = false; }

                //
                // GUI Redraw
                //
                updateLineTreeView();
                linesDraw();
                resetSelectedLine();
                //
            }
        }
        private void lineMultiLockingBox_CheckedChanged(object sender, EventArgs e)
        {
            canvasHandle.lineMultiLocking = lineMultiLockingBox.Checked;
        }

        private void toolStripMenuItemForceRedraw_Click(object sender, EventArgs e)
        {
            linesDraw();
        }

        private void createLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setCanvasMode(CanvasModes.createLine);
        }

        private void editLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setCanvasMode(CanvasModes.createLine);
        }

        private void removeLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setCanvasMode(CanvasModes.removeLine);
        }

        private void createPolygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setCanvasMode(CanvasModes.createPolygon);
        }

        private void enableToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            canvasHandle.DebugMode = true;
            linesDraw();
        }

        private void disableToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            canvasHandle.DebugMode = false;
            linesDraw();
        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasHandle.lockInToLineEnds = true;
        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasHandle.lockInToLineEnds = false;
        }

        private void enableToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            canvasHandle.lineMultiLocking = true;
        }

        private void disableToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            canvasHandle.lineMultiLocking = false;
        }

        private void radioBtnCreatePolygonMode_CheckedChanged(object sender, EventArgs e)
        {
            setCanvasMode(CanvasModes.createPolygon);
        }

        private void numPolygonCornersBox_ValueChanged(object sender, EventArgs e)
        {
            canvasHandle.numPolygonCorners = (int)numPolygonCornersBox.Value;
        }

        private void resetCanvasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<int> lines = new List<int>();
            foreach (var line in canvasHandle.lineHandle.LineObjects.Keys)
            {
                lines.Add(line);
            }

            bool tempSuccess = false;
            canvasHandle.removeLinesByDialog(lines, out tempSuccess);

            //
            // GUI Redraw
            //
            updateLineTreeView();
            linesDraw();
            resetSelectedLine();
            //
        }

        private void showExceptionLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> log = new List<string>()
            { " " };
            log = exception.RetrieveLog();

            if (log != null)
            {
                StringBuilder logString = new StringBuilder();

                foreach (var line in log)
                {
                    logString.AppendLine(line);
                }

                MessageBox.Show(logString.ToString());
            }
            else { exception.generateException(CustomExceptions.null_list_contents); }
        }

        private void Canvas_SizeChanged(object sender, EventArgs e)
        {
            linesDraw();
        }
    }
}
