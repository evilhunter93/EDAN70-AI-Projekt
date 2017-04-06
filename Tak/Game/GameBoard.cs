using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Utilities;
using Tak.Exceptions;

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
        private bool[,] visited;
        private Colour turn;
        private GameState state;

        private int whiteCapstones, blackCapstones, whiteFlatstones, blackFlatstones;

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

        public int WhiteCapstones { get { return whiteCapstones; } }
        public int WhiteFlatstones { get { return whiteFlatstones; } }
        public int BlackCapstones { get { return blackCapstones; } }
        public int BlackFlatstones { get { return blackFlatstones; } }

        public GameBoard(int size)
        {
            if (size < 3)
                throw new TakException("Minimum board size is 3");
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
            {
                CheckStoneReserve(stone.Colour, stone);
                stacks[x, y].NewStone(stone);
                decrement
            }
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
            state = Search();
            if (state == GameState.InProgress && FullBoard())
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
        }

        private GameState Search()
        {
            visited = new bool[size, size];

            bool WR = false, BR = false;
            for (int i = 0; i < size; i++)
            {
                if (ExploreRoad(0, i, 0, i))
                {
                    WR = WR || (stacks[0, i].Owner == Colour.White);
                    BR = BR || (stacks[0, i].Owner == Colour.Black);
                }
                if (ExploreRoad(i, 0, i, 0))
                {
                    WR = WR || (stacks[i, 0].Owner == Colour.White);
                    BR = BR || (stacks[i, 0].Owner == Colour.Black);
                }
            }

            if (WR && BR)
                return GameState.Tie;

            if (WR || BR)
                return WR ? GameState.WR : GameState.BR;

            return GameState.InProgress;
        }

        private bool ExploreRoad(int x0, int y0, int x, int y)
        {
            if (x < 0 || x >= size || y < 0 || y >= size)
                return false;

            if (stacks[x, y].Count == 0)
                return false;

            if (stacks[x0, y0].Owner != stacks[x, y].Owner)
                return false;

            if (visited[x, y])
                return false;

            visited[x, y] = true;

            if (Math.Abs(x - x0) == (size - 1) || Math.Abs(y - y0) == (size - 1)) // Opposite side reached
                return true;

            return ExploreRoad(x0, y0, x + 1, y)
                || ExploreRoad(x0, y0, x, y + 1)
                || ExploreRoad(x0, y0, x - 1, y)
                || ExploreRoad(x0, y0, x, y - 1);
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

        private void CheckStoneReserve(Colour colour, Stone stone)
        {
            if (colour == Colour.White)
            {
                if (stone is Capstone && whiteCapstones == 0)
                    throw new IllegalMoveException("No more capstones in White's reserve.");
                if (stone is Flatstone && whiteFlatstones == 0)
                    throw new IllegalMoveException("No more flat stones in White's reserve.");
            }
            else
            {
                if (stone is Capstone && blackCapstones == 0)
                    throw new IllegalMoveException("No more capstones in Black's reserve.");
                if (stone is Flatstone && blackFlatstones == 0)
                    throw new IllegalMoveException("No more flat stones in Black's reserve.");
            }
        }

        private class StoneReserve
        {
            private int flatstones;
            private int capstones;

            StoneReserve(int size)
            {
                if (size < 5)
                    capstones = 0;
                else if (size < 7)
                    capstones = 1;
                else
                    capstones = 2;

                if (size == 3)
                    flatstones = 10;
                else if (size == 4)
                    flatstones = 15;
                else if (size == 5)
                    flatstones = 21;
                else
                    flatstones = (size - 3) * 10;
            }

            void Decrement(Stone stone)
            {
                if (stone is Capstone)
                    capstones--;
                else if (stone is Flatstone)
                    flatstones--;
                else
                    throw new TakException("Stone not recognized");
            }

            void CheckReserve(Stone stone)
            {

            }
        }
    }
}
