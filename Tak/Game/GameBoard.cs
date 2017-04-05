using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Utilities;

public enum GameState
{
    WR, BR, WF, BF, Tie, InProgress
}

namespace Tak.Game
{
    public class GameBoard
    {
        public const int UNSPECIFIED = -1;
        private int size;
        private StoneStack[,] stacks;
        private Colour turn;
        private GameState state;

        public Colour Turn { get { return turn; } set { turn = value; } }
        public GameState GameState { get { return state; } }

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
            turn = Colour.Black;
            this.size = size;
            stacks = new StoneStack[size, size];
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    stacks[x, y] = new StoneStack();
        }

        public void PlaceStone(int x, int y, Stone stone, bool existing = false)
        {
            CheckIndex(x, y);

            if (existing)
                stacks[x, y].AddStone(stone);
            else
                stacks[x, y].NewStone(stone);

        }

        public StoneStack PickUpStack(int x, int y, int amount = UNSPECIFIED)
        {
            CheckIndex(x, y);
            CheckOwner(x, y);

            if (amount == UNSPECIFIED)
            {
                if (stacks[x, y].Count > 1)
                    throw new IllegalMoveException("\nUnspecified number of stones to pick up, but stack contains more than one");
                else
                    amount = 1;
            }

            if (amount > size)
                throw new IllegalMoveException("\nCould not pick up " + amount + " stones, the size of the gameboard is " + size);

            StoneStack pickedUp = stacks[x, y].Separate(amount);
            return pickedUp;
        }

        public Colour EndTurn()
        {
            UpdateGameState();
            turn = (turn == Colour.Black) ? Colour.White : Colour.Black;
            return turn;
        }

        private void UpdateGameState()
        {
            if (FullBoard())
            {
                int nbrWhite = 0, nbrBlack = 0;
                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                    {
                        if (stacks[i, j].Owner == Colour.Black)
                            nbrBlack++;
                        else
                            nbrWhite++;
                    }

                if (nbrWhite > nbrBlack)
                    state = GameState.WF;
                else if (nbrWhite < nbrBlack)
                    state = GameState.BF;
                else
                    state = GameState.Tie;
            }
            else if (RoadExists())
            {
                // TODO
            }
            else
            {
                state = GameState.InProgress;
            }



        }

        private bool RoadExists()
        {
            // TODO
            throw new NotImplementedException();
        }

        private bool FullBoard()
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (stacks[i, j].Count == 0)
                        return false;

            return true;
        }

        private void CheckOwner(int x, int y)
        {
            if (stacks[x, y].Owner != turn)
                throw new IllegalMoveException("\nCurrent player does not control the stack at [" + x + ", " + y + "].");
        }

        private void CheckIndex(int x, int y)
        {
            if (x < 0 || x >= size || y < 0 || y >= size)
                throw new IllegalMoveException("\nIndex [" + x + ", " + y + "] is out of bounds");
        }
    }
}
