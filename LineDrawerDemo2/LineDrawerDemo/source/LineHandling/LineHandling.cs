using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LineDrawerDemo.MainWindow;

namespace LineDrawerDemo
{
    public class LineHandling
    {
        //
        // Line Dictionary ------------------------------------------
        //
        private Dictionary<int, LineObject> LineObjects = new Dictionary<int, LineObject>();
        
        private CanvasHandling canvasHandle;
        private ExceptionHandling exception;

        public LineHandling(CanvasHandling canvasHandle)
        {
            this.canvasHandle = canvasHandle;
            exception = LineDrawerDemo.ExceptionHandling.GetInstance();
            //Dictionary<int, LineObject> LineObjects = new Dictionary<int, LineObject>();
        }

        public LineObject GetLine(int key)
        {
            return LineObjects[key];
        }
        public Dictionary<int, LineObject> GetLineObjects()
        {
            return LineObjects;
        }
        public void clearLineObjects()
        {
            LineObjects.Clear();
        }

        //
        // Base line functions handling ------------------------------------------
        //

        /// <summary>
        /// Remove a LineObject/line from dictionary
        /// </summary>
        /// <param name="key">line id</param>
        private void RemoveLineObject(int key)
        {
            LineObjects.Remove(key);
        }

        /// <summary>
        /// Add a LineObject/line to dictionary
        /// </summary>
        /// <param name="key">line id</param>
        /// <param name="StartPosX">x1</param>
        /// <param name="StartPosY">y1</param>
        /// <param name="EndPosX">x2</param>
        /// <param name="EndPosY">y2</param>
        private void InitializeLineObject(int key, int StartPosX, int StartPosY, int EndPosX, int EndPosY) //Creates new line and adds it onto the dictionary, --fixes new line's x and y coordinates to duplicates--
        {
            LineObjects.Add(key, new LineObject
            { Realx1 = StartPosX, Realy1 = StartPosY, Realx2 = EndPosX, Realy2 = EndPosY });
            //fixLineObjectCoordinates(key); // Broken and buggy, test it out
        }

        /// <summary>
        /// Edits a specific line's coordinates
        /// </summary>
        /// <param name="key">line id</param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        private void editLineProperties(int key, int x1 = 0, int y1 = 0, int x2 = 0, int y2 = 0) //Edits a specific line's coordinates, --fixes it's x and y coordinates--, updates the TreeView
        {

            if (x1 != 0) { LineObjects[key].Realx1 = x1; }
            if (y1 != 0) { LineObjects[key].Realy1 = y1; }
            if (x2 != 0) { LineObjects[key].Realx2 = x2; }
            if (y2 != 0) { LineObjects[key].Realy2 = y2; }

            //fixLineObjectCoordinates(key); Broken and buggy, test it out
        }
        /// <summary>
        /// Edits a specific line's coordinates, setting a property as -1 unaffects it
        /// </summary>
        /// <param name="key">line id</param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        private void editSpecificLineCoordinates(int key, int x1 = -1, int y1 = -1, int x2 = -1, int y2 = -1) //Setting a property as -1 unaffects it
        {
            int tempX1; int tempY1; int tempX2; int tempY2;

            if (x1 == -1) { tempX1 = LineObjects[key].Realx1; } else { tempX1 = x1; }
            if (y1 == -1) { tempY1 = LineObjects[key].Realy1; } else { tempY1 = y1; }
            if (x2 == -1) { tempX2 = LineObjects[key].Realx2; } else { tempX2 = x2; }
            if (y2 == -1) { tempY2 = LineObjects[key].Realy2; } else { tempY2 = y2; }
            editLineProperties(key, tempX1, tempY1, tempX2, tempY2);
        }
        private void copyLineObjectWithNewKey(int key, int newKey) //Copies line with new key and removes old line with old key
        {
            InitializeLineObject(newKey, LineObjects[key].Realx1, LineObjects[key].Realy1, LineObjects[key].Realx2, LineObjects[key].Realy2);
            LineObjects.Remove(key);
        }
        /// <summary>
        /// Checks if specified position within dictionary is available
        /// </summary>
        /// <param name="potentialKey">Checks if specified key/id is not already taken</param>
        /// <returns></returns>
        public bool checkKeyAvailability(int potentialKey) //Checks if specified position within dictionary is available
        {
            bool availability = true;

            if (LineObjects.Keys.Count > 0)
            {
                foreach (var lineObject in LineObjects.Keys)
                {
                    if (lineObject == potentialKey) { availability = false; break; }
                }
            }
            return availability;
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
        // Line coordinate fixing handling ------------------------------------------
        //

        private void fixLineObjectCoordinates(int key) //Fixes the coordinates that will be painted onto the control from the "real" input coordinates, a bit broken and buggy
        {
            //LineObjects[key].x1 = fixXCoord(LineObjects[key].Realx1);
            //LineObjects[key].y1 = fixYCoord(LineObjects[key].Realy1);
            //LineObjects[key].x2 = fixXCoord(LineObjects[key].Realx2);
            //LineObjects[key].y2 = fixYCoord(LineObjects[key].Realy2);
        }
        //public int fixXCoord(int xPos) { return xPos + Canvas.Location.X; }
        //public int fixYCoord(int yPos) { return this.Size.Height - (yPos + 44 + (Canvas.Location.Y + Canvas.Size.Height)); }

        //
        // intermediate line handling ------------------------------------------
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
            if (checkKeyAvailability(key))
            {
                InitializeLineObject(key, x1, y1, x2, y2); //Adds new line to list
            }
            else { exception.generateException(CustomExceptions.already_occupied_key); } // already occupied key

        }

        /// <summary>
        /// Removes line from dictionary
        /// </summary>
        /// <param name="key">line id</param>
        public void removeLine(int key) //Removes line from dictionary
        {
            if (!checkKeyAvailability(key))
            {
                RemoveLineObject(key);
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
            var dialog = MessageBox.Show("Remove line:" + canvasHandle.publicVaribles.selectedNode, "Confirm removal", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

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
            if (LineObjects.Count > 0)
            {
                if (canvasHandle.publicVaribles.selectedNode == key)
                {
                    editLineProperties(canvasHandle.publicVaribles.selectedNode, x1, y1, x2, y2);
                }
                else if (checkKeyAvailability(key))
                {
                    copyLineObjectWithNewKey(canvasHandle.publicVaribles.selectedNode, key);
                    editLineProperties(key, x1, y1, x2, y2);
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
            Dictionary<int, LineObject> lineObjects = GetLineObjects();
            if (lineObjects.Count > 0)
            {
                editLineProperties(canvasHandle.publicVaribles.selectedNode, x1, y1, x2, y2);
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
            Dictionary<int, LineObject> lineObjects = GetLineObjects();
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

                    if (line[0] != canvasHandle.publicVaribles.selectedNode)
                    {
                        LineObject lineObject = LineObjects[line[0]];

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
                LineObject lineObject = LineObjects[line[0]];

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
            LineObject line = LineObjects[key];

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
                int key = getLowestAvailableKey();
                int x1 = polygonLine.Value.Item1.X;
                int y1 = polygonLine.Value.Item1.Y;
                int x2 = polygonLine.Value.Item2.X;
                int y2 = polygonLine.Value.Item2.Y;

                InitializeLineObject(key, x1, y1, x2, y2);
            }
        }
    }
}
