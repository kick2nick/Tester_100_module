using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public interface IMapOfLeds
    {
        ObservableCollection<Led> Leds { get; }
        Size MapSize { get; }
    }
}
