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

namespace LineDrawerDemo
{
    public partial class MainWindow : Form
    {
        public int numClicks = 0;
        public Point tempMouseStartPos = new Point();
        public Point tempMouseEndPos = new Point();
        List<LineObject> LineObjects = new List<LineObject>();
        public class LineObject
        {
            public int startPosX { get; set; }
            public int startPosY { get; set; }
            public int endPosX { get; set; }
            public int endPosY { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
        }
        public void InitializeLineObject(int StartPosX, int StartPosY, int EndPosX, int EndPosY) //Create new line and add it onto the list
        {
            LineObjects.Add(new LineObject 
            { startPosX = StartPosX, startPosY = StartPosY, endPosX = EndPosX, endPosY = EndPosY });
        }
        public int fixYCoord(int yPos) { return this.Size.Height - yPos - 44 ; }
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

                InitializeLineObject(tempMouseStartPos.X, tempMouseStartPos.Y, tempMouseEndPos.X, tempMouseEndPos.Y);
            }

        }
        public void BeginGraphics()
        {
            this.SuspendLayout();
            Graphics g = this.CreateGraphics(); //Create drawing canvas

            foreach (LineObject lineObject in LineObjects) //Extract every line from list and draw them
            {
                Point startPoint = new Point(lineObject.startPosX, fixYCoord(lineObject.startPosY));
                Point endPoint = new Point(lineObject.endPosX, fixYCoord(lineObject.endPosY));

                g.DrawLine(Pens.Black, startPoint, endPoint); //Draw Line from start to end points
            }
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
            BeginGraphics();
        }

        private void MainWindow_MouseClick(object sender, MouseEventArgs e)
        {
            OnMouseClickGetCoords(e.Location.X, e.Location.Y);

        }

        private void btnInitiate_Click(object sender, EventArgs e)
        {
            
        }
    }
}
