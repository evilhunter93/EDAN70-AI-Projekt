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
        private class Position
        {
            internal int i, j, distance;
            internal Position(int i, int j, int distance)
            {
                this.i = i;
                this.j = j;
                this.distance = distance;
            }
        }

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
            int size = board.Size;
            int maxDistance = size * 2;
            int nbrPieces = 0;
            StoneStack stack;
            int score = 0;
            StoneStack[,] stacks = board.StacksReference;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    stack = stacks[i, j];
                    if (stack.Count > 0)
                        if (stack.Owner == player)
                            if (stack.Top.Road)
                            {
                                nbrPieces++;
                                score += maxDistance - DistanceToNearest(i, j, stacks, size, player);
                            }
                }
            score = nbrPieces > 0 ? score / nbrPieces : 0;

            return score;
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

        private static int DistanceToNearest(int i, int j, StoneStack[,] stacks, int size, Colour player)
        {
            bool[,] visited = new bool[size, size];
            visited[i, j] = true;

            Queue<Position> q = new Queue<Position>();
            EnqueueDirections(q, new Position(i, j, 0));

            Position p;
            while (q.Count > 0)
            {
                p = q.Dequeue();
                if (p.i < 0 || p.i >= size || p.j < 0 || p.j >= size)
                    continue;
                if (visited[p.i, p.j])
                    continue;

                if (stacks[p.i, p.j].Count > 0)
                    if (stacks[p.i, p.j].Owner == player)
                        if (stacks[p.i, p.j].Top.Road)
                            return p.distance;

                visited[p.i, p.j] = true;

                EnqueueDirections(q, p);
            }

            return 2 * size;
        }

        private static void EnqueueDirections(Queue<Position> q, Position p)
        {
            q.Enqueue(new Position(p.i - 1, p.j, p.distance + 1));
            q.Enqueue(new Position(p.i + 1, p.j, p.distance + 1));
            q.Enqueue(new Position(p.i, p.j - 1, p.distance + 1));
            q.Enqueue(new Position(p.i, p.j + 1, p.distance + 1));
        }
    }
}