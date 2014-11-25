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
            GameState gs1 = new GameState();
            gs1.AddPegs(3);
            gs1.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            gs1.Pegs.ElementAt(1).PushDisc(new Disc(1, "blue"));

            GameState gs2 = new GameState();
            gs2.AddPegs(3);
            gs2.Pegs.ElementAt(1).PushDisc(new Disc(2, "red"));
            gs2.Pegs.ElementAt(2).PushDisc(new Disc(1, "blue"));
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

        [Test]
        public void FindPossibilities_ReturnsCorrectList()
        {
            // Input state (horizontal):
            // 0: 32
            // 1: 
            // 2: 1
            // Expected outputs:
            // (2->0)   (0->1)   (2->1)
            // 0: 321   0: 3     0: 32 
            // 1:       1: 2     1: 1  
            // 2:       2: 1     2:   

            GameState gs = new GameState();
            gs.AddPegs(3);
            gs.Pegs.ElementAt(0).PushDisc(new Disc(3, "red"));
            gs.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            gs.Pegs.ElementAt(2).PushDisc(new Disc(1, "red"));

            GameState exp1, exp2, exp3;
            exp1 = new GameState();
            exp1.AddPegs(3);
            exp1.Pegs.ElementAt(0).PushDisc(new Disc(3, "red"));
            exp1.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            exp1.Pegs.ElementAt(0).PushDisc(new Disc(1, "red"));
            exp2 = new GameState();
            exp2.AddPegs(3);
            exp2.Pegs.ElementAt(0).PushDisc(new Disc(3, "red"));
            exp2.Pegs.ElementAt(1).PushDisc(new Disc(2, "red"));
            exp2.Pegs.ElementAt(2).PushDisc(new Disc(1, "red"));
            exp3 = new GameState();
            exp3.AddPegs(3);
            exp3.Pegs.ElementAt(0).PushDisc(new Disc(3, "red"));
            exp3.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            exp3.Pegs.ElementAt(1).PushDisc(new Disc(1, "red"));

            gs.FindPossibilities();

            Assert.IsTrue(gs.Possibilities.Count == 3);
            Assert.IsTrue(gs.Possibilities.Any(state => state.HasSamePegs(exp1)));
            Assert.IsTrue(gs.Possibilities.Any(state => state.HasSamePegs(exp2)));
            Assert.IsTrue(gs.Possibilities.Any(state => state.HasSamePegs(exp3)));
        }

        [Test]
        public void FindPossibilities_ReturnsCorrectListAlt()
        {
            // Input state (horizontal):
            // 0: 32
            // 1: 22
            // 2: 11

            GameState gs = new GameState();
            gs.AddPegs(3);
            gs.Pegs.ElementAt(0).PushDisc(new Disc(3, "red"));
            gs.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            gs.Pegs.ElementAt(1).PushDisc(new Disc(2, "red"));
            gs.Pegs.ElementAt(1).PushDisc(new Disc(2, "red"));
            gs.Pegs.ElementAt(2).PushDisc(new Disc(1, "red"));
            gs.Pegs.ElementAt(2).PushDisc(new Disc(1, "red"));


            GameState exp1, exp2, exp3, exp4;
            exp1 = new GameState();
            exp1.AddPegs(3);
            exp1.Pegs.ElementAt(0).PushDisc(new Disc(3, "red"));
            exp1.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            exp1.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            exp1.Pegs.ElementAt(1).PushDisc(new Disc(2, "red"));
            exp1.Pegs.ElementAt(2).PushDisc(new Disc(1, "red"));
            exp1.Pegs.ElementAt(2).PushDisc(new Disc(1, "red"));
            exp2 = new GameState();
            exp2.AddPegs(3);
            exp2.Pegs.ElementAt(0).PushDisc(new Disc(3, "red"));
            exp2.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            exp2.Pegs.ElementAt(0).PushDisc(new Disc(1, "red"));
            exp2.Pegs.ElementAt(1).PushDisc(new Disc(2, "red"));
            exp2.Pegs.ElementAt(1).PushDisc(new Disc(2, "red"));
            exp2.Pegs.ElementAt(2).PushDisc(new Disc(1, "red"));
            exp3 = new GameState();
            exp3.AddPegs(3);
            exp3.Pegs.ElementAt(0).PushDisc(new Disc(3, "red"));
            exp3.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            exp3.Pegs.ElementAt(0).PushDisc(new Disc(1, "red"));
            exp3.Pegs.ElementAt(1).PushDisc(new Disc(2, "red"));
            exp3.Pegs.ElementAt(1).PushDisc(new Disc(2, "red"));
            exp3.Pegs.ElementAt(2).PushDisc(new Disc(1, "red"));
            exp4 = new GameState();
            exp4.AddPegs(3);
            exp4.Pegs.ElementAt(0).PushDisc(new Disc(3, "red"));
            exp4.Pegs.ElementAt(0).PushDisc(new Disc(2, "red"));
            exp4.Pegs.ElementAt(1).PushDisc(new Disc(2, "red"));
            exp4.Pegs.ElementAt(1).PushDisc(new Disc(2, "red"));
            exp4.Pegs.ElementAt(1).PushDisc(new Disc(1, "red"));
            exp4.Pegs.ElementAt(2).PushDisc(new Disc(1, "red"));

            gs.FindPossibilities();

            Assert.IsTrue(gs.Possibilities.Count == 4);
            Assert.IsTrue(gs.Possibilities.Any(state => state.HasSamePegs(exp1)));
            Assert.IsTrue(gs.Possibilities.Any(state => state.HasSamePegs(exp2)));
            Assert.IsTrue(gs.Possibilities.Any(state => state.HasSamePegs(exp3)));
            Assert.IsTrue(gs.Possibilities.Any(state => state.HasSamePegs(exp4)));
        }

        [Test]
        public void FindPossibilities_WillNotReturnAnyFromTaboo()
        {
            GameState gs = new GameState();
            gs.AddPegs(3);
            gs.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(3, "green"));
            gs.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(2, "grey"));
            gs.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(2, "green"));
            gs.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(1, "grey"));
            gs.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(1, "green"));
            List<GameState> taboo = new List<GameState>();

            gs.FindPossibilities();
            taboo.Add(gs);
            taboo.Union(gs.Possibilities);

            for (var i = 0; i < gs.Possibilities.Count; ++i)
            {
                GameState here = gs.Possibilities.ElementAtOrDefault(i);
                here.FindPossibilities(taboo);
                for (var j = 0; j < here.Possibilities.Count; ++j)
                {
                    Assert.IsFalse(taboo.Any(state => state.HasSamePegs(here.Possibilities.ElementAtOrDefault(j))));
                }
            }
        }

        [Test]
        public void GetSequence_SimpleExampleCompletesAtAll()
        {
            GameState initial = new GameState();
            initial.AddPegs(3);
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(2, "green"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(1, "grey"));

            GameState desired = new GameState();
            desired.AddPegs(3);
            desired.Pegs.ElementAtOrDefault(1).PushDisc(new Disc(2, "green"));
            desired.Pegs.ElementAtOrDefault(1).PushDisc(new Disc(1, "grey"));

            string payoff = GameState.GetSequence(initial, desired);
            Assert.IsTrue(payoff.Length > 0);
        }

        [Test]
        //[Ignore]
        public void GetSequence_ReturnsCorrectLength5()
        {
            GameState initial = new GameState();
            initial.AddPegs(3);
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(3, "green"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(2, "yellow"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(2, "green"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(1, "yellow"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(1, "green"));

            GameState desired = new GameState();
            desired.AddPegs(3);
            desired.Pegs.ElementAtOrDefault(1).PushDisc(new Disc(2, "yellow"));
            desired.Pegs.ElementAtOrDefault(1).PushDisc(new Disc(1, "yellow"));
            desired.Pegs.ElementAtOrDefault(2).PushDisc(new Disc(3, "green"));
            desired.Pegs.ElementAtOrDefault(2).PushDisc(new Disc(2, "green"));
            desired.Pegs.ElementAtOrDefault(2).PushDisc(new Disc(1, "green"));

            string payoff = GameState.GetSequence(initial, desired);
            Assert.IsTrue(payoff.Length > 0);
        }

        [Test]
        //[Ignore]
        public void GetSequence_ReturnsCorrectLength6()
        {
            GameState initial = new GameState();
            initial.AddPegs(3);
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(3, "yellow"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(3, "green"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(2, "yellow"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(2, "green"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(1, "yellow"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(1, "green"));

            GameState desired = new GameState();
            desired.AddPegs(3);
            desired.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(3, "yellow"));
            desired.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(2, "yellow"));
            desired.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(1, "yellow"));
            desired.Pegs.ElementAtOrDefault(1).PushDisc(new Disc(3, "green"));
            desired.Pegs.ElementAtOrDefault(1).PushDisc(new Disc(2, "green"));
            desired.Pegs.ElementAtOrDefault(1).PushDisc(new Disc(1, "green"));

            string payoff = GameState.GetSequence(initial, desired);
            Assert.IsTrue(payoff.Length > 0);
        }

        [Test]
        public void GameState_CtorWithParamWillPointToCreatorState()
        {
            GameState gs = new GameState();
            gs.AddPegs(3);
            gs.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(3, "green"));
            gs.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(2, "grey"));
            gs.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(2, "green"));
            gs.Pegs.ElementAtOrDefault(1).PushDisc(new Disc(2, "red"));
            gs.Pegs.ElementAtOrDefault(2).PushDisc(new Disc(1, "grey"));

            gs.FindPossibilities();
            gs.Possibilities.ElementAtOrDefault(0).FindPossibilities();

            Assert.IsTrue(gs.Possibilities.ElementAtOrDefault(0).PreviousState == gs);
            Assert.IsTrue(gs.Possibilities.ElementAtOrDefault(0).Possibilities.ElementAtOrDefault(0).PreviousState == gs.Possibilities.ElementAtOrDefault(0));
        }
    }
}
