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
    public partial class Form1 : Form
    {
        List<LineObject> LineObjects = new List<LineObject>();
        public class LineObject
        {
            public int startPosX { get; set; }
            public int startPosY { get; set; }
            public int endPosX { get; set; }
            public int endPosY { get; set; }
        }
        public Form1()
        {
            InitializeComponent();
        }
        public void InitializeLineObject(int StartPosX, int StartPosY, int EndPosX, int EndPosY) 
        { 
            LineObjects.Add(new LineObject( StartPosX = 1,  )); 
        }
        public int fixYCoord(int yPos) { return this.Size.Height - yPos - 44;  }
        private void Form1_Load(object sender, EventArgs e)
        {

            LineObjects.Add(new LineObject());
            this.SuspendLayout();
            Graphics g = this.CreateGraphics();

            foreach (LineObject lineObject in LineObjects)
            {
                Point startPoint = new Point(lineObject.startPosX, fixYCoord(lineObject.startPosY));
                Point endPoint = new Point(lineObject.endPosX, fixYCoord(lineObject.endPosY));

                g.DrawLine(Pens.Black, startPoint, endPoint);
            }
        }
    }
}
