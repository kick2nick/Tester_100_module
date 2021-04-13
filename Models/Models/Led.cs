using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Models
{
    public class Led
    {
        public Rectangle LedRectangle { get; set; }

        public int OrderNum { get; set; }

        public Point CentralPoint { get => new Point(LedRectangle.X + LedRectangle.Width / 2, LedRectangle.Y - LedRectangle.Y / 2); }
    }
}
