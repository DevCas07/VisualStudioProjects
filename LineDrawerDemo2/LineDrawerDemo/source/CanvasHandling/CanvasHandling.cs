using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LineDrawerDemo.MainWindow;
using System.Windows.Forms;
using System.Security.Policy;

namespace LineDrawerDemo
{
    /// <summary>
    /// Handles overall available functions to user and acts as a data intermediary between classes
    /// </summary>
    public class CanvasHandling
    {
        private LineHandling lineHandle;
        private FileHandling fileHandle;
        private ExceptionHandling exception;
        private CanvasDrawingHandling drawingHandle;
        
        internal CanvasPublicVaribles publicVaribles;

        private Dictionary<int, LineObject> LineObjects;

        public CanvasHandling()
        {
            publicVaribles = new CanvasPublicVaribles();

            lineHandle = new LineHandling(this);
            fileHandle = new FileHandling(this);
            drawingHandle = new CanvasDrawingHandling(this);

            exception = LineDrawerDemo.ExceptionHandling.GetInstance();
            //LineObjects = lineHandle.GetLineObjects();
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
        /// Adds polygon lines to Dictionary
        /// </summary>
        /// <param name="polygonLines"></param>
        public void InitialisePolygon(Dictionary<int, (Point, Point)> polygonLines)
        {
            lineHandle.InitialisePolygon(polygonLines);
        }

        public int getLowestAvailableKey()
        {
            return lineHandle.getLowestAvailableKey();
        }
        public bool checkKeyAvailability(int potentialKey)
        {
            return lineHandle.checkKeyAvailability(potentialKey);
                    //(potentialKey >= 0);
        }
        internal void ClearLineObjects()
        {
            lineHandle.clearLineObjects();
        }

        public LineObject GetLine(int key)
        {
            return lineHandle.GetLine(key);
        }

        public Dictionary<int, LineObject> GetLineObjects()
        {
            return lineHandle.GetLineObjects();
        }

        public int GetLineObjectsCount()
        {
            return GetLineObjects().Count();
        }

        /// <summary>
        /// Opens a SaveFileDialog
        /// </summary>
        public void viewSaveFileDialog()
        { 
            fileHandle.viewSaveFileDialog();
        }

        /// <summary>
        /// Opens a OpenFileDialog
        /// </summary>
        public void viewOpenFileDialog()
        {
            fileHandle.viewOpenFileDialog();
        }

        public void reloadFile(string fileLocation)
        {
            fileHandle.reloadfile(fileLocation);
        }

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
            lineHandle.createLine(key, x1, y1, x2, y2);
        }

        /// <summary>
        /// Removes line from dictionary
        /// </summary>
        /// <param name="key">line id</param>
        public void removeLine(int key) //Removes line from dictionary
        {
            lineHandle.removeLine(key);
        }

        /// <summary>
        /// Removes specified line from dictionary only after user confirmation
        /// </summary>
        /// <param name="key">line id</param>
        /// <param name="success"></param>
        public void removeLineByDialog(int key, out bool success)
        {
            lineHandle.removeLineByDialog(key, out success);
        }

        /// <summary>
        /// Removes specified lines from dictionary only after user confirmation
        /// </summary>
        /// <param name="lines">Each item value represents a line key/id</param>
        /// <param name="success"></param>
        public void removeLinesByDialog(List<int> lines, out bool success)
        {
            lineHandle.removeLinesByDialog(lines, out success);
        }

        /// Edits a specified line's properties, if specified key is diffrent than selected node the selected node line will be cloned anew over to specified key
        /// </summary>
        /// <param name="key">line id</param>        /// <summary>

        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public void saveLine(int key, int x1, int y1, int x2, int y2) //Edits line properties, if diffrent key it then copies line to new key
        {
            lineHandle.saveLine(key, x1, y1, x2, y2);
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
            lineHandle.editLine(key, x1, y1, x2, y2);
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
            return lineHandle.isWithinDistanceToLineEnd(key, posX, posY, minDistance, out lineEnd);
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
            return lineHandle.getClosestLinesAsArrayWithLineEnds(baseXPos, baseYPos, minDistance);
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
            return lineHandle.getClosestLineEndCoordinate(baseXPos, baseYPos, minDistance, out lineEnd, allowSelf);
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
            return lineHandle.getClosestLinesAsArray(baseXPos, baseYPos, minDistance);
        }

        /// <summary>
        /// Retrieves line end coordinate
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lineEnd"></param>
        /// <returns></returns>
        public Point getLineEndCoordinates(int key, int lineEnd) //Retrieves line end coordinates 
        {
            return lineHandle.getLineEndCoordinates(key, lineEnd);
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
            return lineHandle.createPolygonLines(xPos, yPos, numPolygonCorners, radius, angleExtension);
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
        /// <summary>
        /// Add line to temporary selectable list
        /// </summary>
        /// <param name="key">line id</param>
        /// <param name="lineEnd"></param>
        public void InitialiseTempSelectedLine(int key, int lineEnd)
        {
            publicVaribles.selectedLineObjects.Add(key, lineEnd);
        }
        public void ClearTempSelectedLine()
        {
            publicVaribles.selectedLineObjects.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvasMode"></param>
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
