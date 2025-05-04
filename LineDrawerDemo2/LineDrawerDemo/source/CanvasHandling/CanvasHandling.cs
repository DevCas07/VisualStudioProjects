using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LineDrawerDemo.MainWindow;
using System.Windows.Forms;

namespace LineDrawerDemo
{
    /// <summary>
    /// Handles overall available functions to user and acts as a data intermediary between classes
    /// </summary>
    public class CanvasHandling
    {
        internal LineHandling lineHandle;
        internal FileHandling fileHandle;
        internal ExceptionHandling exception;
        private CanvasDrawingHandling drawingHandle;
        
        internal CanvasPublicVaribles publicVaribles;

        public CanvasHandling()
        {
            publicVaribles = new CanvasPublicVaribles();

            lineHandle = new LineHandling();
            fileHandle = new FileHandling(lineHandle);
            drawingHandle = new CanvasDrawingHandling(this);

            exception = LineDrawerDemo.ExceptionHandling.GetInstance();
        }
        public void resetFormParameters()
        {
            publicVaribles.currentNumClick = numClick.First;

            publicVaribles.ConfirmAction = false;
            publicVaribles.CancelAction = false;

            publicVaribles.selectedPointPos = new Point(-20, -20);

            publicVaribles.selectedLineObjects.Clear();
        }
        /// <summary>
        /// Creates new canvas control based on CanvasObject
        /// </summary>
        /// <param name="canvasObject"></param>
        public void CreateCanvas(CanvasObject canvasObject)
        {
            drawingHandle.createCanvas(canvasObject.position, canvasObject.size);
        }
        /// <summary>
        /// Removes canvas control, use with caution
        /// </summary>
        public void RemoveCanvas()
        {
            drawingHandle.removeCanvas();
        }
        /// <summary>
        /// Retrieves canvas control represented as PictureBox
        /// </summary>
        public Control getCanvas()
        {
            return drawingHandle.getCanvas();
        }
        /// <summary>
        /// Force canvas to redraw
        /// </summary>
        public void Redraw()
        {
            drawingHandle.Redraw();
        }

        //
        // Basic line functions handling ------------------------------------------
        //
        /// <summary>
        /// Creates new line if selected key is available
        /// </summary>
        /// <param name="key">line id</param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public void createLine(int key, int x1, int y1, int x2, int y2) //Creates new line if key is available
        {
            if (lineHandle.checkKeyAvailability(key))
            {
                lineHandle.InitializeLineObject(key, x1, y1, x2, y2); //Adds new line to list
            }
            else { exception.generateException(CustomExceptions.already_occupied_key); } // already occupied key

        }
        /// <summary>
        /// Removes line from dictionary
        /// </summary>
        /// <param name="key">line id</param>
        public void removeLine(int key) //Removes line from dictionary
        {
            if (!lineHandle.checkKeyAvailability(key))
            {
                lineHandle.RemoveLineObject(key);
            }
            else { exception.generateException(CustomExceptions.not_existing_key); } // not existing key
        }
        /// <summary>
        /// Removes specified line from dictionary only after user confirmation
        /// </summary>
        /// <param name="key">line id</param>
        /// <param name="success"></param>
        public void removeLineByDialog(int key, out bool success)
        {
            success = false;
            var dialog = MessageBox.Show("Remove line:" + publicVaribles.selectedNode, "Confirm removal", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (dialog == DialogResult.OK)
            {
                removeLine(key);
                success = true;
            }
        }
        /// <summary>
        /// Removes specified lines from dictionary only after user confirmation
        /// </summary>
        /// <param name="lines">Each item value represents a line key/id</param>
        /// <param name="success"></param>
        public void removeLinesByDialog(List<int> lines, out bool success)
        {
            success = false;

            StringBuilder stringIds = new StringBuilder(); //Add ids of all selected lines marked for removal
            foreach (var line in lines)
            {
                stringIds.AppendLine($"line:{line}");
            }

            var dialog = MessageBox.Show("Remove lines\n" + stringIds, "Confirm removal", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (dialog == DialogResult.OK)
            {
                foreach (var line in lines)
                {
                    removeLine(line); //line = key
                }
                success = true;
            }
        }
        /// <summary>
        /// Edits a specified line's properties, if specified key is diffrent than selected node the selected node line will be cloned anew over to specified key
        /// </summary>
        /// <param name="key">line id</param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public void saveLine(int key, int x1, int y1, int x2, int y2) //Edits line properties, if diffrent key it then copies line to new key
        {
            Dictionary<int, LineObject> lineObjects = lineHandle.GetLineObjects();
            if (lineObjects.Count > 0)
            {
                if (publicVaribles.selectedNode == key)
                {
                    lineHandle.editLineProperties(publicVaribles.selectedNode, x1, y1, x2, y2);
                }
                else if (lineHandle.checkKeyAvailability(key))
                {
                    lineHandle.copyLineObjectWithNewKey(publicVaribles.selectedNode, key);
                    lineHandle.editLineProperties(key, x1, y1, x2, y2);
                }
            }
            else { exception.generateException(CustomExceptions.not_existing_key); } // not existing key
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">line id</param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public void editLine(int key, int x1, int y1, int x2, int y2)
        {
            Dictionary<int, LineObject> lineObjects = lineHandle.GetLineObjects();
            if (lineObjects.Count > 0)
            {
                lineHandle.editLineProperties(publicVaribles.selectedNode, x1, y1, x2, y2);
            }
            else { exception.generateException(CustomExceptions.not_existing_key); } // not existing key
        }
        /// <summary>
        /// Checks if inputted coordinates is within distance to one of specified line's ends
        /// </summary>
        /// <param name="key">line id</param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="minDistance">minimun distance from mouse pointer which can be detected</param>
        /// <param name="lineEnd">returns which lineEnd is the closest from mouse pointer</param>
        /// <returns></returns>
        public bool isWithinDistanceToLineEnd(int key, int posX, int posY, int minDistance, out int lineEnd) //Checks if inputted coordinates is within distance to one of specified line's ends
        {
            int diffX1 = Math.Abs(lineHandle.LineObjects[key].Realx1 - posX);
            int diffY1 = Math.Abs(lineHandle.LineObjects[key].Realy1 - posY);

            int diffX2 = Math.Abs(lineHandle.LineObjects[key].Realx2 - posX);
            int diffY2 = Math.Abs(lineHandle.LineObjects[key].Realy2 - posY);

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
        /// <summary>
        /// Deprecated method
        /// </summary>
        /// <param name="baseXPos"></param>
        /// <param name="baseYPos"></param>
        /// <param name="lineEnd">returns which lineEnd is the closest from mouse pointer</param>
        /// <returns></returns>
        public int getClosestLineAsKey(int baseXPos, int baseYPos, out int lineEnd) //Maybe implement this system in future, (not used), maybe test
        {
            int key = 0;
            int closestLineEnd = 0;
            int closestLineEndDistance = 10000000;
            int baseDistance = (int)(Math.Pow(baseXPos, 2) + Math.Pow(baseYPos, 2));

            Dictionary<int, LineObject> LineObjects = lineHandle.GetLineObjects();
            foreach (var line in LineObjects)
            {
                int lineDistanceLineEnd1 = (int)Math.Abs((Math.Pow(lineHandle.LineObjects[line.Key].Realx1, 2) + Math.Pow(lineHandle.LineObjects[line.Key].Realy1, 2)) - baseDistance);
                int lineDistanceLineEnd2 = (int)Math.Abs((Math.Pow(lineHandle.LineObjects[line.Key].Realx2, 2) + Math.Pow(lineHandle.LineObjects[line.Key].Realy2, 2)) - baseDistance);

                if (lineDistanceLineEnd1 < closestLineEndDistance)
                {
                    key = line.Key;
                    closestLineEnd = 1;
                }
                else if (lineDistanceLineEnd2 < closestLineEndDistance)
                {
                    key = line.Key;
                    closestLineEnd = 2;
                }
            }

            lineEnd = closestLineEnd;
            return key;
        }
        /// <summary>
        /// Get line keys and line ends that are within a certain area
        /// </summary>
        /// <param name="baseXPos"></param>
        /// <param name="baseYPos"></param>
        /// <param name="minDistance">minimun distance from mouse pointer which can be detected</param>

        /// 
        /// <returns></returns>
        public List<int[]> getClosestLinesAsArrayWithLineEnds(int baseXPos, int baseYPos, int minDistance) //Get line keys and line ends that are within a certain area
        {
            List<int[]> lines = new List<int[]>();
            int tempOutLineEnd;
            int indexCount = 0;
            Dictionary<int, LineObject> lineObjects = lineHandle.GetLineObjects();
            foreach (var line in lineObjects)
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
        /// <summary>
        /// Gets line key and line end that are within a certain area
        /// </summary>
        /// <param name="baseXPos"></param>
        /// <param name="baseYPos"></param>
        /// <param name="minDistance">minimun distance from mouse pointer which can be detected</param>
        /// <param name="lineEnd">returns which lineEnd is the closest from mouse pointer</param>
        /// <param name="allowSelf">allow selectedNode key to be detected and used</param>
        /// <returns></returns>
        public Point getClosestLineEndCoordinate(int baseXPos, int baseYPos, int minDistance, out int lineEnd, bool allowSelf) //Get line keys and line ends that are within a certain area
        {
            int tempXPos = 0;
            int tempYPos = 0;
            Point lineCoordinates;

            List<int[]> lines = getClosestLinesAsArrayWithLineEnds(baseXPos, baseYPos, minDistance);
            int[] line = lines[0]; // (line key, line end)
            if (allowSelf == false) 
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    line = lines[i]; // (line key, line end)

                    if (line[0] != publicVaribles.selectedNode)
                    {
                        LineObject lineObject = lineHandle.GetLine(line[0]);

                        if (line[1] == 1) //Line end 1
                        {
                            tempXPos = lineObject.Realx1;
                            tempYPos = lineObject.Realy1;
                        }
                        else if (line[1] == 2) //Line end 2
                        {
                            tempXPos = lineObject.Realx2;
                            tempYPos = lineObject.Realy2;
                        }
                        break;
                    }
                }
            }
            if (allowSelf == true)
            {
                LineObject lineObject = lineHandle.GetLine(line[0]);

                if (line[1] == 1) //Line end 1
                {
                    tempXPos = lineObject.Realx1;
                    tempYPos = lineObject.Realy1;
                }
                else if (line[1] == 2) //Line end 2
                {
                    tempXPos = lineObject.Realx2;
                    tempYPos = lineObject.Realy2;
                }
            }

            lineEnd = line[1];

            lineCoordinates = new Point(tempXPos, tempYPos);
            return lineCoordinates;
        }
        /// <summary>
        /// Gets line key that are within a certain area
        /// </summary>
        /// <param name="baseXPos"></param>
        /// <param name="baseYPos"></param>
        /// <param name="minDistance">minimun distance from mouse pointer which can be detected</param>
        /// <returns></returns>
        public int[] getClosestLinesAsArray(int baseXPos, int baseYPos, int minDistance) //Get line keys that are within a certain area
        {
            List<int> listLines = new List<int>();
            int tempOutLineEnd;
            int indexCount = 0;

            foreach (var line in lineHandle.LineObjects)
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
        /// <summary>
        /// Retrieves line end coordinate
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lineEnd"></param>
        /// <returns></returns>
        public Point getLineEndCoordinates(int key, int lineEnd) //Retrieves line end coordinates 
        {
            //int[] line = new int[2];
            Point point = new Point();
            LineObject line = lineHandle.GetLine(key);

            if (lineEnd == 1)
            {
                point.X = line.Realx1;
                point.Y = line.Realy1;
            }
            else if (lineEnd == 2)
            {
                point.X = line.Realx2;
                point.Y = line.Realy2;
            }

            return point;
        }
        /// <summary>
        /// Creates a polygon shape consisting of connected but individually seperated lines 
        /// </summary>
        /// <param name="xPos">Middle point x position</param>
        /// <param name="yPos">Middle point y position</param>
        /// <param name="numPolygonCorners"></param>
        /// <param name="radius"></param>
        /// <param name="angleExtension">The angle between the second selected position and the first selected postion based on the x-axis</param>
        /// <returns></returns>
        //
        // Advanced line functions handling ------------------------------------------
        //
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
        /// <summary>
        /// Adds polygon lines to Dictionary
        /// </summary>
        /// <param name="polygonLines"></param>
        public void InitialisePolygon(Dictionary<int, (Point, Point)> polygonLines)
        {
            foreach (var polygonLine in polygonLines)
            {
                int key = lineHandle.getLowestAvailableKey();
                int x1 = polygonLine.Value.Item1.X;
                int y1 = polygonLine.Value.Item1.Y;
                int x2 = polygonLine.Value.Item2.X;
                int y2 = polygonLine.Value.Item2.Y;

                lineHandle.InitializeLineObject(key, x1, y1, x2, y2);
            }
        }
        //
        // Necessary miscellaneous canvas handling ------------------------------------------
        //
        /// <summary>
        /// Adds found lines to temporary selectable list
        /// </summary>
        /// <param name="lines"></param>
        public void InitializeTempSelectableLines(List<int[]> lines) // Adds found lines to temporary selectable list
        {
            publicVaribles.selectedLineObjects.Clear();

            foreach (var line in lines)
            {
                publicVaribles.selectedLineObjects.Add(line[0], line[1]);
            }
        }
        public void InitialiseTempSelectedLine(int key, int lineEnd)
        {
            publicVaribles.selectedLineObjects.Add(key, lineEnd);
        }
        public void ClearTempSelectedLine()
        {
            publicVaribles.selectedLineObjects.Clear();
        }
        public void setCanvasMode(CanvasModes canvasMode) //string modeId)
        {

            //if (modeId == "editLine") { canvasLineMode = "editLine"; }
            //else if (modeId == "createLine") { canvasLineMode = "createLine"; }
            //else if (modeId == "removeLine") { canvasLineMode = "removeLine"; }
            //else if (modeId == "createPolygon") { canvasLineMode = "createPolygon"; }
            
            publicVaribles.canvasLineMode = canvasMode;
            publicVaribles.currentNumClick = numClick.First;
            publicVaribles.selectedLineObjects.Clear();
            publicVaribles.selectedPointPos = new Point(-20, -20);
        }
        public void setSelectedNode(int key)
        {
            publicVaribles.selectedNode = key;
        }
    }
}
