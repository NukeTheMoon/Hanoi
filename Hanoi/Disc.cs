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
            if (size < 1)
            {
                throw new Exception("Disc size cannot be smaller than 1!");
            }
            Size = size;
            Color = color;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Disc;
 
            if (Object.ReferenceEquals(obj, null))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.Color == item.Color && this.Size == item.Size) 
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            if (Object.ReferenceEquals(this, null))
            {
                return 0;
            }
            return Size ^ Color.GetHashCode();
        } 
    }
}
