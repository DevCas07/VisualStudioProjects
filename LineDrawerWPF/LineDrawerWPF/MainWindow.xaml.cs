using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static LineDrawerWPF.MainWindow;

namespace LineDrawerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // temp variabler i kod
        public int x1;
        public int y1;
        public int x2;
        public int y2;
        //
        Window1 CoordsWindow = new Window1();
        Canvas canvas = new Canvas();
        public MainWindow()
        {
            InitializeComponent();
            canvas.Height = this.Height - MenuStrip.Height; canvas.Width = this.Width;
        }
        List<LineObject> listLineObjects = new List<LineObject>();
        public class LineObject // LineObject objekt som kan kallas
        {
            public Int32 x1 { get; set; }
            public Int32 y1 { get; set; }
            public Int32 x2 { get; set; }
            public Int32 y2 { get; set; }
        }
        public int fixY(int yPos) { return (Int32)this.Height - 44 - yPos; }
        public void InitializeLineObject(Int16 x1, Int16 y1, Int16 x2, Int16 y2)
        {
            listLineObjects.Add(new LineObject { x1 = x1, y1 = y1, x2 = x2, y2 = y2 });
        }

        public void Draw(Canvas canvas) 
        { 
            
        }
        public void CreateGraphics()
        {
            foreach (LineObject lineObject in listLineObjects)
            {
                Point startPoint = new Point(lineObject.x1, lineObject.y1);
                Point endPoint = new Point(lineObject.x2, lineObject.y2);

                double kValue = (fixY(lineObject.y2) - fixY(lineObject.y1)) / (lineObject.x2 - lineObject.x1);
                double Hypotenuse = Math.Sqrt((fixY(lineObject.y2) - fixY(lineObject.y1)) * (fixY(lineObject.y2) - fixY(lineObject.y1)) + (lineObject.x2 - lineObject.x1) * (lineObject.x2 - lineObject.x1));
                double incrementValue = Hypotenuse * kValue;

                //rita linjen
                for (Int32 i = 0; i < Math.Round(Hypotenuse); i++)
                {
                    double yPoint = lineObject.y1 + i * kValue; // + i * incrementValue;
                    double xPoint = lineObject.x1 + i * kValue; // + i * incrementValue;
                    Point drawPoint = new Point(xPoint, yPoint);

                    Rectangle rec = new Rectangle();
                    rec.Height = 1;
                    rec.Width = 1;

                    rec.PointToScreen(drawPoint);
                }
                //...

                this.UpdateLayout();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCoordsWindow_Click(object sender, RoutedEventArgs e)
        {
            CoordsWindow.ShowDialog();
        }

        private void btnInitiateGraphics_Click(object sender, RoutedEventArgs e)
        {
            InitializeLineObject(16, 16, 32, 32);
            CreateGraphics();
        }
    }
}
