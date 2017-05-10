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
        private IAI ai;
        public AIPlayer(Colour c, Interpreter i, IAI ai) : base(c, i)
        {
            this.ai = ai;
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
