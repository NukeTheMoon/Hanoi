using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanoi
{
    public class Disc
    {
        public int Size { get; private set; }
        public string Color { get; private set; }

        public Disc(int size, string color)
        {
            Size = size;
            Color = color;
        }
    }
}
