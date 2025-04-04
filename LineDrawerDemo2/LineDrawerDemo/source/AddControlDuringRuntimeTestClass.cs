using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineDrawerDemo
{
    class AddControlDuringRuntimeTestClass
    {
        private PictureBox pictureBox1;

        public AddControlDuringRuntimeTestClass()
        {
            pictureBox1 = new PictureBox();

            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.BackColor = Color.Black;
            pictureBox1.Location = new Point(10, 10);
            pictureBox1.Size = new Size(100, 100);
        }

        public Control getControl()
        {
                return pictureBox1;
        }

    }
}
