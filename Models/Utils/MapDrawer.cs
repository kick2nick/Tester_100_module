using System.Drawing;

namespace Models.Utils
{
    public static class MapDrawer
    {
        public static void DrawOnBitmap(this MapOfLedsImage map, Bitmap image, Color color)
        {
            if (image.Size != map.MapSize)
                return;

            Graphics gr = Graphics.FromImage(image);

            using (var pen = new Pen(color, 3))
                foreach (var led in map.Leds)
                {
                    gr.DrawRectangle(pen, led.LedRectangle);
                    gr.DrawString($"Led №{led.LedNum}", new Font("Tahoma", 10), Brushes.Red, new PointF { X = led.LedRectangle.X, Y = led.LedRectangle.Y + led.LedRectangle.Height });
                }
        }
    }
}
