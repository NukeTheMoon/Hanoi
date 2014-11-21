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
    }
}
