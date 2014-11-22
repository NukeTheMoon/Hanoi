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

        public override bool Equals(object obj)
        {
            var item = obj as Disc;
 
            if (item == null)
            {
                return false;
            }

            if (this.Color == item.Color && this.Size == item.Size) 
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Size ^ Color.GetHashCode();
        } 
    }
}
