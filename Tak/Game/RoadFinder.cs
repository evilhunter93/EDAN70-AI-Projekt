using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak;

namespace Tak.Game
{
    class RoadFinder
    {
        private StoneStack[,] stacks;
        private int size;
        private bool[,] visited;

        public GameState Search(StoneStack[,] stacks, int size)
        {
            this.stacks = stacks;
            this.size = size;
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

            if (stacks[x0, y0].Owner != stacks[x, y].Owner)
                return false;

            if (visited[x, y])
                return false;

            visited[x, y] = true;

            if (Math.Abs(x0 - x) == (size - 1) || Math.Abs(y0 - y) == (size - 1)) // Opposite side reached
                return true;

            return ExploreRoad(x0, y0, x + 1, y)
                || ExploreRoad(x0, y0, x, y + 1)
                || ExploreRoad(x0, y0, x - 1, y)
                || ExploreRoad(x0, y0, x, y - 1);
        }
    }
}
