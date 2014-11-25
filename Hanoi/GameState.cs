using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        int tier;
       
        public GameState()
        {
            Pegs = new List<Peg>();
            Possibilities = new List<GameState>();
            PreviousState = null;
            Transition = "";
            tier = 0;
        }

        public GameState(GameState previous)
        {
            Pegs = previous.Pegs.Select(i => i.Clone()).ToList();
            PreviousState = previous;
            tier = previous.tier + 1;
            Possibilities = new List<GameState>();
            Transition = "";
        }

        public GameState(GameState previous, bool clean)
        {
            Pegs = previous.Pegs.Select(i => i.Clone()).ToList();
            if (clean)
            {
                PreviousState = null;
                tier = 0;
            }
            else
            {
                            PreviousState = previous;
            tier = previous.tier + 1;
            }
            Possibilities = new List<GameState>();
            Transition = "";
        }

        public void AddPegs(int amount)
        {
            for (var i = 0; i < amount; ++i)
            {
                Pegs.Add(new Peg());
            }
        }

        public List<Disc> GetAllDiscs()
        {
            List<Disc> allDiscs = new List<Disc>();
            for (var i = 0; i < this.Pegs.Count; ++i)
            {
                allDiscs.AddRange(this.Pegs.ElementAtOrDefault(i).GetDiscList());
            }
            return allDiscs;
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
        public List<GameState> FindPossibilities()
        {
            List<GameState> possibilities = new List<GameState>();
            int pegCount = this.Pegs.Count;

            for (var i = 0; i < pegCount; ++i)
            {
                if (this.Pegs.ElementAtOrDefault(i).GetDiscCount() > 0)
                {
                    for (var j = 0; j < pegCount; ++j)
                    {
                        if (j != i)
                        {
                            GameState possibleState = new GameState(this);
                            if (possibleState.Pegs.ElementAtOrDefault(i).MoveDisc(possibleState.Pegs.ElementAtOrDefault(j)))
                            {

                                    possibilities.Add(possibleState);
                            }
                        }
                    }
                }
            }
            return this.Possibilities = possibilities;
        }

        public List<GameState> FindPossibilities(List<GameState> taboo)
        {
            List<GameState> possibilities = new List<GameState>();
            int pegCount = this.Pegs.Count;

            for (var i = 0; i < pegCount; ++i)
            {
                if (this.Pegs.ElementAtOrDefault(i).GetDiscCount() > 0)
                {
                    for (var j = 0; j < pegCount; ++j)
                    {
                        if (j != i)
                        {
                            GameState possibleState = new GameState(this);
                            if (possibleState.Pegs.ElementAt(i).MoveDisc(possibleState.Pegs.ElementAt(j)))
                            {
                                if (!taboo.Any(state => state.HasSamePegs(possibleState)))
                                {
                                    possibilities.Add(possibleState);
                                }
                            }
                        }
                    }
                }
            }
            return this.Possibilities = possibilities;
        }

        public bool HasSamePegs(GameState other)
        {
            if (!this.SameParametersAs(other))
            {
                return false;
            }
            else 
            {
                for (var i = 0; i < other.Pegs.Count; ++i)
                {
                    Peg thisCurrentPeg = this.Pegs.ElementAtOrDefault(i);
                    Peg otherCurrentPeg = other.Pegs.ElementAtOrDefault(i);
                    if (!thisCurrentPeg.Equals(otherCurrentPeg)) 
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool AllPegsLegal()
        {
            for (var i = 0; i < this.Pegs.Count; ++i )
            {
                if (!this.Pegs.ElementAtOrDefault(i).IsLegal())
                {
                    return false;
                }
            }
            return true;
        }

        public static string GetSequence(GameState initialState, GameState desiredState)
        {
            if (!initialState.SameParametersAs(desiredState))
            {
                throw new Exception("Supplied gamestates have different parameters!");
            }
            else
            {
                StringBuilder sequence = new StringBuilder();

                if (initialState.HasSamePegs(desiredState))
                {
                    //no sequence
                    return "error: no sequence";
                }

                if (! (initialState.AllPegsLegal() && initialState.AllPegsLegal()))
                {
                    //illegal
                    return "error: illegal game state";
                }

                GameStateMatch match = FindMatch(initialState, desiredState);

                Stack<GameState> topStack = new Stack<GameState>();
                while (match.LowestTopState != null)
                {
                    topStack.Push(match.LowestTopState);
                    match.LowestTopState = match.LowestTopState.PreviousState;
                }
                while (topStack.Count > 1)
                {
                    GameState gs1 = topStack.Pop();
                    GameState gs2 = topStack.Peek();
                    sequence.AppendLine(GameState.TranscribeTransition(gs1, gs2));
                }

                while (match.HighestBottomState.PreviousState != null)
                {
                    GameState gs1 = match.HighestBottomState;
                    GameState gs2 = match.HighestBottomState.PreviousState;
                    sequence.AppendLine(GameState.TranscribeTransition(gs1, gs2));
                    match.HighestBottomState = match.HighestBottomState.PreviousState;
                }
                return sequence.ToString();
            }
        }

        private static GameStateMatch FindMatch(GameState origin, GameState desired)
        {
            GameState topMarker = new GameState(origin, true);
            GameState bottomMarker = new GameState(desired, true);

            if (topMarker.HasSamePegs(bottomMarker))
            {
                return new GameStateMatch(topMarker, bottomMarker);
            }

            List<GameState> topTaboo = new List<GameState>();
            List<GameState> bottomTaboo = new List<GameState>();

            topTaboo.Add(topMarker);
            topMarker.FindPossibilities(topTaboo);
            topTaboo = topMarker.GetPossibilitiesUnion(topTaboo);
            bottomTaboo.Add(bottomMarker);
            bottomMarker.FindPossibilities(bottomTaboo);
            bottomTaboo = bottomMarker.GetPossibilitiesUnion(bottomTaboo);

            int searchTier = 0;

            while (true)
            {
                List<GameState> currentTierTop = topTaboo.Where(state => state.tier == searchTier).ToList();
                List<GameState> currentTierBottom = bottomTaboo.Where(state => state.tier == searchTier).ToList();
                for (var i=0; i<currentTierTop.Count; ++i)
                {
                    topMarker = currentTierTop.ElementAtOrDefault(i);
                    for (var j = 0; j < currentTierBottom.Count; ++j)
                    {
                        bottomMarker = currentTierBottom.ElementAtOrDefault(j);
                        if (topMarker.HasSamePegs(bottomMarker))
                        {
                            return new GameStateMatch(topMarker, bottomMarker);
                        }
                        else

                        {
                            bottomMarker.FindPossibilities(bottomTaboo);
                            bottomTaboo = bottomMarker.GetPossibilitiesUnion(bottomTaboo);
                        }
                    }
                    topMarker.FindPossibilities(topTaboo);
                    topTaboo = topMarker.GetPossibilitiesUnion(topTaboo);
                }
                ++searchTier;
            }
        }

        private List<GameState> GetPossibilitiesUnion(List<GameState> taboo)
        {
            foreach (var possibility in this.Possibilities)
            {
                if (!taboo.Contains(possibility))
                {
                    taboo.Add(possibility);
                }
            }
            return taboo;
        }

        public override bool Equals(object obj)
        {

            if (Object.ReferenceEquals(obj, null))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            var item = obj as GameState
                ;
            if (this.HasSamePegs(item))
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Pegs.GetHashCode();
        }
    }
}