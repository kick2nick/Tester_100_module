using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MapOfLedsImage : IMapOfLeds
    {
        public ObservableCollection<Led> Leds { get; private set; }

        public Action OrderChangedAction;

        public Size MapSize { get; }

        public MapOfLedsImage(Size imageSize)
        {
            MapSize = imageSize;
            Leds = new ObservableCollection<Led>();
            //Leds.CollectionChanged += OrderNumChanged;
        }

        public bool AddLed(int orderNum, Rectangle rectFromImage, long interval)
        {
            var coupled = interval == 0 && Leds.Count > 0;
            if (coupled)
            {
                //Leds[orderNum-1].Coupled = true;
            }

            Led led = new Led()
            {
                LedRectangle = new Rectangle()
                {
                    Width = rectFromImage.Width,
                    Height = rectFromImage.Height,
                    Y = rectFromImage.Y,
                    X = rectFromImage.X
                },
                LedNum = orderNum,
                Found = false,
                OrderNum = orderNum,
                TimeFromPrevious = interval,
                Coupled = coupled
            };

            if (LedRectangleIsValid(led) && LedIsNew(led))
            {
                Leds.Add(led);
                return true;
            }
            return false;
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
            foreach (var oldLed in Leds)
            {
                if (newLed.LedRectangle.IntersectsWith(oldLed.LedRectangle))
                    return false;
            }
            return true;
        }

        /*private void OrderNumChanged(object o, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var led in e.NewItems)
                {
                    var newLed = led as Led;
                    if (newLed == null)
                        return;
                    newLed.PropertyChanged += OnOrderNumChanged;
                }
            }
        }
        private void OnOrderNumChanged(object o, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "OrderNum")
                OrderChangedAction();// Leds = new ObservableCollection<Led>(Leds.OrderBy(s => s.OrderNum));
        }*/
    }
}
