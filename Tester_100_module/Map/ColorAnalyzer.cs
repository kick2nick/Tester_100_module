using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester_100_module.Map
{
    public static class ColorAnalyzer
    {
        private static readonly Bgr White = new Bgr(255, 255, 255);

        private static readonly Bgr Red = new Bgr(0, 0, 255);
        public static Bgr GetLedColor(this Image<Hsv, byte> image)
        {
            var splitted = image.Split();

            var Hue = splitted[0].GetAverage();
            var Saturation = splitted[1].GetAverage();
            var Value = splitted[2].GetAverage();

            if (Value.Intensity > 250)
                return White;

            if ((Hue.Intensity < 14 || Hue.Intensity >241) && (Value.Intensity > 100 && Saturation.Intensity > 100))
                return Red;

            return new Bgr(0, 0, 0);
        }
    }
}
