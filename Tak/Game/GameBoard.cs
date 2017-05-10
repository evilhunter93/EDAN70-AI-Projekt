using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Utilities;
using Tak.Exceptions;
using System.Collections;

public enum GameState
{
    WR, BR, WF, BF, Tie, InProgress
}

public enum Direction
{
    UP, DOWN, LEFT, RIGHT
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

        public StoneStack[,] StacksReference
        {
            get { return stacks; }
            set { stacks = value; }
        }

        public StoneStack[,] StacksCopy
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

        private GameBoard(GameBoard other)
        {
            size = other.size;
            stacks = other.StacksCopy; // deep copy
            state = other.state;
            turn = other.turn;
            whiteStones = other.whiteStones.Clone();
            blackStones = other.blackStones.Clone();
        }

        public GameBoard Clone()
        {
            return new GameBoard(this);
        }

        public void PlaceStone(int x, int y, Stone stone, bool existing = false)
        {
            if (!ValidIndex(x, y))
                throw new IllegalMoveException("\nIndex [" + x + ", " + y + "] is out of bounds");

            if (existing)
                stacks[x, y].AddStone(stone);
            else
            {
                if (stone.Colour == Colour.White)
                {
                    if (!whiteStones.CheckReserve(stone))
                        throw new IllegalMoveException("No more " + (stone is Capstone ? "capstones" : "flatstones") + " in the reserve.");

                    stacks[x, y].NewStone(stone);
                    whiteStones.Decrement(stone);
                }
                else if (stone.Colour == Colour.Black)
                {
                    if (!blackStones.CheckReserve(stone))
                        throw new IllegalMoveException("No more " + (stone is Capstone ? "capstones" : "flatstones") + " in the reserve.");

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
            if (!ValidIndex(x, y))
                throw new IllegalMoveException("\nIndex [" + x + ", " + y + "] is out of bounds");
            if (!CurrentPlayerIsOwner(x, y))
                throw new IllegalMoveException("\nCurrent player does not control the stack at [" + x + ", " + y + "].");

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

        public List<string> ValidMoves(Colour player)
        {
            List<string> validMoves = new List<string>();
            StoneReserve stones;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    string coords = CoordsToString(i, j);
                    if (stacks[i, j].Count == 0)
                    {
                        if (player == Colour.White)
                            stones = whiteStones;
                        else if (player == Colour.Black)
                            stones = blackStones;
                        else
                            throw new TakException("Colour not recognised.");

                        if (stones.CheckReserve(new Flatstone(player)))
                        {
                            validMoves.Add(coords);
                            validMoves.Add("S" + coords);
                        }

                        if (stones.CheckReserve(new Capstone(player)))
                            validMoves.Add("C" + coords);
                    }
                    else
                    {
                        if (player == stacks[i, j].Owner)
                        {
                            for (int nbrPickUp = 1; nbrPickUp <= size; nbrPickUp++)
                            {
                                if (nbrPickUp > stacks[i, j].Count)
                                    break;

                                bool singleMove = true;
                                string move = coords;
                                if (stacks[i, j].Count > 1)
                                {
                                    move = nbrPickUp + move;
                                    singleMove = false;
                                }

                                foreach (Direction d in Enum.GetValues(typeof(Direction)))
                                {
                                    string direction;
                                    int iStep = 0, jStep = 0;
                                    switch (d)
                                    {
                                        case Direction.UP:
                                            direction = "+";
                                            jStep = 1;
                                            break;
                                        case Direction.DOWN:
                                            direction = "-";
                                            jStep = -1;
                                            break;
                                        case Direction.LEFT:
                                            direction = "<";
                                            iStep = -1;
                                            break;
                                        case Direction.RIGHT:
                                            direction = ">";
                                            iStep = 1;
                                            break;
                                        default:
                                            throw new TakException("Direction not recognised.");
                                    }
                                    for (int dist = 1; dist <= nbrPickUp; dist++)
                                    {
                                        if (!ValidIndex(i + iStep * dist, j + jStep * dist))
                                            break;

                                        PutDownR(nbrPickUp, dist, move + direction, ref validMoves, singleMove);
                                    }
                                }
                            }
                        }
                    }
                }
            RemoveInvalidMoves(ref validMoves);
            return validMoves;
        }

        private void RemoveInvalidMoves(ref List<string> validMoves)
        {
            // TODO: Make faster version
            validMoves.RemoveAll(move =>
            {
                GameBoard testBoard = Clone();
                try
                {
                    Interpreter.Input(move, testBoard);
                    return false;
                }
                catch (IllegalMoveException)
                {
                    return true;
                }
                catch (IllegalInputException e)
                {
                    Console.Write("\nInvalid move: " + move + "\n");
                    throw e;
                }
            });
        }

        private void PutDownR(int pickUp, int dist, string move, ref List<string> moves, bool singleMove = false)
        {
            //TODO: Stop search if blocking stone found
            if (dist < 1)
                throw new TakException("Error in PutDownR(): dist < 1");



            if (dist == 1)
            {
                if (!singleMove)
                    move += pickUp;

                moves.Add(move);
            }
            else
                for (int i = 1; i <= pickUp - dist + 1; i++)
                    PutDownR(pickUp - i, dist - 1, move + i, ref moves);
        }

        private string CoordsToString(int i, int j)
        {
            char[] chars = { (char)('a' + i), (char)('0' + j + 1) };
            return new string(chars);
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

            if (!stacks[x, y].Top.Road)
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

        private bool CurrentPlayerIsOwner(int x, int y)
        {
            if (stacks[x, y].Owner != turn)
                return false;
            return true;
        }

        private bool ValidIndex(int x, int y)
        {
            if (x < 0 || x >= size || y < 0 || y >= size)
                return false;
            return true;
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

            StoneReserve(StoneReserve other)
            {
                flatstones = other.flatstones;
                capstones = other.capstones;
            }

            internal StoneReserve Clone()
            {
                return new StoneReserve(this);
            }

            internal bool CheckReserve(Stone stone)
            {
                if (stone is Capstone)
                {
                    if (capstones < 1)
                        return false;
                }
                else if (stone is Flatstone)
                {
                    if (flatstones < 1)
                        return false;
                }
                else
                    throw new TakException("Stone not recognised");

                return true;
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
