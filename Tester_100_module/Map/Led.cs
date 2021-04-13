using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester_100_module.Map
{
    public class Led
    {
        public Rectangle LedRect { get; }
        public Bgr Color { get; set; }
        public Led(Rectangle ledRect, int number, Bgr color)
        {
            LedRect = ledRect;
            Color = color;
        }
    }
}
