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
        private bool test;

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
            set
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        stacks[x, y] = ObjectExtensions.Copy(value[x, y]);
                    }
                }
            }
        }

        public bool Test
        {
            get { return test; }
            set
            {
                test = value;
                foreach (StoneStack ss in stacks)
                    ss.Test = value;
            }
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
            Test = false;
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
                    if (!test)
                        whiteStones.Decrement(stone);
                }
                else if (stone.Colour == Colour.Black)
                {
                    if (!blackStones.CheckReserve(stone))
                        throw new IllegalMoveException("No more " + (stone is Capstone ? "capstones" : "flatstones") + " in the reserve.");

                    stacks[x, y].NewStone(stone);
                    if (!test)
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
            if (stacks[x, y].Count < 1)
                throw new IllegalMoveException("\nStack is empty.");
            if (!CurrentPlayerIsOwner(x, y))
                throw new IllegalMoveException("\nCurrent player does not control the stack at [" + x + ", " + y + "].");

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

        private IEnumerable<string> Moves(Colour player, bool setup)
        {
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
                            yield return coords;
                            if (!setup)
                                yield return "S" + coords;
                        }

                        if (stones.CheckReserve(new Capstone(player)))
                            if (!setup)
                                yield return "C" + coords;
                    }
                    else
                    {
                        if (player == stacks[i, j].Owner && !setup)
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

                                        foreach (string m in PutDownR(nbrPickUp, dist, move + direction, singleMove))
                                            yield return m;
                                    }
                                }
                            }
                        }
                    }
                }
        }

        public IEnumerable<string> ValidMoves(Colour player, bool setup = false)
        {
            Test = true;
            foreach (string m in Moves(player, setup))
            {
                try { Interpreter.Input(m, this); }
                catch (IllegalMoveException) { continue; }
                catch (IllegalInputException e)
                {
                    Console.Write("\nInvalid input: " + m + "\n");
                    throw e;
                }
                yield return m;
            }
            Test = false;
        }

        private IEnumerable<string> PutDownR(int pickUp, int dist, string move, bool singleMove = false)
        {
            if (dist < 1)
                throw new TakException("Error in PutDownR(): dist < 1");
            if (dist == 1)
            {
                if (!singleMove)
                    move += pickUp;

                yield return move;
            }
            else
                for (int i = 1; i <= pickUp - dist + 1; i++)
                    PutDownR(pickUp - i, dist - 1, move + i);
        }

        private string CoordsToString(int i, int j)
        {
            char[] chars = { (char)('a' + i), (char)('0' + j + 1) };
            return new string(chars);
        }

        private void UpdateGameState()
        {
            state = Search();
            if (state == GameState.InProgress && (FullBoard() || whiteStones.Empty() || blackStones.Empty()))
            {
                int nbrWhite = 0, nbrBlack = 0;
                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                    {
                        if (stacks[i, j].Top is Flatstone)
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
            bool WR = false, BR = false;

            for (int i = 0; i < size; i++)
            {
                visited = new bool[size, size];
                if (ExploreRoad(0, i, 0, i))
                {
                    WR = WR || (stacks[0, i].Owner == Colour.White);
                    BR = BR || (stacks[0, i].Owner == Colour.Black);
                }
                visited = new bool[size, size];
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

        public int LargestConnectedComponent(Colour player)
        {
            int largest = 0;
            int current;
            visited = new bool[size, size];

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    current = ExploreComponent(i, j, player);
                    if (current > largest)
                        largest = current;
                }
            return largest;
        }

        private int ExploreComponent(int x, int y, Colour player)
        {
            if (x < 0 || x >= size || y < 0 || y >= size)
                return 0;

            if (stacks[x, y].Count == 0)
                return 0;

            if (player != stacks[x, y].Owner)
                return 0;

            if (visited[x, y])
                return 0;

            if (!stacks[x, y].Top.Road)
                return 0;

            visited[x, y] = true;

            return 1 + ExploreComponent(x + 1, y, player)
                     + ExploreComponent(x, y + 1, player)
                     + ExploreComponent(x - 1, y, player)
                     + ExploreComponent(x, y - 1, player);
        }

        public int BestRoad(Colour player)
        {
            int bestScore = 0;
            Boolean[,] visited = new Boolean[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (!visited[i, j])
                    {
                        int[] index = new int[] { i, i, j, j };
                        bestScore = ScoreRoad(i, j, visited, index, player);
                    }
                }
            }
            return bestScore;
        }

        private int ScoreRoad(int i, int j, Boolean[,] visited, int[] index, Colour player)
        {
            if (visited[i, j] || i < 0 || i >= size || j < 0 || j >= size)
                return 0;

            if (stacks[i, j].Count == 0)
                return 0;

            if (player != stacks[i, j].Owner)
                return 0;

            visited[i, j] = true;

            if (i < index[0])
                index[0] = i;

            if (i > index[1])
                index[1] = i;

            if (j < index[2])
                index[2] = j;

            if (j > index[3])
                index[3] = j;

            var horScore = index[1] - index[0];
            var verScore = index[3] - index[2];
            var currScore = (verScore > horScore) ? verScore : horScore;
            currScore = 1 > currScore ? 1 : currScore;

            var scores = new int[3];
            scores[0] = ScoreRoad(i--, j, visited, index, player);
            scores[1] = ScoreRoad(i++, j, visited, index, player);
            scores[2] = ScoreRoad(i, j--, visited, index, player);
            scores[3] = ScoreRoad(i, j++, visited, index, player);

            var maxScore = 0;
            for (int m = 0; m < 4; m++)
            {
                if (maxScore < scores[m])
                    maxScore = scores[m];
            }
            maxScore = maxScore > currScore ? maxScore : currScore;
            return maxScore;
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

            internal bool Empty()
            {
                return flatstones == 0 && capstones == 0;
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
