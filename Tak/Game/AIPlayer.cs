﻿using System;
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
        public AIPlayer(GameBoard b, Colour c, IAI ai) : base(b, c)
        {
            this.ai = ai;
        }

        public override void DoMove()
        {
            string move = ai.BestMove();
            Interpreter.Input(move, board);
            Console.Write("\nAI did move: " + move);
            Task.Delay(2000).Wait();
        }
    }
}
