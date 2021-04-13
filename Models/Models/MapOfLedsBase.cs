using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MapOfLedsBase : IMapOfLeds
    {
        public const int FieldSizeY = 1000;
        public const int FieldSizeX = 1000;

        public List<Led> Leds { get; }

        private readonly float scaleX;
        private readonly float scaleY;

        public Size MapSize { get => new Size(FieldSizeX, FieldSizeY); private set { } }

        public MapOfLedsBase(Size imageSize)
        {
            Leds = new List<Led>();

            scaleX = FieldSizeX / imageSize.Width;
            scaleY = FieldSizeY / imageSize.Height;
            MapSize = imageSize;
        }

        public void AddLed(int orderNum, Rectangle rectFromImage)
        {
            Led led = new Led()
            {
                LedRectangle = new Rectangle()
                {
                    Width = (int)(rectFromImage.Width * scaleX),
                    Height = (int)(rectFromImage.Height * scaleY),
                    Y = (int)(rectFromImage.Y * scaleY),
                    X = (int)(rectFromImage.X * scaleX),
                },
                OrderNum = orderNum
            };

            if (LedPositionIsValid(led, new Size() {Width = FieldSizeX, Height = FieldSizeY }))
                Leds.Add(led);
        }

        public List<Led> GetLedsScaledOnImage(float scale)
        {
            var resultList = new List<Led>();
            foreach (var led in Leds)
            {
                resultList.Add(new Led()
                {
                    OrderNum = led.OrderNum,
                    LedRectangle = new Rectangle()
                    {
                        Width = (int)((led.LedRectangle.Width / scaleX) * scale),
                        Height = (int)((led.LedRectangle.Height / scaleY) * scale),
                        Y = (int)((led.LedRectangle.Y / scaleY) * scale),
                        X = (int)((led.LedRectangle.X / scaleX) * scale)
                    }
                });
            }
            if (resultList.All(s => LedPositionIsValid(s, MapSize)))
                return resultList;

            return null;
        }

        private bool LedPositionIsValid(Led led, Size size)
        {
            if ((led.LedRectangle.Left >= 0 && led.LedRectangle.Right <= size.Width - 1)
                && (led.LedRectangle.Bottom >= 0 && led.LedRectangle.Top <= size.Height - 1))
                return true;
            return false;
        }
    }
}
