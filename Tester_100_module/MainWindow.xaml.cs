using System.Windows;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Windows.Interop;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using Models;
using Models.Utils;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Data;

namespace Tester_100_module
{
    public partial class MainWindow : Window
    {
        public MapOfLedsImage LedsMap { get; private set; }
        public TesterState TesterState { get; set; }

        private VideoCapture _capture;
        private CalibrationService _calibrationService;
        private TestingService _testingService;

        public MainWindow()
        {
            InitializeComponent();
            if (_capture == null)
            {
                try
                {
                    _capture = new VideoCapture(0);
                    LedsMap = new MapOfLedsImage(new System.Drawing.Size()
                    {
                        Width = _capture.Width,
                        Height = _capture.Height
                    });

                    ledGrid.ItemsSource = LedsMap.Leds;
                    ledGrid.AutoGeneratingColumn += Led.OnAutoGeneratingColumn;
                    ledGrid.AutoGeneratingColumn += ColumnNameAttribute.DgPrimaryGrid_AutoGeneratingColumn;
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Capture Error");
                }
            }

            if (_capture != null)
                ComponentDispatcher.ThreadIdle += new EventHandler(ProcessFrame);
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            Image<Bgr, byte> capturedImage = _capture.QueryFrame().ToImage<Bgr, byte>();

            switch (TesterState)
            {
                case TesterState.Calibration:
                    _calibrationService.GetLedsFromImageToMap(capturedImage);
                    break;
                case TesterState.Testing:
                    _testingService.Testing(capturedImage);
                    break;
                case TesterState.Waiting:
                    break;
                default:
                    break;
            }

            var bitmap = capturedImage.AsBitmap();
            LedsMap.DrawOnBitmap(bitmap, Color.Red);
            img5.Source = bitmap.ToBitmapImage();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_capture != null)
                _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Exposure, SliderExposure.Value);
        }

        private void checkboxIdle_Checked(object sender, RoutedEventArgs e)
        {
            TesterState = TesterState.Waiting;
        }

        private void checkboxCalib_Checked(object sender, RoutedEventArgs e)
        {
            _calibrationService = new CalibrationService(LedsMap);
            TesterState = TesterState.Calibration;
        }

        private void checkboxTest_Checked(object sender, RoutedEventArgs e)
        {
            _testingService = new TestingService(LedsMap);
            _testingService.PropertyChanged += BoardTestedChanged;
            TesterState = TesterState.Testing;
        }
        private void BoardTestedChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "BoardsTested")
                labelBoardsCounter.Content = $"Протестировано\r\nплат {_testingService.BoardsTested}";
        }
    }
}
