using Emgu.CV;
using Emgu.CV.Structure;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Tester_100_module
{
    public class CalibrationService
    {
        private const int SquareWidthForLedInPixels = 10;
        private MapOfLedsImage _ledsMap;

        private Stopwatch _timer = new Stopwatch();

        public CalibrationService(MapOfLedsImage ledsMap)
        {
            _ledsMap = ledsMap;
            ledsMap.Leds.Clear();
        }

        public void GetLedsFromImageToMap(Image<Bgr, byte> capturedImage)
        {
            ImageProcessing proc = new ImageProcessing();
            var rects = proc
                .GetLedsRects(capturedImage)
                .Where(rect => rect.Size.Width > SquareWidthForLedInPixels && rect.Size.Height > SquareWidthForLedInPixels);

            foreach (var rect in rects)
            {
                capturedImage.Draw(rect, new Bgr(0, 0, 255));
                AddLedWithSpan(rect);
            }
        }

        private void AddLedWithSpan(Rectangle rect)
        {
            if (_ledsMap.Leds.Count == 0)
                _timer.Reset();

            if (_ledsMap.AddLed(_ledsMap.Leds.Count, rect, _timer.ElapsedMilliseconds))
            {
                _timer.Restart();
            }
        }
    }
}