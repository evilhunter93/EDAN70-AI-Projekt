using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Utilities;

namespace Tak.Game
{
    public class GameBoard
    {
        public const int UNSPECIFIED = -1;
        private int size;
        private StoneStack[,] stacks;

        public StoneStack[,] Stacks
        {
            get
            {
                StoneStack[,] tempStacks = new StoneStack[size, size];
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        tempStacks[x, y] = ObjectExtensions.Copy(stacks[x, y]);
                    }
                }
                return tempStacks;
            }
            set { stacks = value; }
        }

        public GameBoard(int size)
        {
            this.size = size;
            stacks = new StoneStack[size, size];
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    stacks[x, y] = new StoneStack();
        }

        public bool PlaceStone(int x, int y, Stone stone, bool existing = false)
        {
            CheckIndex(x, y);

            if (stacks[x, y].Count == 0)
            {
                stacks[x, y].AddStone(stone);
                return true;
            }
            else
            {
                if (!existing)
                {
                    throw new IllegalMoveException("\nCan not place new stone on an occupied space.");
                }
                else
                {
                    // Todo
                }
            }
            return false;
        }

        public StoneStack PickUpStack(int x, int y, int amount = UNSPECIFIED)
        {
            CheckIndex(x, y);

            if (amount == UNSPECIFIED)
            {
                if (stacks[x, y].Count > 1)
                    throw new IllegalMoveException("\nUnspecified number of stones to pick up, but stack contains more than one");
                else
                    amount = 1;
            }

            if (amount > stacks[x, y].Count)
                throw new IllegalMoveException("\nCould not pick up " + amount + " stones, stack contains " + stacks[x, y].Count);

            StoneStack pickedUp = stacks[x, y].Separate(amount);
            return pickedUp;
        }

        private void CheckIndex(int x, int y)
        {
            if (x < 0 || x >= size || y < 0 || y >= size)
                throw new IllegalMoveException("\nIndex [" + x + ", " + y + "] is out of bounds");
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
