using Hanoi;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HanoiTests
{
    [TestFixture]
    public class GameStateTests
    {
        [Test]
        public void GameStateInitializesWithDefaultCtor()
        {
            GameState gs = new GameState();
            Assert.IsNotNull(gs);
        }

        [Test]
        public void SameParametersAs_BothEmpty_ReturnTrue()
        {
            GameState gs1, gs2;
            gs1 = new GameState();
            gs2 = new GameState();
            Assert.IsTrue(gs1.SameParametersAs(gs2));
        }

        [Test]
        public void SameParametersAs_DifferentPegCount_ReturnFalse()
        {
            GameState gs1, gs2;
            gs1 = new GameState();
            gs2 = new GameState();
            gs1.AddPegs(3);
            gs2.AddPegs(4);
            Assert.IsFalse(gs1.SameParametersAs(gs2));
        }

        [Test]
        public void SameParametersAs_SamePegCount_ReturnTrue()
        {
            GameState gs1, gs2;
            gs1 = new GameState();
            gs2 = new GameState();
            gs1.AddPegs(5);
            gs2.AddPegs(5);
            Assert.IsTrue(gs1.SameParametersAs(gs2));
        }

        [Test]
        public void SameParametersAs_DifferentDiscsCount_ReturnsFalse()
        {
            GameState gs1, gs2;
            gs1 = new GameState();
            gs2 = new GameState();
            gs1.AddPegs(1);
            gs2.AddPegs(1);
            gs1.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            gs1.Pegs.ElementAt(0).PushDisc(new Disc(1, "blue"));
            gs2.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            Assert.IsFalse(gs1.SameParametersAs(gs2));
        }

        [Test]
        public void SameParametersAs_DifferentDiscColors_ReturnsFalse()
        {
            GameState gs1, gs2;
            gs1 = new GameState();
            gs2 = new GameState();
            gs1.AddPegs(1);
            gs2.AddPegs(1);
            gs1.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            gs1.Pegs.ElementAt(0).PushDisc(new Disc(1, "blue"));
            gs2.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            gs2.Pegs.ElementAt(0).PushDisc(new Disc(1, "red"));
            Assert.IsFalse(gs1.SameParametersAs(gs2));
        }

        [Test]
        public void SameParametersAs_SameDiscsSameOrder_ReturnsTrue()
        {
            GameState gs1, gs2;
            gs1 = new GameState();
            gs2 = new GameState();
            gs1.AddPegs(1);
            gs2.AddPegs(1);
            gs1.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            gs1.Pegs.ElementAt(0).PushDisc(new Disc(1, "blue"));
            gs2.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            gs2.Pegs.ElementAt(0).PushDisc(new Disc(1, "blue"));
            Assert.IsTrue(gs1.SameParametersAs(gs2));
        }

        [Test]
        public void SameParametersAs_SameDiscsDifferentOrder_ReturnsTrue()
        {
            GameState gs1, gs2;
            gs1 = new GameState();
            gs2 = new GameState();
            gs1.AddPegs(2);
            gs2.AddPegs(2);
            gs1.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            gs1.Pegs.ElementAt(1).PushDisc(new Disc(1, "blue"));
            gs2.Pegs.ElementAt(1).PushDisc(new Disc(2, "red"));
            gs2.Pegs.ElementAt(0).PushDisc(new Disc(1, "blue"));
            Assert.IsTrue(gs1.SameParametersAs(gs2));
        }

        [Test]
        public void TranscribeTransition_LegalMove_ReturnsValid()
        {
            GameState gs1, gs2;

            gs1 = new GameState();
            gs1.AddPegs(3);
            gs1.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            gs1.Pegs.ElementAt(0).PushDisc(new Disc(1, "blue"));
            gs1.Pegs.ElementAt(2).PushDisc(new Disc(3, "red"));
            gs1.Pegs.ElementAt(2).PushDisc(new Disc(2, "blue"));
            gs1.Pegs.ElementAt(2).PushDisc(new Disc(1, "red"));

            gs2 = new GameState();
            gs2.AddPegs(3);
            gs2.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            gs2.Pegs.ElementAt(0).PushDisc(new Disc(1, "blue"));
            gs2.Pegs.ElementAt(0).PushDisc(new Disc(1, "red"));
            gs2.Pegs.ElementAt(2).PushDisc(new Disc(3, "red"));
            gs2.Pegs.ElementAt(2).PushDisc(new Disc(2, "blue"));

            Assert.AreEqual(GameState.TranscribeTransition(gs1, gs2), "3 1");
        }

        [Test]
        public void TranscribeTransition_NoTransition_ThrowsException()
        {
            GameState gs1, gs2;

            gs1 = new GameState();
            gs1.AddPegs(3);
            gs1.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            gs2 = new GameState();
            gs2.AddPegs(3);
            gs1.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            Assert.Throws<Exception>(delegate { GameState.TranscribeTransition(gs1, gs2); });
        }
    }
}
