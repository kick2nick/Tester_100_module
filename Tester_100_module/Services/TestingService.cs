using Emgu.CV;
using Emgu.CV.Structure;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tester_100_module
{
    public class TestingService : INotifyPropertyChanged
    {
        private MapOfLedsImage _ledsMap;
        private Stopwatch stopwatch = new Stopwatch(); 
        public TestingService( MapOfLedsImage ledsMap)
        {
            int[] a = new int[10];

            if (ledsMap == null)
                return;

            _ledsMap = ledsMap;
            foreach (var led in _ledsMap.Leds)
            {
                led.Found = false;
            }
        }

        private static int ledOrder=0;
        public void Testing(Image<Bgr, byte> capturedImage)
        {
            if (_ledsMap.Leds.Count == 0)
                return;

            if (DelayAfterSuccess)
            {
                for (int i = 0; i < _ledsMap.Leds.Count; i++)
                {
                    if(ImageProcessing.LedIsOn(capturedImage.GetSubRect(_ledsMap.Leds[i].LedRectangle)))
                    {
                        stopwatch.Restart();
                        i = _ledsMap.Leds.Count + 1;
                    }
                }
                if (stopwatch.ElapsedMilliseconds > 2000)
                {
                    DelayAfterSuccess = false;
                }
            }


            //Finding of first led
            if(_ledsMap.Leds.All(s => s.Found == false))
            {
                var firstLed = _ledsMap.Leds.First(l => l.OrderNum == 0);
                firstLed.Found = ImageProcessing.LedIsOn(capturedImage.GetSubRect(firstLed.LedRectangle));
                if(firstLed.Found)
                {
                    stopwatch.Restart();
                    ledOrder++;
                }
            }
            //other leds
            else
            {
                var nextLed = _ledsMap.Leds[ledOrder];
                
                nextLed.Found = ImageProcessing.LedIsOn(capturedImage.GetSubRect(nextLed.LedRectangle));

                if (nextLed.Found)
                {
                    stopwatch.Restart();
                    ledOrder++;
                    if (nextLed.Coupled == true && _ledsMap.Leds.Count() < ledOrder)
                    {
                        if (_ledsMap.Leds[ledOrder].Coupled == true)
                        {
                            this.Testing(capturedImage);
                        }
                    }
                }
                else if(stopwatch.ElapsedMilliseconds > nextLed.TimeFromPrevious * 3 && nextLed.Coupled != true)
                {
                    ResetToStart(false);
                }
            }

            //when all leds are found
            if (_ledsMap.Leds.All(s => s.Found == true))
            {
                ResetToStart(true);
            }
        }

        private bool DelayAfterSuccess;
        private void ResetToStart(bool withOk)
        {
            ledOrder = 0;
            if (withOk)
            {
                BoardsTested++;
                DelayAfterSuccess = true;
                stopwatch.Restart();
            }
                

            foreach (var led in _ledsMap.Leds)
            {
                led.Found = false;
            }
        }

        private int _boardsTested;
        public int BoardsTested
        {
            get { return _boardsTested; }
            set
            {
                _boardsTested = value;
                OnPropertyChanged("BoardsTested");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
