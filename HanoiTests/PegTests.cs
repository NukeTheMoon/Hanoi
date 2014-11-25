using Hanoi;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HanoiTests
{
    [TestFixture]
    public class PegTests
    {
        [Test]
        public void PegInitializesWithDefaultCtor()
        {
            Peg p = new Peg();
            Assert.IsNotNull(p);
        }

        [Test]
        public void PegIsLegal_IllegalStack_ReturnsFalse()
        {
            Peg p = new Peg();
            p.Stack.Push(new Disc(2, "blue"));
            p.Stack.Push(new Disc(1, "blue"));
            p.Stack.Push(new Disc(1, "blue"));
            p.Stack.Push(new Disc(3, "blue"));
            Assert.IsFalse(p.IsLegal());
        }

        [Test]
        public void PegIsLegal_LegalStack_ReturnsTrue()
        {
            Peg p = new Peg();
            p.Stack.Push(new Disc(3, "blue"));
            p.Stack.Push(new Disc(2, "blue"));
            p.Stack.Push(new Disc(2, "blue"));
            p.Stack.Push(new Disc(1, "blue"));
            Assert.IsTrue(p.IsLegal());
        }

        [Test]
        public void PegClone_CloneHasSameStack()
        {
            Peg p = new Peg();
            Disc disc1 = new Disc(3, "red");
            Disc disc2 = new Disc(2, "yellow");
            p.Stack.Push(disc1);
            p.Stack.Push(disc2);
            Peg clone = p.Clone();
            p.Stack.Clear();
            disc1 = disc2 = null;
            Assert.IsTrue(clone.Stack.ElementAtOrDefault(0).Size == 2 && clone.Stack.ElementAtOrDefault(0).Color == "yellow");
            Assert.IsTrue(clone.Stack.ElementAtOrDefault(1).Size == 3 && clone.Stack.ElementAtOrDefault(1).Color == "red");
        }

        [Test]
        public void PegPushDisc_IllegalMove_ReturnsFalseAndRevertsToInitialStack()
        {
            Peg p = new Peg();
            p.PushDisc(new Disc(2, "orange"));
            Assert.IsFalse(p.PushDisc(new Disc(3, "orange")));
            Assert.IsTrue(p.GetDiscCount() == 1);
            Assert.IsTrue(p.Stack.Peek().Size == 2 && p.Stack.Peek().Color == "orange");
        }

        [Test]
        public void PegPushDisc_LegalMove_ReturnsTrue()
        {
            Peg p = new Peg();
            p.PushDisc(new Disc(2, "orange"));
            Assert.IsTrue(p.PushDisc(new Disc(1, "yellow")));
        }

        [Test]
        public void PegPushDisc_LegalMove_StackIsModified()
        {
            Peg p = new Peg();
            p.PushDisc(new Disc(2, "orange"));
            p.PushDisc(new Disc(1, "yellow"));
            Assert.IsTrue(p.GetDiscCount() == 2 && 
                p.Stack.Peek().Size == 1 
                && p.Stack.Peek().Color == "yellow");
        }

        [Test]
        public void PegMoveDisc_LegalMove_ReturnsTrue()
        {
            Peg origin = new Peg();
            origin.PushDisc(new Disc(2, "orange"));
            origin.PushDisc(new Disc(1, "yellow"));
            Peg dest = new Peg();
            Assert.IsTrue(origin.MoveDisc(dest));
        }

        [Test]
        public void PegMoveDisc_IllegalMove_ReturnsFalse()
        {
            Peg origin = new Peg();
            origin.PushDisc(new Disc(3, "orange"));
            origin.PushDisc(new Disc(2, "yellow"));
            Peg dest = new Peg();
            dest.PushDisc(new Disc(1, "yellow"));
            Assert.IsFalse(origin.MoveDisc(dest));
        }

        [Test]
        public void PegMoveDisc_IllegalMove_OriginUnaltered()
        {
            Peg origin = new Peg();
            origin.PushDisc(new Disc(3, "orange"));
            origin.PushDisc(new Disc(2, "yellow"));
            Peg originOriginal = origin.Clone();
            Peg dest = new Peg();
            dest.PushDisc(new Disc(1, "yellow"));
            origin.MoveDisc(dest);
            Assert.IsTrue(origin.Equals(originOriginal));
        }

        [Test]
        public void PegMoveDisc_IllegalMove_DestinationUnaltered()
        {
            Peg origin = new Peg();
            origin.PushDisc(new Disc(3, "orange"));
            origin.PushDisc(new Disc(2, "yellow"));
            Peg dest = new Peg();
            dest.PushDisc(new Disc(1, "yellow"));
            Peg destOriginal = dest.Clone();
            origin.MoveDisc(dest);
            Assert.IsTrue(dest.Equals(destOriginal));
        }
    }
}
