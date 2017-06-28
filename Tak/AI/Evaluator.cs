using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Game;

namespace Tak.AI
{
    class Evaluator
    {
        public static int StackScore(GameBoard board, Colour player)
        {
            int score = 0;
            StoneStack[,] stacks = board.StacksReference;
            foreach (StoneStack stack in stacks)
            {
                if (stack.Count > 0)
                {
                    if (stack.Owner == player)
                        score += stack.Count;
                }
            }
            return score;
        }

        public static int TopPieceScore(GameBoard board, Colour player)
        {
            int score = 0;
            StoneStack[,] stacks = board.StacksReference;
            foreach (StoneStack stack in stacks)
                if (stack.Count > 0)
                    if (stack.Owner == player)
                        if (stack.Top.Road)
                            if (stack.Top is Flatstone)
                                score += 3;
                            else
                                score += 1;
                        else
                            score += 2;
            return score;
        }

        public static int RoadScore(GameBoard board, Colour player)
        {
            return board.BestRoad(player);
        }

        public static int ConnectedComponentScore(GameBoard board, Colour player)
        {
            return board.LargestConnectedComponent(player);
        }

        public static int ProximityScore(GameBoard board, Colour player)
        {
            // FIXME: score should benefit short distances and many pieces
            int size = board.Size;
            StoneStack stack;
            int score = 0;
            StoneStack[,] stacks = board.StacksReference;
            bool[,] visited = new bool[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    stack = stacks[i, j];
                    if (stack.Count > 0)
                        if (stack.Owner == player)
                            if (stack.Top.Road)
                            {
                                visited = new bool[size, size];
                                score += DistanceToNearest(i, j, stacks, size, visited, player);
                            }
                }

            return -score;
        }

        public static int GameStateScore(GameState gs, Colour player)
        {
            if (gs != GameState.InProgress && gs != GameState.Tie)
            {
                if (((gs == GameState.BF || gs == GameState.BR) && player == Colour.Black) ||
                    ((gs == GameState.WF || gs == GameState.WR) && player == Colour.White))
                    return int.MaxValue;
                else
                    return int.MinValue;
            }
            return 0;
        }

        private static int DistanceToNearest(int i, int j, StoneStack[,] stacks, int size, bool[,] visited, Colour player)
        {
            if (i < 0 || i >= size || j < 0 || j >= size)
                return int.MaxValue;

            if (visited[i, j])
                return int.MaxValue;

            visited[i, j] = true;

            if (stacks[i, j].Count > 0)
                if (stacks[i, j].Owner == player)
                    if (stacks[i, j].Top.Road)
                        return 1;

            int[] distances = new int[4];
            distances[0] = DistanceToNearest(i - 1, j, stacks, size, visited, player);
            distances[1] = DistanceToNearest(i + 1, j, stacks, size, visited, player);
            distances[2] = DistanceToNearest(i, j - 1, stacks, size, visited, player);
            distances[3] = DistanceToNearest(i, j + 1, stacks, size, visited, player);

            return 1 + distances.Min();
        }
    }
}