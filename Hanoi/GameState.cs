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
        public GameState PreviousPossibility;

        public List<Peg> Pegs;
        public string Transition { get; set; }
       
        public GameState()
        {
            Pegs = new List<Peg>();
            Possibilities = new List<GameState>();
            PreviousState = null;
            PreviousPossibility = null;
            Transition = "";
        }

        public GameState(GameState previous)
        {
            Pegs = previous.Pegs.Select(i => i.Clone()).ToList();
            PreviousState = previous;
            PreviousPossibility = null;
            Possibilities = new List<GameState>();
            Transition = "";
        }

        public GameState(GameState previous, GameState previousPossibility)
        {
            Pegs = previous.Pegs.Select(i => i.Clone()).ToList();
            PreviousState = previous;
            PreviousPossibility = previousPossibility;
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
                            GameState possibleState = new GameState(this, possibilities.ElementAtOrDefault(possibilities.Count - 1));
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
                            GameState possibleState = new GameState(this, possibilities.ElementAtOrDefault(possibilities.Count - 1));
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

        private static GameStateMatch FindMatch(GameState topMarker, GameState bottomMarker)
        {
            List<GameState> topTaboo = new List<GameState>();
            List<GameState> bottomTaboo = new List<GameState>();

            topTaboo.Add(topMarker);
            bottomTaboo.Add(bottomMarker);

            //initial movement down and to rightmost edge of possibility group
            topTaboo = topTaboo.Union(topMarker.FindPossibilities(topTaboo)).ToList();
            GameState topRightmost = topMarker.Possibilities.ElementAtOrDefault(topMarker.Possibilities.Count - 1);
            topMarker = topRightmost;

            //bottom marker and it's rightmost initially stay at desired state and will always lag
            //behind by one possibility group row
            GameState bottomRightmost = bottomMarker;

            //search loop
            while (!topMarker.HasSamePegs(bottomMarker))
            {
                MoveInstruction topInstruction = MarkerMovement(topMarker, topRightmost, topTaboo);
                topMarker = topInstruction.MainMarker;
                topRightmost = topInstruction.RightmostMarker;
                topTaboo = topInstruction.Taboo;
                //if (topInstruction.TimeToMoveBottom)
                //{
                //    //bottom marker is not at left edge of it's possibility group row?
                //    if (bottomMarker.PreviousPossibility != null || (bottomMarker.PreviousState != null && bottomMarker.PreviousState.PreviousPossibility != null))
                //    {
                //        //top marker returns to the rightmost edge of the possibility group row it just descended from
                //        topMarker = topMarker.PreviousState;
                //    }
                //    MoveInstruction bottomInstruction = MarkerMovement(bottomMarker, bottomRightmost, bottomTaboo);
                //    bottomMarker = bottomInstruction.MainMarker;
                //    bottomRightmost = bottomInstruction.RightmostMarker;
                //    bottomTaboo = bottomInstruction.Taboo;
                //}
            }

            return new GameStateMatch(topMarker, bottomMarker);
        }

        private static MoveInstruction MarkerMovement(GameState marker, GameState rightmost, List<GameState> taboo)
        {
            //is main marker on leftmost edge of possibility group?
            if (marker.PreviousPossibility == null)
            {
                //can main marker move up and left?
                if (marker.PreviousState != null && marker.PreviousState.PreviousPossibility != null)
                {
                    //move main marker up and left
                    marker = marker.PreviousState.PreviousPossibility;
                    //can main marker move down?
                    if (marker.Possibilities.Count > 0)
                    {
                        //move main marker down to rightmost edge of possibility group below
                        marker = marker.Possibilities.ElementAtOrDefault(marker.Possibilities.Count - 1);
                    }
                }
                //is main marker either blocked from moving upward, or is it's parent state on leftmost edge of possibility group?
                else if (marker.PreviousState == null || marker.PreviousState.PreviousPossibility == null)
                {
                    //loop until rightmost marker can move down
                    while (rightmost.Possibilities.Count < 1)
                    {
                        //is rightmost marker on leftmost edge of possibility group?
                        if (rightmost.PreviousPossibility == null)
                        {
                            //can rightmost marker move up and left?
                            if (rightmost.PreviousState != null && rightmost.PreviousState.PreviousPossibility != null)
                            {
                                //move rightmost marker up and left
                                rightmost = rightmost.PreviousState.PreviousPossibility;
                            }
                        }
                        else
                        {
                            //move rightmost marker left
                            rightmost = rightmost.PreviousPossibility;
                        }
                    }
                    //move rightmost marker down to rightmost edge of possibility group below
                    rightmost = rightmost.Possibilities.ElementAtOrDefault(rightmost.Possibilities.Count - 1);
                    //move main marker to rightmost marker position
                    marker = rightmost;
                    //mark this position as visited
                    taboo.Add(marker);
                    //expand possibility group for this state
                    marker.FindPossibilities(taboo);
                    //if this is top marker then it's time to move bottom marker
                    return new MoveInstruction(marker, rightmost, taboo, true);
                }
            }
            else
            {
                //mark this position as visited
                taboo.Add(marker);
                //expand possibility group for this state
                marker.FindPossibilities(taboo);
                //move main marker left
                marker = marker.PreviousPossibility;
            }
            //it is not yet time to move bottom marker
            return new MoveInstruction(marker, rightmost, taboo, false);
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