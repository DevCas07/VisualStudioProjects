using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LineDrawerDemo.MainWindow;

namespace LineDrawerDemo
{
    public class LineHandling
    {
        //
        // Line Dictionary ------------------------------------------
        //
        public Dictionary<int, LineObject> LineObjects = new Dictionary<int, LineObject>();
        public LineHandling()
        {
            //Dictionary<int, LineObject> LineObjects = new Dictionary<int, LineObject>();
        }

        //
        // Base line functions handling ------------------------------------------
        //
        /// <summary>
        /// Add a LineObject/line to dictionary
        /// </summary>
        /// <param name="key">line id</param>
        /// <param name="StartPosX">x1</param>
        /// <param name="StartPosY">y1</param>
        /// <param name="EndPosX">x2</param>
        /// <param name="EndPosY">y2</param>
        public void InitializeLineObject(int key, int StartPosX, int StartPosY, int EndPosX, int EndPosY) //Creates new line and adds it onto the dictionary, --fixes new line's x and y coordinates to duplicates--
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
        public void editLineProperties(int key, int x1, int y1, int x2, int y2) //Edits a specific line's coordinates, --fixes it's x and y coordinates--, updates the TreeView
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
        public void editSpecificLineCoordinates(int key, int x1 = -1, int y1 = -1, int x2 = -1, int y2 = -1) //Setting a property as -1 unaffects it
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

        public void fixLineObjectCoordinates(int key) //Fixes the coordinates that will be painted onto the control from the "real" input coordinates, a bit broken and buggy
        {
            //LineObjects[key].x1 = fixXCoord(LineObjects[key].Realx1);
            //LineObjects[key].y1 = fixYCoord(LineObjects[key].Realy1);
            //LineObjects[key].x2 = fixXCoord(LineObjects[key].Realx2);
            //LineObjects[key].y2 = fixYCoord(LineObjects[key].Realy2);
        }
        //public int fixXCoord(int xPos) { return xPos + Canvas.Location.X; }
        //public int fixYCoord(int yPos) { return this.Size.Height - (yPos + 44 + (Canvas.Location.Y + Canvas.Size.Height)); }
    }
}
