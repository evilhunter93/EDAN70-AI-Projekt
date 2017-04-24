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
    public class GameBoard : IEquatable<GameBoard>
    {
        public const int UNSPECIFIED = -1;
        private int size;
        private StoneStack[,] stacks;
        private bool[,] visited;
        private Colour turn;
        private GameState state;
        private StoneReserve whiteStones;
        private StoneReserve blackStones;

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
            if (size < 3 || size > 8)
                throw new TakException("Minimum board size is 3. Maximum board size is 8.");
            turn = Colour.White;
            this.size = size;
            stacks = new StoneStack[size, size];
            state = GameState.InProgress;
            whiteStones = new StoneReserve(size);
            blackStones = new StoneReserve(size);

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
                if (stone.Colour == Colour.White)
                {
                    whiteStones.CheckReserve(stone);
                    stacks[x, y].NewStone(stone);
                    whiteStones.Decrement(stone);
                }
                else if (stone.Colour == Colour.Black)
                {
                    blackStones.CheckReserve(stone);
                    stacks[x, y].NewStone(stone);
                    blackStones.Decrement(stone);
                }
                else
                {
                    throw new TakException("Colour not recognised.");
                }
            }
        }

        public StoneStack PickUpStack(int x, int y, int amount = UNSPECIFIED)
        {
            CheckIndex(x, y);
            CheckOwner(x, y);

            if (amount == UNSPECIFIED)
            {
                if (stacks[x, y].Count < 1)
                    throw new IllegalMoveException("\nCount < 1");
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


        public override bool Equals(Object obj)
        {
            var other = obj as GameBoard;
            if (other == null)
                return false;

            return Equals(other);
        }

        public bool Equals(GameBoard other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (size != other.size)
                return false;

            if (turn != other.Turn)
                return false;

            if (state != other.GameState)
                return false;

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (!stacks[i, j].Equals(other.stacks[i, j]))
                        return false;

            return true;
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

        private class StoneReserve
        {
            private int flatstones;
            private int capstones;

            internal StoneReserve(int size)
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

            internal void CheckReserve(Stone stone)
            {
                if (stone is Capstone)
                {
                    if (capstones < 1)
                        throw new IllegalMoveException("No more capstones in the reserve.");
                }
                else if (stone is Flatstone)
                {
                    if (flatstones < 1)
                        throw new IllegalMoveException("No more flat stones in the reserve.");
                }
                else
                    throw new TakException("Stone not recognised");
            }

            internal void Decrement(Stone stone)
            {
                if (stone is Capstone)
                    capstones--;
                else if (stone is Flatstone)
                    flatstones--;
                else
                    throw new TakException("Stone not recognised");
            }
        }
    }
}
