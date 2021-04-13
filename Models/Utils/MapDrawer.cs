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

            using (var pen = new Pen(color, 5))
                foreach (var led in map.Leds)
                {
                    gr.DrawRectangle(pen, led.LedRectangle);
                    gr.DrawString($"Led №{led.OrderNum}", new Font("Tahoma", 8), Brushes.Black, new PointF { X = led.LedRectangle.X, Y = led.LedRectangle.Y + led.LedRectangle.Height });
                }
        }
    }
}
