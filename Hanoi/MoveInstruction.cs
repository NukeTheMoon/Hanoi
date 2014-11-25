using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanoi
{
    public class MoveInstruction
    {
        public GameState MainMarker;
        public GameState RightmostMarker;
        public List<GameState> Taboo;
        public bool TimeToMoveBottom;

        public MoveInstruction(GameState mainMarker, GameState rightmostMarker, List<GameState> taboo, bool timeToMoveBottom)
        {
            MainMarker = mainMarker;
            RightmostMarker = rightmostMarker;
            Taboo = taboo;
            TimeToMoveBottom = timeToMoveBottom;
        }
    }
}
