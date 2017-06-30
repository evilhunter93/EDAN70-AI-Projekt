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
            int score = 0;
            int currScore = 0;
            int size = board.Size;
            bool[,] visited = new bool[size, size];
            StoneStack[,] stacks = board.StacksReference;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int[] index = new int[] { i, i, j, j };
                    currScore = ScoreRoad(i, j, size, visited, stacks, index, player);
                    score = score > currScore ? score : currScore;
                }
            }
            return score;
        }

        private static int ScoreRoad(int i, int j, int size, bool[,] visited, StoneStack[,] stacks, int[] index, Colour player)
        {
            if (i < 0 || i >= size || j < 0 || j >= size)
                return 0;

            if (visited[i, j])
                return 0;

            if (stacks[i, j].Count == 0)
                return 0;

            if (player != stacks[i, j].Owner)
                return 0;

            if (!stacks[i, j].Top.Road)
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

            int horScore = index[1] - index[0] + 1;
            int verScore = index[3] - index[2] + 1;
            int currScore = (verScore > horScore) ? verScore : horScore;

            int[] scores = new int[4];
            scores[0] = ScoreRoad(i - 1, j, size, visited, stacks, index, player);
            scores[1] = ScoreRoad(i + 1, j, size, visited, stacks, index, player);
            scores[2] = ScoreRoad(i, j - 1, size, visited, stacks, index, player);
            scores[3] = ScoreRoad(i, j + 1, size, visited, stacks, index, player);

            int score = 0;
            for (int m = 0; m < scores.Count(); m++)
            {
                if (score < scores[m])
                    score = scores[m];
            }
            score = score > currScore ? score : currScore;
            return score;
        }

        public static int ConnectedComponentScore(GameBoard board, Colour player)
        {
            int score = 0;
            int current;
            int size = board.Size;
            bool[,] visited = new bool[size, size];
            StoneStack[,] stacks = board.StacksReference;

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    current = ExploreComponent(i, j, size, visited, stacks, player);
                    if (current > score)
                        score = current;
                }
            return score;
        }

        private static int ExploreComponent(int x, int y, int size, bool[,] visited, StoneStack[,] stacks, Colour player)
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

            return 1 + ExploreComponent(x + 1, y, size, visited, stacks, player)
                     + ExploreComponent(x, y + 1, size, visited, stacks, player)
                     + ExploreComponent(x - 1, y, size, visited, stacks, player)
                     + ExploreComponent(x, y - 1, size, visited, stacks, player);
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
            EnqueueAdjacent(q, new Position(i, j, 0));

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

                EnqueueAdjacent(q, p);
            }

            return 2 * size;
        }

        private static void EnqueueAdjacent(Queue<Position> q, Position p)
        {
            q.Enqueue(new Position(p.i - 1, p.j, p.distance + 1));
            q.Enqueue(new Position(p.i + 1, p.j, p.distance + 1));
            q.Enqueue(new Position(p.i, p.j - 1, p.distance + 1));
            q.Enqueue(new Position(p.i, p.j + 1, p.distance + 1));
        }
    }
}