using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineDrawerDemo
{
    public enum numClick
    {
        First,
        Second,
    };

    public enum CanvasModes
    {
        None,
        editLine,
        createLine,
        removeLine,
        createPolygon,
    }

    public class CanvasPublicVaribles
    {
        //
        // Neccesarry Public Class Varibles ------------------------------------------
        //
        public int selectedCanvasLineEnd = 0;
        //public int numClick = 0; //old system
        public numClick currentNumClick = numClick.First;
        

        public Point tempMouseStartPos = new Point();
        public Point tempMouseEndPos = new Point();
        public Point selectedPointPos = new Point();

        public int selectedNode = 0;
        public bool lockInToLineEnds = false;
        public bool lineMultiLocking = false;
        public bool DebugMode = false;

        //public string canvasLineMode = ""; //old system
        public CanvasModes canvasLineMode = CanvasModes.None;


        public int minSelectDistance = 10; //These four varibles could be parameters
        public int lineWidth = 1;
        public int numPolygonCorners = 4;
        public int radiusPolygon = 10;

        public bool ConfirmAction = false;
        public bool CancelAction = false;


        public Dictionary<int, int> selectedLineObjects = new Dictionary<int, int>();
    }
}
