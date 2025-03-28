﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineDrawerDemo
{
    public class CanvasDrawingHandling
    {
        private CanvasHandling canvasHandle;

        private PictureBox CanvasPictureBox;
        private CanvasObject canvasObject;

        private GUIExternalEvents externalEvents;

        public CanvasDrawingHandling(CanvasHandling canvasHandleObject) 
        {
            canvasHandle = canvasHandleObject;

            canvasObject = new CanvasObject();
            CanvasPictureBox = new PictureBox();

            externalEvents = GUIExternalEvents.GetInstance();

            createCanvas(canvasObject.position, canvasObject.size);
        }

        /// <summary>
        /// Creates the canvas object on the gui which is represented by a picture box
        /// </summary>
        public void createCanvas(Point location, Size size)
        {
            // 
            // Canvas Object Paramaters //Add parameters that make some of these depend on the imported CanvasObject class data
            // 
            this.CanvasPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CanvasPictureBox.BackColor = System.Drawing.Color.White;
            this.CanvasPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CanvasPictureBox.Name = "Canvas";

            this.CanvasPictureBox.Location = new System.Drawing.Point(location.X, location.Y);
            this.CanvasPictureBox.Size = new System.Drawing.Size(size.Width, size.Height);

            this.CanvasPictureBox.TabIndex = 0;
            this.CanvasPictureBox.TabStop = false;
            //this.CanvasPictureBox.SizeChanged += new System.EventHandler(this.Canvas_SizeChanged);
            this.CanvasPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.lineCanvas_Paint);
            this.CanvasPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseClick); 
        }

        public void removeCanvas()
        {
            CanvasPictureBox = null;
        }

        public Control getCanvas //Retrieve canvas represented as picture box to canvasHandle
        { 
            get
            {
                return CanvasPictureBox;
            }
        }
        /// <summary>
        /// Force canvas to redraw
        /// </summary>
        public void Redraw() //Forces canvas to redraw
        {
            CanvasPictureBox.Invalidate();
            CanvasPictureBox.Update();
        }

        //
        // Canvas mouse click handling ------------------------------------------
        //
        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            //onMouseClickGetCoordinates(e.Location.X, e.Location.Y);
            //int temp1 = getClosestLineAsKey(e.Location.X, e.Location.Y, out int LineEnd);
            //List<int[]> temp2 = getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, 10);
            //int[] temp3 = getClosestLinesAsArray(e.Location.X, e.Location.Y, 10);
            if (canvasHandle.publicVaribles.DebugMode == true)
            {
                if (canvasHandle.publicVaribles.currentNumClick == numClick.First) //First mouse click
                {
                    if (canvasHandle.publicVaribles.canvasLineMode == CanvasModes.editLine) //Line edit mode ------------------------------------------------------------------------------------
                    {
                        List<int[]> lines = canvasHandle.getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, canvasHandle.publicVaribles.minSelectDistance);
                        if (canvasHandle.lineHandle.LineObjects.Count > 0)
                        {
                            if (lines.Count == 1) //Skips selected window and goes to numClick 2
                            {
                                if (true)
                                {
                                    int[] line = lines[0];
                                    //setSelectedNode(line[0]);
                                    //setSelectedNodeAndLineEnd(line[0], line[1]);

                                    canvasHandle.publicVaribles.selectedCanvasLineEnd = line[1]; //Set to selected line end
                                    canvasHandle.publicVaribles.currentNumClick = numClick.Second;

                                    Point endPoint = canvasHandle.getLineEndCoordinates(canvasHandle.publicVaribles.selectedNode, canvasHandle.publicVaribles.selectedCanvasLineEnd);
                                    canvasHandle.publicVaribles.selectedPointPos = new Point(endPoint.X, endPoint.Y);

                                    externalEvents.UpdateFormObjects(); // Trigger Event

                                }
                            }
                            else if (lines.Count > 0)
                            {
                                int[] line = lines[0];
                                Point endPoint = canvasHandle.getLineEndCoordinates(line[0], line[1]);
                                canvasHandle.publicVaribles.selectedPointPos = new Point(endPoint.X, endPoint.Y);

                                canvasHandle.InitializeTempSelectableLines(lines);

                                if (canvasHandle.publicVaribles.lineMultiLocking == true)
                                {
                                    canvasHandle.publicVaribles.currentNumClick = numClick.Second;
                                }

                                //
                                // GUI Redraw
                                //
                                externalEvents.UpdateFormTreeViews(); // Trigger Event
                                Redraw();
                                //selectedLinesTreeView.Focus();
                                //
                            }
                            else { canvasHandle.exception.generateException(CustomExceptions.no_line_end_nearby); } // no close line end

                            canvasHandle.publicVaribles.CancelAction = true;

                            //
                            // GUI Redraw
                            //
                            externalEvents.UpdateFormObjects(); // Trigger Event
                            Redraw();
                            //
                        }
                        else { canvasHandle.exception.generateException(CustomExceptions.no_existing_lines); } // no existing lines
                    }
                    else if (canvasHandle.publicVaribles.canvasLineMode == CanvasModes.createLine) //Line create mode ------------------------------------------------------------------------------------
                    {
                        List<int[]> lines = canvasHandle.getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, canvasHandle.publicVaribles.minSelectDistance);
                        if (lines.Count > 0 && canvasHandle.publicVaribles.lockInToLineEnds == true)
                        {
                            Point endPoint = canvasHandle.getClosestLineEndCoordinate(e.Location.X, e.Location.Y, canvasHandle.publicVaribles.minSelectDistance, out int lineEnd);
                            canvasHandle.publicVaribles.tempMouseStartPos = new Point(endPoint.X, endPoint.Y);
                            canvasHandle.publicVaribles.currentNumClick = numClick.Second;
                            canvasHandle.publicVaribles.selectedPointPos = new Point(endPoint.X, endPoint.Y);
                        }
                        else //Executes if no nearby line is found, if lines <= 0
                        {
                            //generateException("");
                            canvasHandle.publicVaribles.tempMouseStartPos = new Point(e.Location.X, e.Location.Y);
                            canvasHandle.publicVaribles.selectedPointPos = new Point(e.Location.X, e.Location.Y);
                            canvasHandle.publicVaribles.currentNumClick = numClick.Second;
                        }

                        canvasHandle.publicVaribles.CancelAction = true;

                        //
                        // GUI Redraw
                        //
                        //setSelectedNodeText("...");
                        externalEvents.UpdateFormObjects(); // Trigger Event
                        Redraw();
                        //
                    }
                    else if (canvasHandle.publicVaribles.canvasLineMode == CanvasModes.removeLine) //Line remove mode ------------------------------------------------------------------------------------
                    {
                        List<int[]> lines = canvasHandle.getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, canvasHandle.publicVaribles.minSelectDistance);
                        if (canvasHandle.lineHandle.LineObjects.Count > 0)
                        {   
                            if (lines.Count == 1) //Skips selected window and goes to attempt to remove selected node
                            {
                                int[] line = lines[0];
                                canvasHandle.publicVaribles.selectedNode = line[0];

                                bool tempSuccess;
                                canvasHandle.removeLineByDialog(canvasHandle.publicVaribles.selectedNode, out tempSuccess);
                                if (tempSuccess) { canvasHandle.publicVaribles.CancelAction = false; canvasHandle.publicVaribles.ConfirmAction = false; }
                                externalEvents.ResetFormParameters();
                            }
                            else if (lines.Count > 0)
                            {
                                canvasHandle.InitializeTempSelectableLines(lines);
                            }
                            else { canvasHandle.exception.generateException(CustomExceptions.no_line_end_nearby); } // no close line end
                            //btnCancelCanvasLineAction.Enabled = true;

                            //
                            // GUI Redraw
                            //
                            externalEvents.UpdateFormObjects(); // Trigger Event
                            externalEvents.UpdateFormTreeViews(); // Trigger Event
                            Redraw();
                            //

                        }
                        else { canvasHandle.exception.generateException("no_existing_lines"); } // no existing lines
                    }
                    else if (canvasHandle.publicVaribles.canvasLineMode == CanvasModes.createPolygon) //Polygon create mode ------------------------------------------------------------------------------------
                    {
                        canvasHandle.publicVaribles.tempMouseStartPos = new Point(e.Location.X, e.Location.Y);
                        canvasHandle.publicVaribles.selectedPointPos = new Point(e.Location.X, e.Location.Y);

                        canvasHandle.publicVaribles.CancelAction = true;

                        canvasHandle.publicVaribles.currentNumClick = numClick.Second;

                        //
                        // GUI Redraw
                        //
                        externalEvents.UpdateFormObjects(); // Trigger Event
                        Redraw();
                        //
                    }
                    //btnCancelCanvasLineAction.Enabled = true;

                }
                else if (canvasHandle.publicVaribles.currentNumClick == numClick.Second) //Second mouse click
                {
                    if (canvasHandle.publicVaribles.canvasLineMode == CanvasModes.editLine) //Line edit mode ------------------------------------------------------------------------------------
                    {
                        if (canvasHandle.lineHandle.LineObjects.Count > 0)
                        {
                            List<int[]> lines = canvasHandle.getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, canvasHandle.publicVaribles.minSelectDistance);

                            if (lines.Count > 0 && canvasHandle.publicVaribles.lockInToLineEnds == true) //Checks if lines count is over zero and if lockInLineEnds is true
                            {
                                Point endPoint = canvasHandle.getClosestLineEndCoordinate(e.Location.X, e.Location.Y, canvasHandle.publicVaribles.minSelectDistance, out int lineEnd);
                                if (canvasHandle.publicVaribles.lineMultiLocking == false) //No MultiLineLocking 
                                {
                                    if (canvasHandle.publicVaribles.selectedCanvasLineEnd == 1)
                                    {
                                        canvasHandle.lineHandle.editLineProperties(canvasHandle.publicVaribles.selectedNode, //key
                                            endPoint.X, //x1
                                            endPoint.Y, //y1
                                            canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realx2, //x2
                                            canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realy2 //y2
                                            );
                                    }
                                    else if (canvasHandle.publicVaribles.selectedCanvasLineEnd == 2)
                                    {
                                        canvasHandle.lineHandle.editLineProperties(canvasHandle.publicVaribles.selectedNode, //key
                                            canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realx1, //x1
                                            canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realy1, //y1
                                            endPoint.X, //x2
                                            endPoint.Y //y2
                                            );
                                    }
                                }
                                else if (canvasHandle.publicVaribles.lineMultiLocking == true) //With MultiLineLocking
                                {
                                    Point lineEndPoint;
                                    foreach (var line in canvasHandle.publicVaribles.selectedLineObjects) //Go through the dictionary and edit each line's coordinates to LockedIntoLineEnd's coordinate
                                    {
                                        lineEndPoint = canvasHandle.getLineEndCoordinates(line.Key, line.Value);

                                        if (canvasHandle.publicVaribles.selectedCanvasLineEnd == 1)
                                        {
                                            canvasHandle.lineHandle.editLineProperties(canvasHandle.publicVaribles.selectedNode, //key
                                                lineEndPoint.X, //x1
                                                lineEndPoint.Y, //y1
                                                canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realx2, //x2
                                                canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realy2 //y2
                                                );
                                        }
                                        else if (canvasHandle.publicVaribles.selectedCanvasLineEnd == 2)
                                        {
                                            canvasHandle.lineHandle.editLineProperties(canvasHandle.publicVaribles.selectedNode, //key
                                                canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realx1, //x1
                                                canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realy1, //y1
                                                lineEndPoint.X, //x2
                                                lineEndPoint.Y //y2
                                                );
                                        }
                                    }
                                }

                                externalEvents.ResetFormParameters(); // Trigger Event, executes before "overriding" code

                                canvasHandle.publicVaribles.selectedPointPos = new Point(endPoint.X, endPoint.Y); // viewing rectangle coordinates
                                //canvasHandle.numClicks = 0;

                            }
                            else //Executes if no nearby line is found, if lines <= 0 or if LockIntoLineEnds == false
                            {
                                if (canvasHandle.publicVaribles.lineMultiLocking == false) //No MultiLineLocking
                                {
                                    if (canvasHandle.publicVaribles.selectedCanvasLineEnd == 1) //checks if selected line end is first end
                                    {
                                        canvasHandle.lineHandle.editLineProperties(
                                            canvasHandle.publicVaribles.selectedNode, //key
                                            e.Location.X, //x1
                                            e.Location.Y, //y1
                                            canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realx2, //x2
                                            canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realy2 //y2
                                            );
                                    }
                                    else if (canvasHandle.publicVaribles.selectedCanvasLineEnd == 2) //checks if selected line end is second end
                                    {
                                        canvasHandle.lineHandle.editLineProperties(canvasHandle.publicVaribles.selectedNode, //key
                                            canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realx1, //x1
                                            canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realy1, //y1
                                            e.Location.X, //x2
                                            e.Location.Y //y2
                                            );
                                    }
                                }
                                else if (canvasHandle.publicVaribles.lineMultiLocking == true) //With MultiLineLocking
                                {
                                    foreach (var line in canvasHandle.publicVaribles.selectedLineObjects) //Go through the dictionary and edit each line's coordinates to LockedIntoLineEnd's coordinate
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

                                externalEvents.ResetFormParameters(); // Trigger Event, executes before "overriding" code

                                canvasHandle.publicVaribles.selectedPointPos = new Point(e.Location.X, e.Location.Y); // viewing rectangle coordinates
                                //canvasHandle.numClicks = 1;
                            }

                            //resetSelectedLine(); //Maybe add a if statement to a multi after eachother selecting option public varible
                            canvasHandle.publicVaribles.ConfirmAction = false;
                            canvasHandle.publicVaribles.CancelAction = false;

                            //
                            // GUI Redraw
                            //
                            externalEvents.UpdateFormTreeViews();
                            Redraw();
                            //
                        }
                        else { canvasHandle.exception.generateException(CustomExceptions.no_existing_lines); } // no existing lines
                    }
                    else if (canvasHandle.publicVaribles.canvasLineMode == CanvasModes.createLine) //Line create mode ------------------------------------------------------------------------------------
                    {
                        int key = canvasHandle.lineHandle.getLowestAvailableKey();
                        List<int[]> lines = canvasHandle.getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, canvasHandle.publicVaribles.minSelectDistance);
                        if (lines.Count > 0 && canvasHandle.publicVaribles.lockInToLineEnds == true) //Checks if lines count is over zero and if lockInLineEnds is true
                        {
                            int lineEnd = 0;
                            Point endPoint = canvasHandle.getClosestLineEndCoordinate(e.Location.X, e.Location.Y, canvasHandle.publicVaribles.minSelectDistance, out lineEnd);

                            canvasHandle.publicVaribles.tempMouseEndPos = new Point(endPoint.X, endPoint.Y);
                            canvasHandle.createLine(key,
                                    canvasHandle.publicVaribles.tempMouseStartPos.X,
                                    canvasHandle.publicVaribles.tempMouseStartPos.Y,
                                    canvasHandle.publicVaribles.tempMouseEndPos.X,
                                    canvasHandle.publicVaribles.tempMouseEndPos.Y);

                            canvasHandle.publicVaribles.selectedPointPos = new Point(endPoint.X, endPoint.Y); // viewing rectangle coordinates
                            canvasHandle.publicVaribles.currentNumClick = numClick.First;
                        }
                        else //Executes if no nearby line is found, if lines <= 0
                        {
                            canvasHandle.publicVaribles.tempMouseEndPos = new Point(e.Location.X, e.Location.Y);

                            canvasHandle.createLine(key,
                                canvasHandle.publicVaribles.tempMouseStartPos.X,
                                canvasHandle.publicVaribles.tempMouseStartPos.Y,
                                canvasHandle.publicVaribles.tempMouseEndPos.X,
                                canvasHandle.publicVaribles.tempMouseEndPos.Y
                                );
                            canvasHandle.publicVaribles.currentNumClick = numClick.First;
                            canvasHandle.publicVaribles.selectedPointPos = new Point(e.Location.X, e.Location.Y); // viewing rectangle coordinates
                        }
                        //
                        // GUI Redraw
                        //
                        externalEvents.UpdateFormTreeViews();
                        externalEvents.ResetFormParameters();
                        Redraw();
                        //

                        canvasHandle.publicVaribles.ConfirmAction = false; //Disables the buttons
                        canvasHandle.publicVaribles.CancelAction = false;
                    }
                    else if (canvasHandle.publicVaribles.canvasLineMode == CanvasModes.removeLine) //Line remove mode ------------------------------------------------------------------------------------
                    {

                    }
                    else if (canvasHandle.publicVaribles.canvasLineMode == CanvasModes.createPolygon) //Polygon create mode ------------------------------------------------------------------------------------
                    {
                        {
                            canvasHandle.publicVaribles.tempMouseEndPos = new Point(e.Location.X, e.Location.Y);
                            canvasHandle.publicVaribles.radiusPolygon = (int)Math.Sqrt(
                                Math.Pow((canvasHandle.publicVaribles.tempMouseEndPos.X - canvasHandle.publicVaribles.tempMouseStartPos.X), 2) +
                                Math.Pow((canvasHandle.publicVaribles.tempMouseEndPos.Y - canvasHandle.publicVaribles.tempMouseStartPos.Y), 2));

                            double angleExtension = ((180 * Math.Atan2(canvasHandle.publicVaribles.tempMouseEndPos.Y - canvasHandle.publicVaribles.tempMouseStartPos.Y,
                                                                        canvasHandle.publicVaribles.tempMouseEndPos.X - canvasHandle.publicVaribles.tempMouseStartPos.X)
                            ) / Math.PI);


                            Dictionary<int, (Point, Point)> polygonLines = canvasHandle.createPolygonLines(canvasHandle.publicVaribles.tempMouseStartPos.X, canvasHandle.publicVaribles.tempMouseStartPos.Y, canvasHandle.publicVaribles.numPolygonCorners, canvasHandle.publicVaribles.radiusPolygon, angleExtension);
                            canvasHandle.InitialisePolygon(polygonLines);

                            canvasHandle.publicVaribles.selectedPointPos = new Point(e.Location.X, e.Location.Y); // viewing rectangle coordinates
                            canvasHandle.publicVaribles.currentNumClick = numClick.First;
                        }

                        List<int[]> lines = canvasHandle.getClosestLinesAsArrayWithLineEnds(e.Location.X, e.Location.Y, canvasHandle.publicVaribles.minSelectDistance);
                        if (lines.Count > 0 && canvasHandle.publicVaribles.lockInToLineEnds == true)
                        {
                            Point endPoint = canvasHandle.getClosestLineEndCoordinate(e.Location.X, e.Location.Y, canvasHandle.publicVaribles.minSelectDistance, out int lineEnd);
                            
                            canvasHandle.publicVaribles.tempMouseStartPos = new Point(endPoint.X, endPoint.Y);
                            canvasHandle.publicVaribles.currentNumClick = numClick.Second;  
                            canvasHandle.publicVaribles.selectedPointPos = new Point(endPoint.X, endPoint.Y);
                        }
                        else //Executes if no nearby line is found, if lines <= 0
                        {
                            //generateException("");
                            canvasHandle.publicVaribles.tempMouseStartPos = new Point(e.Location.X, e.Location.Y);
                            canvasHandle.publicVaribles.selectedPointPos = new Point(e.Location.X, e.Location.Y);
                            canvasHandle.publicVaribles.currentNumClick = numClick.Second;
                        }

                        //
                        // GUI Redraw
                        //
                        externalEvents.UpdateFormTreeViews();
                        Redraw();
                        //
                    }
                }
                //resetSelectedLine();
            }
        }






        //
        // Canvas drawing handling ------------------------------------------
        //
        private void lineCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (canvasHandle.lineHandle.LineObjects.Keys.Count > 0)
            {
                //this.SuspendLayout();
                Graphics g = e.Graphics; //Create drawing canvas
                Dictionary<int, LineObject> lineObjects = canvasHandle.lineHandle.GetLineObjects();

                foreach (var lineObject in lineObjects) //Extract every line from list and draw them
                {
                    Point startPoint = new Point(lineObject.Value.Realx1, lineObject.Value.Realy1);
                    Point endPoint = new Point(lineObject.Value.Realx2, lineObject.Value.Realy2);

                    Pen pen = new Pen(Brushes.Black, canvasHandle.publicVaribles.lineWidth);

                    g.DrawLine(pen, startPoint, endPoint); //Draw Line from start to end points

                    if (canvasHandle.publicVaribles.DebugMode == true) //Draw line key on canvas
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

                        Font font = new Font("StandardFont", 12, FontStyle.Regular);

                        g.DrawString("line" + lineObject.Key, font, Brushes.Black, new Point(posX, posY));

                        //Label label = new Label();
                        //label.Name = "line" + lineObject.Key;
                        //label.Text = "line" + lineObject.Key;
                        //label.Location = new Point(tempX, tempY);
                        //label.Tag = "tempLabel";
                        //Canvas.Controls.Add(label);
                    }
                }
                if (canvasHandle.publicVaribles.DebugMode == true) //Selection Point Handling
                {
                    Rectangle rec = new Rectangle(); // Draws out a rectangle on top of selected line's line end to visualy show selected line end
                    Pen pen = new Pen(Brushes.Gray, 1);
                    Pen pen2 = new Pen(Brushes.Gray, 1);

                    int xPos = canvasHandle.publicVaribles.selectedPointPos.X - canvasHandle.publicVaribles.minSelectDistance / 2;
                    int yPos = canvasHandle.publicVaribles.selectedPointPos.Y - canvasHandle.publicVaribles.minSelectDistance / 2;
                    int xPos2 = canvasHandle.publicVaribles.selectedPointPos.X;
                    int yPos2 = canvasHandle.publicVaribles.selectedPointPos.Y;

                    rec.X = xPos;
                    rec.Y = yPos;
                    rec.Width = canvasHandle.publicVaribles.minSelectDistance;
                    rec.Height = canvasHandle.publicVaribles.minSelectDistance;

                    Point temp1StartPos = new Point(0, yPos2);
                    Point temp1EndPos = new Point(canvasObject.size.Width, yPos2);
                    Point temp2StartPos = new Point(xPos2, 0);
                    Point temp2EndPos = new Point(xPos2, canvasObject.size.Height);

                    g.DrawLine(pen2, temp1StartPos, temp1EndPos);
                    g.DrawLine(pen2, temp2StartPos, temp2EndPos);
                    g.DrawRectangle(pen, rec);
                }
            }
        }
    }
}
