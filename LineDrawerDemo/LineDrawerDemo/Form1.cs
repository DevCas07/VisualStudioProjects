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
        List<LineObject> LineObjects = new List<LineObject>();
        public TreeNode selectedNode;
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
        public void InitializeLineObject(int StartPosX, int StartPosY, int EndPosX, int EndPosY) //Create new line and add it onto the list, fixes new line's x and y coordinates to duplicates
        {
            LineObjects.Add(new LineObject
            {Realx1 = StartPosX, Realy1 = StartPosY, Realx2 = EndPosX, Realy2 = EndPosY });
            fixLineObjectCoordinates(LineObjects.Count - 1);
        }
        public void fixLineObjectCoordinates(int id) //Fixes the coordinates that will be painted onto the control from the "real" input coordinates
        {
            LineObjects[id].x1 = fixXCoord(LineObjects[id].Realx1);
            LineObjects[id].y1 = fixXCoord(LineObjects[id].Realy1);
            LineObjects[id].x2 = fixXCoord(LineObjects[id].Realx2);
            LineObjects[id].y2 = fixXCoord(LineObjects[id].Realy2);
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
            this.SuspendLayout();
            Graphics g = this.CreateGraphics(); //Create drawing canvas

            foreach (LineObject lineObject in LineObjects) //Extract every line from list and draw them
            {
                Point startPoint = new Point(lineObject.x1, fixYCoord(lineObject.y1));
                Point endPoint = new Point(lineObject.x2, fixYCoord(lineObject.y2));

                g.DrawLine(Pens.Black, startPoint, endPoint); //Draw Line from start to end points
            }
        }
        public void linesDraw()
        {
            if (LineObjects.Count > 0)
            {
                Canvas.Invalidate();
                Canvas.Update();
            }
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
            int tempX1; int tempY1; int tempX2; int tempY2; //Temporary varibles

            if (int.TryParse(x1Box.Text, out tempX1)) { } else { tempX1 = 0; }
            if (int.TryParse(y1Box.Text, out tempY1)) { } else { tempY1 = 0; }
            if (int.TryParse(x2Box.Text, out tempX2)) { } else { tempX2 = 1; }
            if (int.TryParse(y2Box.Text, out tempY2)) { } else { tempY2 = 1; }

            InitializeLineObject(tempX1, tempY1, tempX2, tempY2); //Adds new line to list
            updateLineTreeView();
            selectedNode = null;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (selectedNode != null) { 
                LineObjects.RemoveAt(selectedNode.Index);
            }
            updateLineTreeView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int tempX1; int tempY1; int tempX2; int tempY2; //Temporary varibles

            if (int.TryParse(x1Box.Text, out tempX1)) { } else { tempX1 = 0; }
            if (int.TryParse(y1Box.Text, out tempY1)) { } else { tempY1 = 0; }
            if (int.TryParse(x2Box.Text, out tempX2)) { } else { tempX2 = 1; }
            if (int.TryParse(y2Box.Text, out tempY2)) { } else { tempY2 = 1; }

            if (selectedNode != null) { 
                editLineCoordinates(selectedNode.Index, tempX1, tempY1, tempX2, tempY2); 
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

            foreach (LineObject lineObject in LineObjects) //Extract every line from list and draw them
            {
                Point startPoint = new Point(lineObject.x1, lineObject.y1);
                Point endPoint = new Point(lineObject.x2, lineObject.y2);

                g.DrawLine(Pens.Black, startPoint, endPoint); //Draw Line from start to end points
            }
        }

        public void updateLineTreeView() //Updates the treeView to correspond to the updated list's contents
        {
            int count = -1;
            linesTreeView.Nodes[0].Nodes.Clear();
            foreach (LineObject lineObject in LineObjects)
            {
                count++;
                linesTreeView.Nodes[0].Nodes.Add("line" + count, "line" + count);

            }
        }

        public void editLineCoordinates(int id, int x1, int y1, int x2, int y2) //Edits a specific line's coordinates, fixes it's x and y coordinates, updates the TreeView
        {
            if (x1 != 0) { LineObjects[id].Realx1 = x1; }
            if (y1 != 0) { LineObjects[id].Realy1 = y1; }
            if (x2 != 0) { LineObjects[id].Realx2 = x2; }
            if (y2 != 0) { LineObjects[id].Realy2 = y2; }

            fixLineObjectCoordinates(id);
            updateLineTreeView();
        }

        private void linesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (linesTreeView.SelectedNode.Tag != "main") {

                selectedNode = linesTreeView.SelectedNode;

                if (LineObjects[selectedNode.Index].x1 != 0) { x1Box.Text = LineObjects[selectedNode.Index].x1.ToString(); }
                if (LineObjects[selectedNode.Index].y1 != 0) { y1Box.Text = LineObjects[selectedNode.Index].y1.ToString(); }
                if (LineObjects[selectedNode.Index].x2 != 0) { x2Box.Text = LineObjects[selectedNode.Index].x2.ToString(); }
                if (LineObjects[selectedNode.Index].y2 != 0) { y2Box.Text = LineObjects[selectedNode.Index].y2.ToString(); }
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
