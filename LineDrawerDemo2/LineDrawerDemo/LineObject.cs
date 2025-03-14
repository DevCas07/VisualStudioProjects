using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineDrawerDemo
{
    public class LineObject //LineObject 
    {
        public int key { get; set; }
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
}
