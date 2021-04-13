using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester_100_module.Map
{
    public class MapOfLeds : IComparable
    {
        private const int SquareScopeForLedSide = 10;

        List<Led> LedsOnMap { get; }

        public MapOfLeds()
        {
            LedsOnMap = new List<Led>();
        }

        public bool AddLed(Rectangle led, int number, Bgr color)
        {
            Led _led = new Led(led, number, color);

            if (LedIsOnMap(_led))
                return false;

            LedsOnMap.Add(_led);
            return true;
        }

        private bool LedIsOnMap(Led led)
        {
            return LedsOnMap.Any(r => r.LedRect.IntersectsWith(led.LedRect));
            /*Rectangle ScopeRect = new Rectangle(led.LedRect.X - SquareScopeForLedSide/2, 
                led.LedRect.Y + SquareScopeForLedSide/2, 
                SquareScopeForLedSide, SquareScopeForLedSide);

            return LedsOnMap
                .Any(l => ScopeRect.Contains(l.LedRect.Location));*/
        }

        public Bitmap DrawOnImage()
        {
            var size = GetBitmapSize();
            Bitmap map = new Bitmap(size.Width, size.Height);
            
            Graphics gr = Graphics.FromImage(map);
            gr.Clear(Color.White);
            using (var pen = new Pen(Color.Black, 5))
                foreach (var led in LedsOnMap)
                {
                    pen.Color = Color.FromArgb((int)led.Color.Red, (int)led.Color.Green, (int)led.Color.Blue);
                    gr.DrawRectangle(pen, led.LedRect);
                    gr.DrawString($"Led №{LedsOnMap.IndexOf(led)}", new Font("Tahoma", 8), Brushes.Black, new PointF { X = led.LedRect.X, Y = led.LedRect.Y + led.LedRect.Height });
                }
            return map;
        }
        private Size GetBitmapSize()
        {
            if (LedsOnMap.Count() == 0)
                return new Size {Width = 100,Height = 100};

            return new Size
            {
                Width = LedsOnMap.Max(l => l.LedRect.Right) + 50,
                Height = LedsOnMap.Max(l => l.LedRect.Top) + 50
            };
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
