using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public interface IMapOfLeds
    {
        List<Led> Leds { get; }
        Size MapSize { get; }
    }
}
