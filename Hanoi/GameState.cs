using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanoi
{
    public class GameState
    {
        public GameState PreviousState;
        public List<GameState> Possibilities;

        public List<Peg> Pegs;
        public string Transition { get; set; }
       
        public GameState()
        {
            Pegs = new List<Peg>();
            Possibilities = new List<GameState>();
            PreviousState = null;
            Transition = "";
        }

        public GameState(GameState previous, List<Peg> pegs)
        {
            Pegs = pegs.Select(i => i.Clone()).ToList();
            PreviousState = previous;
            Possibilities = new List<GameState>();
            Transition = TranscribeTransition(PreviousState, this);
        }

        public void AddPegs(int amount)
        {
            for (var i = 0; i < amount; ++i)
            {
                Pegs.Add(new Peg());
            }
        }

        public bool SameParametersAs(GameState state)
        {
            int pegCount = this.Pegs.Count;
            if (pegCount == state.Pegs.Count)
            {
                if (this.Pegs.Count == 0 || state.Pegs.Count == 0)
                {
                    return true;
                }
                List<Disc> thisStateAllDiscs = new List<Disc>();
                List<Disc> otherStateAllDiscs = new List<Disc>();

                for (var i = 0; i < pegCount; ++i)
                {
                    Peg thisCurrentPeg = this.Pegs.ElementAtOrDefault(i);
                    thisStateAllDiscs.AddRange(thisCurrentPeg.GetDiscList());
                    Peg otherCurrentPeg = state.Pegs.ElementAtOrDefault(i);
                    otherStateAllDiscs.AddRange(otherCurrentPeg.GetDiscList());
                }

                return ArePermutations(thisStateAllDiscs, otherStateAllDiscs);
            }
            return false;
        }

        private bool ArePermutations(ICollection<Disc> list1, ICollection<Disc> list2)
        {
            return list1.OrderBy(x => x.Size).ThenBy(x => x.Color).ToList().
                SequenceEqual(list2.OrderBy(x => x.Size).ThenBy(x => x.Color).ToList());
        }

        public static string TranscribeTransition(GameState initial, GameState result)
        {
            StringBuilder transition = new StringBuilder();
            int startPegNo = 0, destinationPegNo = 0;
            if (initial.SameParametersAs(result))
            {
                for (var i=0; i < initial.Pegs.Count; ++i)
                {
                    //we're adding +1 because the verification software counts pegs from 1
                    if (initial.Pegs.ElementAt(i).GetDiscCount() > result.Pegs.ElementAt(i).GetDiscCount())
                    {
                        startPegNo = i + 1;
                    }
                    else if (initial.Pegs.ElementAt(i).GetDiscCount() < result.Pegs.ElementAt(i).GetDiscCount())
                    {
                        destinationPegNo = i + 1;
                    }
                }
                if (startPegNo != 0 && destinationPegNo != 0)
                {
                    transition.Append(startPegNo);
                    transition.Append(' ');
                    transition.Append(destinationPegNo);
                    return transition.ToString();
                }
                throw new Exception("No transition between these gamestates!");
            }
            throw new Exception("The provided gamestates have conflicting parameters!");
        }
        private List<GameState> findPossibilites()
        {
            throw new NotImplementedException();
        }
    }
}