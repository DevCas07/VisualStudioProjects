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

namespace MånghörningarDrawingDemo
{
    public partial class Form1 : Form
    {
        Dictionary<int, Point> Points = new Dictionary<int, Point>();
        Dictionary<int, (Point, Point)> Lines = new Dictionary<int, (Point, Point)>();

        public Form1()
        {
            InitializeComponent();
        }

        private void CanvasBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            g = e.Graphics;
            
            foreach (var point in Points.Keys) 
            {
                Point p = Points[point];
                Pen pen = new Pen(Brushes.Black, 4);
                Rectangle rec = new Rectangle();

                rec.X = p.X - 1;
                rec.Y = p.Y - 1;
                rec.Height = 2;
                rec.Width = 2;
                
                g.DrawRectangle(pen, rec);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            int xFix = 200;
            int yFix = 150;
            int radius = 100;
            int n = 5;
            double angleExtension = 270;
            
            double angle = 0;
            double radianAngle = 0;
            int i = 0;
            int x;
            int y;

            for (i = 0; i < n; i++)
            {

                angle = ((360 / n) * i) + angleExtension;
                radianAngle = (angle * (Math.PI / 180));

                x = (int)(radius * Math.Cos(radianAngle)) + xFix;
                y = (int)(radius * Math.Sin(radianAngle)) + yFix;

                Points.Add(i, new Point(x, y));
            }

            foreach (var point in Points)
            {
                string xPoint = point.Value.X.ToString();
                string yPoint = point.Value.Y.ToString();
                string key = point.Key.ToString();

                pointsTreeView.Nodes[0].Nodes.Add(Points.Keys.ToString(), "Key: " + key + ", ( " + xPoint + " : " + yPoint + " )");
            }

        }
    }
}
