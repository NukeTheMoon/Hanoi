using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanoi
{
    public class GameStateMatch
    {
        public GameState LowestTopState;
        public GameState HighestBottomState;

        public GameStateMatch(GameState lowestTop, GameState highestBottom)
        {
            LowestTopState = lowestTop;
            HighestBottomState = highestBottom;
        }
    }
}
