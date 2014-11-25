using System.Linq;
using Hanoi;
using NUnit.Framework;

namespace HanoiTests
{
    [TestFixture]
    public class PegTests
    {
        private Peg _sourcePeg;
        private Peg _destinationPeg;

        [SetUp]
        public void SetUp()
        {
            _sourcePeg = new Peg();
             _destinationPeg = new Peg();
        }

        private void PushDisksOnSourcePeg(string diskColor, params int[] diskSizes)
        {
            foreach (var diskSize in diskSizes)
            {
                PushDiscOnSourcePeg(diskSize, diskColor);
            }            
        }
        private void PushDiscOnSourcePeg(int diskSize, string diskColor)
        {
            PushDiskOnPeg(_sourcePeg, diskSize, diskColor);
        }
        private void PushDiscOnDestinationPeg(int diskSize, string diskColor)
        {
            PushDiskOnPeg(_destinationPeg, diskSize, diskColor);
        }
        private void PushDiskOnPeg(Peg peg, int diskSize, string diskColor)
        {
            peg.TryPushDisc(new Disc(diskSize, diskColor));
        }
        private bool TryPushDiscOnSourcePeg(int diskSize, string diskColor)
        {
            return _sourcePeg.TryPushDisc(new Disc(diskSize, diskColor));
        }
        private bool MoveDiscBetweenPegs()
        {
            return _sourcePeg.MoveDisc(_destinationPeg);
        }
        private void AssertFirstDiskOnTopIs(int size, string color)
        {
            Assert.AreEqual(size, _sourcePeg.GetDiscList().First().Size);
            Assert.AreEqual(color, _sourcePeg.GetDiscList().First().Color);
        }

        [Test]
        public void PegInitializesWithDefaultCtor()
        {
            Assert.IsNotNull(_sourcePeg);
        }

        [Test]
        public void PegIsLegal_IllegalStack_ReturnsFalse()
        {
            PushDisksOnSourcePeg("blue", 2, 1, 1, 3);
            Assert.IsFalse(_sourcePeg.IsInLegalState());
        }

        [Test]
        public void PegIsLegal_LegalStack_ReturnsTrue()
        {
            PushDisksOnSourcePeg("blue", 3, 2, 2, 1);
            Assert.IsTrue(_sourcePeg.IsInLegalState());
        }

        [Test]
        public void PegClone_CloneHasSameStack()
        {
            PushDiscOnSourcePeg(3, "red");
            PushDiscOnSourcePeg(2, "yellow");
            Peg clone = _sourcePeg.Clone() as Peg;
            CollectionAssert.AreEqual(_sourcePeg.GetDiscList(), clone.GetDiscList());            
        }

        [Test]
        public void PegPushDisc_IllegalMove_ReturnsFalseAndRevertsToInitialStack()
        {
            PushDiscOnSourcePeg(2, "orange");
            Assert.IsFalse(TryPushDiscOnSourcePeg(3, "orange"));
            Assert.AreEqual(1, _sourcePeg.DiscCount);
            AssertFirstDiskOnTopIs(2, "orange");
        }

        [Test]
        public void PegPushDisc_LegalMove_ReturnsTrue()
        {
            PushDiscOnSourcePeg(2, "orange");
            Assert.IsTrue(TryPushDiscOnSourcePeg(1, "yellow"));
        }

        [Test]
        public void PegPushDisc_LegalMove_StackIsModified()
        {
            PushDiscOnSourcePeg(2, "orange");
            PushDiscOnSourcePeg(1, "yellow");
            Assert.AreEqual(2, _sourcePeg.DiscCount);
            AssertFirstDiskOnTopIs(1, "yellow");
        }

        [Test]
        public void PegMoveDisc_LegalMove_ReturnsTrue()
        {
            PushDiscOnSourcePeg(2, "orange");
            PushDiscOnSourcePeg(1, "yellow");
            Assert.IsTrue(_sourcePeg.MoveDisc(_destinationPeg));
        }

        [Test]
        public void PegMoveDisc_IllegalMove_ReturnsFalse()
        {
            PushDiscOnSourcePeg(3, "orange");
            PushDiscOnSourcePeg(2, "yellow");
            PushDiscOnDestinationPeg(1, "yellow");
            Assert.IsFalse(MoveDiscBetweenPegs());
        }

        [Test]
        public void PegMoveDisc_IllegalMove_OriginUnaltered()
        {
            PushDiscOnSourcePeg(3, "orange");
            PushDiscOnSourcePeg(2, "yellow");
            Peg originOriginal = _sourcePeg.Clone() as Peg;
            PushDiscOnDestinationPeg(1, "yellow");
            MoveDiscBetweenPegs();
            Assert.AreEqual(_sourcePeg, originOriginal);
        }

        [Test]
        public void PegMoveDisc_IllegalMove_DestinationUnaltered()
        {
            PushDiscOnSourcePeg(3, "orange");
            PushDiscOnSourcePeg(2, "yellow");
            PushDiscOnDestinationPeg(1, "yellow");
            Peg destOriginal = _destinationPeg.Clone() as Peg;
            MoveDiscBetweenPegs();
            Assert.AreNotEqual(_sourcePeg, destOriginal);
        }

        [Test]
        public void PegGetDiscList_ReturnsElementsInStackOrder()
        {
            PushDiscOnSourcePeg(3, "orange");
            PushDiscOnSourcePeg(2, "yellow");
            PushDiscOnSourcePeg(1, "orange");
            var list = _sourcePeg.GetDiscList();
            CollectionAssert.AreEqual(_sourcePeg.GetDiscList(), list);
        }
    }
}
