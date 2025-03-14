using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
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

            foreach (var lines in Lines)
            {
                Point p1 = lines.Value.Item1;
                Point p2 = lines.Value.Item2;
                Pen pen = new Pen(Brushes.Black, 4);
                

                g.DrawLine(pen, p1, p2);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            int xFix = 200;
            int yFix = 150;
            int radius = 100;
            int n = 8;
            double angleExtension = 0; //270
            
            double angle = 0;
            double radianAngle = 0;
            int i = 0;
            int x = 0;
            int y = 0;

            angle = ((360 / n) * i) + angleExtension;
            radianAngle = (angle * (Math.PI / 180));

            x = (int)(radius * Math.Cos(radianAngle)) + xFix;
            y = (int)(radius * Math.Sin(radianAngle)) + yFix;

            Points.Add(i, new Point(x, y));

            for (i = 1; i < 2*n + 1; i = i + 2)
            {

                angle = ((360 / n) * (i +1)/ 2) + angleExtension;
                radianAngle = (angle * (Math.PI / 180));

                x = (int)(radius * Math.Cos(radianAngle)) + xFix;
                y = (int)(radius * Math.Sin(radianAngle)) + yFix;

                Points.Add(i, new Point(x, y));

               

                Points.Add((i + 1), new Point(x, y));
            }

            for (i = 0; i < 2 * n; i = i + 2)
            {
                Lines.Add(i / 2, (Points[i], Points[i + 1]));
            }



            foreach (var point in Points)
            {
                string xPoint = point.Value.X.ToString();
                string yPoint = point.Value.Y.ToString();
                string key = point.Key.ToString();

                TreeNode node = new TreeNode();
                node.Name = key;
                node.Tag = key;
                node.Text = "Key: " + key + ", ( " + xPoint + " : " + yPoint + " )";

                linesTreeView.Nodes[0].Nodes.Add(node);
            }

            foreach (var line in Lines)
            {

                string xPoint1 = line.Value.Item1.X.ToString();
                string yPoint1 = line.Value.Item1.Y.ToString();
                string xPoint2 = line.Value.Item2.X.ToString();
                string yPoint2 = line.Value.Item2.Y.ToString();

                string key = line.Key.ToString();

                TreeNode node = new TreeNode();
                node.Name = key;
                node.Tag = key;
                node.Text = "Key: " + key + ", ( " + xPoint1 + " : " + yPoint1 + " )" + ", ( " + xPoint2 + " : " + yPoint2 + " )";

                pointsTreeView.Nodes[0].Nodes.Add(node);
            }

        }
    }
}
