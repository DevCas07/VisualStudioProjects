using System;
using System.Collections.Generic;
using System.Linq;
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

namespace LineDrawerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        List<LineObject> listLineObjects = new List<LineObject>();
        public class LineObject
        {
            public int x1 { get; set; }
            public int y1 { get; set; }
            public int x2 { get; set; }
            public int y2 { get; set; }
        }
        public int fixY(int yPos) { return (int)this.Height - 44 - yPos; }
        public void InitializeLineObject(int x1, int y1, int x2, int y2)
        {
            listLineObjects.Add(new LineObject { x1 = x1, y1 = y1, x2 = x2, y2 = y2 });
        }
        public void CreateGraphics()
        {
            this.UpdateLayout();
            
        }
    }
}
