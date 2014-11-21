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

        private List<Peg> _pegs;
        public string Transition { get; set; }
       
        public GameState()
        {
            _pegs = new List<Peg>();
            Possibilities = findPossibilites();
            PreviousState = null;
            Transition = "";
        }


        public GameState(GameState previous)
        {
            PreviousState = previous;
            Transition = TranscribeTransition(PreviousState, this);
        }

        public void AddPegs(int amount)
        {
            for (var i = 0; i < amount; ++i)
            {
                _pegs.Add(new Peg());
            }
        }

        private string TranscribeTransition(GameState initial, GameState result)
        {
            throw new NotImplementedException();
        }
        private List<GameState> findPossibilites()
        {
            throw new NotImplementedException();
        }
    }
}