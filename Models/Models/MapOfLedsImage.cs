using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MapOfLedsImage : IMapOfLeds
    {
        public List<Led> Leds { get; }

        public Size MapSize { get;}

        public MapOfLedsImage(Size imageSize)
        {
            MapSize = imageSize;
            Leds = new List<Led>();
        }

        public void AddLed(int orderNum, Rectangle rectFromImage)
        {
            Led led = new Led()
            {
                LedRectangle = new Rectangle()
                {
                    Width = rectFromImage.Width,
                    Height = rectFromImage.Height,
                    Y = rectFromImage.Y,
                    X = rectFromImage.X
                },
                OrderNum = orderNum
            };

            if (LedRectangleIsValid(led) && LedIsNew(led))
                Leds.Add(led);
        }

        private bool LedRectangleIsValid(Led led)
        {
            if ((led.LedRectangle.Left >= 0 && led.LedRectangle.Right <= MapSize.Width - 1)
                && (led.LedRectangle.Bottom >= 0 && led.LedRectangle.Top <= MapSize.Height - 1))
                return true;
            return false;
        }

        private bool LedIsNew(Led newLed)
        {
            foreach (var oldLed  in Leds)
            {
                if (newLed.LedRectangle.IntersectsWith(oldLed.LedRectangle))
                    return false;
            }
            return true;
        }
    }
}
