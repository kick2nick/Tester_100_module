using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Tester_100_module
{
    class ImageProcessing
    {
        private const int GasussianKernelSize = 5;
        private const int ErodeIterations = 1;
        private const int DilateIterations = 2;
        private const int LedWidthAndHeightInPixels = 10;
        private const int LedValueTreshold = 200;

        public Hsv MinFilter { get; } = new Hsv(0, 0, 255);
        public Hsv MaxFilter { get; } = new Hsv(255, 255, 255);
        public double BinaryTreshold { get; set; } = 70;

        public Action<Rectangle> _reactangleAction;

        public ImageProcessing()
        {

        }

        public ImageProcessing(Action<Rectangle> rectangleAction)
        {
            _reactangleAction = rectangleAction;
        }

        public ImageProcessing(Hsv minFilter, Hsv maxFilter, double binaryTreshold)
        {
            MinFilter = minFilter;
            MaxFilter = maxFilter;
        }

        public List<Rectangle> GetLedsRects(Image<Bgr, byte> imageToProccess)
        {
            List<Rectangle> foundedRects = new List<Rectangle>();

            var hsvImage = imageToProccess.Convert<Hsv, byte>();

            var mask = hsvImage.InRange(MinFilter, MaxFilter).Not();
            hsvImage.SetValue(new Hsv(0, 0, 0), mask);

            Image<Gray, byte> grayImage = hsvImage
                .SmoothGaussian(GasussianKernelSize)
                .Convert<Gray, byte>()
                .ThresholdBinary(new Gray(BinaryTreshold), new Gray(byte.MaxValue))
                .Erode(ErodeIterations)
                .Dilate(DilateIterations);

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            CvInvoke.FindContours(grayImage, contours, hierarchy, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            for (int i = 0; i < contours.Size; i++)
            {
                var rect = CvInvoke.BoundingRectangle(contours[i]);
                if (rect.Size.Width > LedWidthAndHeightInPixels
                    && rect.Size.Height > LedWidthAndHeightInPixels)
                {
                    foundedRects.Add(rect);
                    _reactangleAction?.Invoke(rect);
                }
            }
            return foundedRects;
        }

        public static bool LedIsOn(Image<Bgr, byte> ledRect)
        {
            var hsvRect = ledRect.Convert<Hsv, byte>();
            var avg = hsvRect.GetAverage();
            if (avg.Value >= LedValueTreshold)
                return true;
            return false;
        }
    }
}