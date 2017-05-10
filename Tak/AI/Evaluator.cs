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
        public static int EvaluateStack(GameBoard board, Colour turn)
        {
            int score = 0;
            StoneStack[,] stacks = board.StacksReference;
            foreach (StoneStack stack in stacks)
            {
                if (stack.Count > 0)
                {
                    if (stack.Owner == turn)
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

        public static int EvaluateTopPiece(GameBoard board, Colour turn)
        {
            int score = 0;
            StoneStack[,] stacks = board.StacksReference;
            foreach (StoneStack stack in stacks)
                if (stack.Count > 0)
                    if (stack.Owner == turn)
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

        public static int EvaluateGameState(GameBoard board, Colour turn)
        {
            GameState gs = board.GameState;
            if (gs != GameState.InProgress && gs != GameState.Tie)
            {
                if (((gs == GameState.BF || gs == GameState.BR) && turn == Colour.Black) ||
                    ((gs == GameState.WF || gs == GameState.WR) && turn == Colour.White))
                    return int.MaxValue;
                else
                    return int.MinValue;
            }
            return 0;
        }
    }
}
