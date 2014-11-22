using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanoi
{
    public class Peg
    {
        public Stack<Disc> Stack;
        public Peg()
        {
            Stack = new Stack<Disc>();
        }
        public int GetDiscCount()
        {
            return Stack.Count();
        }

        public bool PushDisc(Disc disc)
        {
            Peg originalState = this.Clone();
            Stack.Push(disc);
            if (this.IsLegal())
            {
                return true;
            }
            this.Stack = originalState.Stack;
            return false;
        }

        public Peg Clone()
        {
            Peg clone = new Peg();
            clone.Stack = new Stack<Disc>(new Stack<Disc>(this.Stack));
            return clone;
        }

        public List<Disc> GetDiscList()
        {
            List<Disc> list = new List<Disc>();
            for (var i = 0; i < this.Stack.Count; ++i)
            {
                list.Add(this.Stack.ElementAtOrDefault(i));
            }
            return list;
        }

        public bool IsLegal()
        {
            if (Stack.Count > 1)
            {
                for (var i = 1; i < Stack.Count; ++i)
                {
                    if (Stack.ElementAtOrDefault(i-1).Size > Stack.ElementAtOrDefault(i).Size)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Peg;
            if (item == null)
            {
                return false;
            }
            if (!this.Stack.SequenceEqual(item.Stack))
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return Stack.GetHashCode();
        }
    }
}
