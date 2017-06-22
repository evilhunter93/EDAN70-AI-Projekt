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
                    {
                        score += stack.Count;
                    }
                    else
                    {
                        score -= stack.Count;
                    }
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
                    else
                        score -= 1;
            return score;
        }

        public static int RoadScore(GameBoard board, Colour player)
        {

            return 0;
        }

        public static int ConnectedComponentScore(GameBoard board, Colour player)
        {
            return 0;
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
    }
}
