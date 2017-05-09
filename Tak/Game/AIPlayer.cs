using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.AI;

namespace Tak.Game
{
    class AIPlayer : Player
    {
        MiniMaxAI ai;
        public AIPlayer(Colour c, Interpreter i, GameBoard board) : base(c, i)
        {
            ai = new MiniMaxAI(board);
        }

        public override void DoMove()
        {
            string move = ai.BestMove();
            interpreter.Input(move);
            Console.Write("\nAI did move: " + move);
            Task.Delay(2000).Wait();
        }
    }
}
