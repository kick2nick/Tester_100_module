using System.Windows;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Windows.Interop;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using Emgu.CV.Util;
using Tester_100_module.Map;
using Models;
using Models.Utils;

namespace Tester_100_module
{
    public partial class MainWindow : Window
    {
        private VideoCapture _capture;
        /*OpenFileDialog ofd;*/
        MapOfLeds map = new MapOfLeds();
        MapOfLedsImage baseMap;
        int counter = 0;
        public MainWindow()
        {
            InitializeComponent();
            if (_capture == null)
            {
                try
                {
                    _capture = new VideoCapture(0);
                    baseMap = new MapOfLedsImage(new System.Drawing.Size()
                    {
                        Width = _capture.Width,
                        Height = _capture.Height
                    });
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Capture Error");
                }
            }
            if (_capture != null)
                ComponentDispatcher.ThreadIdle += new EventHandler(ProcessFrame);

            /*ofd = new OpenFileDialog();
            ofd.ShowDialog();*/
        }
        private void ProcessFrame(object sender, EventArgs e)
        {
            /*if (ofd.FileName.Length < 2)
                return;*/
            Image<Bgr, byte> capturedImage = _capture.QueryFrame().ToImage<Bgr, byte>();//new Image<Bgr, byte>(ofd.FileName);

            Image<Hsv, byte> hsvImage = _capture.QueryFrame().ToImage<Hsv, byte>();//new Image<Hsv, byte>(ofd.FileName);
                                                                                   //_capture.QueryFrame().ToImage<Bgr, byte>();
                                                                                   //img1.Source = BitmapToImageSource(capturedImage.AsBitmap());


            //capturedImage.SmoothGaussian(11);
            //Bgr lower = new Bgr(slider2.Value, slider3.Value, slider4.Value);
            //Bgr higher = new Bgr(slider5.Value, slider6.Value, slider7.Value);



            Hsv lower = new Hsv(slider2.Value, slider3.Value, slider4.Value);
            Hsv higher = new Hsv(slider5.Value, slider6.Value, slider7.Value);
            var mask = hsvImage.InRange(lower, higher).Not();
            hsvImage.SetValue(new Hsv(0, 0, 0), mask);

            img2.Source = BitmapToImageSource(hsvImage.AsBitmap());


            //var mask = capturedImage.InRange(lower, higher).Not();
            //capturedImage.SetValue(new Bgr(0, 0, 0), mask);
            //img2.Source = BitmapToImageSource(capturedImage.AsBitmap());

            Image<Gray, byte> grayImage = hsvImage.SmoothGaussian(5).Convert<Gray, byte>().ThresholdBinary(new Gray(slider1.Value), new Gray(255)).Erode(1).Dilate(2);

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

            Mat hierarchy = new Mat();

            CvInvoke.FindContours(grayImage, contours, hierarchy, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            for (int i = 0; i < contours.Size; i++)
            {
                double perimeter = CvInvoke.ArcLength(contours[i], true);

                VectorOfPoint approximation = new VectorOfPoint();

                CvInvoke.ApproxPolyDP(contours[i], approximation, 0.04 * perimeter, true);


                //CvInvoke.DrawContours(capturedImage, contours, i, new MCvScalar(0, 0, 255), 2);


                var rect = CvInvoke.BoundingRectangle(contours[i]);
                if (rect.Size.Width > 10 && rect.Size.Height > 10)
                    capturedImage.Draw(rect, new Bgr(0, 0, 255));



                if ((bool)checkbox1.IsChecked && rect.Size.Width > 10 && rect.Size.Height > 10)
                {
                    /*rect.Height = (int)(rect.Height*2);
                    rect.Width = (int)(rect.Width * 2);
                    rect.X = (int)(rect.X - rect.Width * 0.25);
                    rect.Y = (int)(rect.Y - rect.Height * 0.25);
                    capturedImage.Draw(rect, new Bgr(0, 255, 0));*/


                    Mat mat = new Mat(capturedImage.Mat, rect);
                    Bgr color = mat.ToImage<Bgr, byte>().GetAverage();

                    baseMap.AddLed(i, rect);
                    map.AddLed(rect, i, color);
                    img1.Source = BitmapToImageSource(mat.ToImage<Bgr, byte>().AsBitmap());
                }

                /*if(approximation.Size > 5)
                {
                    CvInvoke.PutText(capturedImage, "EDL", new System.Drawing.Point(x, y), 
                        Emgu.CV.CvEnum.FontFace.HersheyPlain, 5, new MCvScalar(0, 0, 0), 1);
                }*/
            }
            img4.Source = BitmapToImageSource(capturedImage.AsBitmap());

            img3.Source = BitmapToImageSource(grayImage.AsBitmap());

            if ((bool)checkbox1.IsChecked && counter > 100)
            {
                counter = 0;
                var bitmap = capturedImage.AsBitmap();
                baseMap.DrawOnBitmap(bitmap, Color.White);
                img5.Source = BitmapToImageSource(bitmap);

                //img5.Source = BitmapToImageSource(map.DrawOnImage());
            }
            counter++;
        }
        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_capture != null)
                _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Exposure, SliderExposure.Value);
        }
    }
}
