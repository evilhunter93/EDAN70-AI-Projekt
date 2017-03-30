using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    class GameBoard
    {
        const int UNSPECIFIED = -1;
        private int size;
        private StoneStack[,] stacks;

        public GameBoard(int size)
        {
            this.size = size;
            stacks = new StoneStack[size, size];
        }

        public bool PlaceStone(int x, int y, Stone stone)
        {
            return false;
        }

        public bool PickUpStack(int x, int y, int amount = UNSPECIFIED)
        {
            if (amount == UNSPECIFIED)
                if (stacks[x, y].Count > 1)
                    throw new IllegalMoveException("Unspecified number of stones but stack has more than one");
            return false;
        }
    }
}
