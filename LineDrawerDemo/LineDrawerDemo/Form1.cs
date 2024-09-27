using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LineDrawerDemo.MainWindow;

namespace LineDrawerDemo
{
    public partial class MainWindow : Form
    {
        public int numClicks = 0;
        public Point tempMouseStartPos = new Point();
        public Point tempMouseEndPos = new Point();
        Dictionary<int, LineObject> LineObjects = new Dictionary<int, LineObject>();
        public int selectedNode;
        public class LineObject
        {
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
        public void InitializeLineObject(int key, int StartPosX, int StartPosY, int EndPosX, int EndPosY) //Create new line and add it onto the list, fixes new line's x and y coordinates to duplicates
        {
            LineObjects.Add(key, new LineObject
            {Realx1 = StartPosX, Realy1 = StartPosY, Realx2 = EndPosX, Realy2 = EndPosY });
            //fixLineObjectCoordinates(LineObjects.Count - 1); Broken and buggy
        }
        public void fixLineObjectCoordinates(int key) //Fixes the coordinates that will be painted onto the control from the "real" input coordinates, a bit broken and buggy
        {
            LineObjects[key].x1 = fixXCoord(LineObjects[key].Realx1);
            LineObjects[key].y1 = fixXCoord(LineObjects[key].Realy1);
            LineObjects[key].x2 = fixXCoord(LineObjects[key].Realx2);
            LineObjects[key].y2 = fixXCoord(LineObjects[key].Realy2);
        }
        public int fixXCoord(int xPos) {  return xPos + Canvas.Location.X; }
        public int fixYCoord(int yPos) { return this.Size.Height - (yPos + 44 + (Canvas.Location.Y + Canvas.Size.Height)) ; }
        public void OnMouseClickGetCoords(int xPos, int yPos) //doesn't fully work, cant detect mouse click
        {
            if (numClicks == 0) { numClicks = 1; return; }
            else if (numClicks == 1)
            {
                numClicks = 2;
                tempMouseStartPos.X = xPos;
                tempMouseStartPos.Y = yPos;
            }
            else if (numClicks == 2)
            {
                numClicks = 1;
                tempMouseEndPos.X = xPos;
                tempMouseEndPos.Y = yPos;

                //InitializeLineObject(tempMouseStartPos.X, tempMouseStartPos.Y, tempMouseEndPos.X, tempMouseEndPos.Y);
            }

        }

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
        public void linesDraw()
        {
            if (LineObjects.Keys.Count > 0)
            {
                Canvas.Invalidate();
                Canvas.Update();
            }
        }
        public void updateLineTreeView() //Updates the treeView to correspond to the updated list's contents
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
        }

        public void editLineProperties(int key, int x1, int y1, int x2, int y2) //Edits a specific line's coordinates, fixes it's x and y coordinates, updates the TreeView
        {
            if (x1 != 0) { LineObjects[key].Realx1 = x1; }
            if (y1 != 0) { LineObjects[key].Realy1 = y1; }
            if (x2 != 0) { LineObjects[key].Realx2 = x2; }
            if (y2 != 0) { LineObjects[key].Realy2 = y2; }

            //fixLineObjectCoordinates(key); Broken and buggy
        }
        public void copyLineObjectWithNewKey(int key, int newKey)
        {
            InitializeLineObject(newKey, LineObjects[key].Realx1, LineObjects[key].Realy1, LineObjects[key].Realx2, LineObjects[key].Realy2);
            LineObjects.Remove(key);
        }
        public bool checkKeyAvailability(int potenstialKey)
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

        private void MainWindow_Load(object sender, EventArgs e)
        {
            //InitializeLineObject(10, 20, 120, 80);
            //BeginGraphics();
        }

        private void MainWindow_MouseClick(object sender, MouseEventArgs e)
        {
            OnMouseClickGetCoords(e.Location.X, e.Location.Y);

        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            int tempX1; int tempY1; int tempX2; int tempY2; int tempLineKeyBox; //Temporary varibles

            if (int.TryParse(x1Box.Text, out tempX1)) { } else { tempX1 = 0; }
            if (int.TryParse(y1Box.Text, out tempY1)) { } else { tempY1 = 0; }
            if (int.TryParse(x2Box.Text, out tempX2)) { } else { tempX2 = 1; }
            if (int.TryParse(y2Box.Text, out tempY2)) { } else { tempY2 = 1; }
            
            tempLineKeyBox = ((int)lineKeyBox.Value);
            if (checkKeyAvailability(tempLineKeyBox))
            {
                InitializeLineObject(tempLineKeyBox, tempX1, tempY1, tempX2, tempY2); //Adds new line to list
                
                updateLineTreeView();

                selectedNode = tempLineKeyBox;
                selectedNodeLabel.Text = "Selected node: [" + selectedNode + "]";
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (selectedNode != null) { 
                LineObjects.Remove(selectedNode);
            }
            updateLineTreeView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int tempX1; int tempY1; int tempX2; int tempY2; int tempLineKeyBox; //Temporary varibles

            if (int.TryParse(x1Box.Text, out tempX1)) { } else { tempX1 = 0; }
            if (int.TryParse(y1Box.Text, out tempY1)) { } else { tempY1 = 0; }
            if (int.TryParse(x2Box.Text, out tempX2)) { } else { tempX2 = 1; }
            if (int.TryParse(y2Box.Text, out tempY2)) { } else { tempY2 = 1; }

            tempLineKeyBox = ((int)lineKeyBox.Value);

            if (selectedNode != null) {
                if (selectedNode == tempLineKeyBox) {
                    editLineProperties(selectedNode, tempX1, tempY1, tempX2, tempY2);
                }
                else if (checkKeyAvailability(tempLineKeyBox)) {
                    copyLineObjectWithNewKey(selectedNode, tempLineKeyBox);
                    editLineProperties(tempLineKeyBox, tempX1, tempY1, tempX2, tempY2);
                }

                updateLineTreeView();
                selectedNode = tempLineKeyBox;
            }
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
                Point startPoint = new Point(lineObject.Value.x1, lineObject.Value.y1);
                Point endPoint = new Point(lineObject.Value.x2, lineObject.Value.y2);

                g.DrawLine(Pens.Black, startPoint, endPoint); //Draw Line from start to end points
            }
        }

        private void linesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (linesTreeView.SelectedNode.Tag != "main") {

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

        private void tabControl1_Resize(object sender, EventArgs e)
        {
            linesDraw();
        }

        private void MainWindow_ResizeEnd(object sender, EventArgs e)
        {
            tabControl1.Dock = DockStyle.Fill;
            Canvas.Dock = DockStyle.Fill;
        }
    }
}
