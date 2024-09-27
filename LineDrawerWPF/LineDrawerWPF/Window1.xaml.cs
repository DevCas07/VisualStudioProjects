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
using System.Windows.Shapes;

namespace LineDrawerWPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            if (x1Box.Text != null || x2Box.Text != null || y1Box.Text != null || y2Box.Text != null) // testar om textboxarna inte har ingenting
            {
                //skickar över värdena till MainWindow's temp variabler
                //mainWindow.x1 = Int32.Parse(x1Box.Text);
                //mainWindow.x2 = Int32.Parse(x2Box.Text);
                //mainWindow.y1 = Int32.Parse(y1Box.Text);
                //mainWindow.y2 = Int32.Parse(y2Box.Text);
                mainWindow.InitializeLineObject(Int16.Parse(x1Box.Text), Int16.Parse(y1Box.Text), Int16.Parse(x2Box.Text), Int16.Parse(y2Box.Text));
            }
            else
            {
                MessageBox.Show("Error");
            }

            this.Close();
        }
    }
}
