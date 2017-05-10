using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    class HumanPlayer : Player
    {
        public HumanPlayer(GameBoard b, Colour c) : base(b, c) { }

        public override string DoMove()
        {
            string move;
            do
            {
                move = PromptInput();
            } while (move.Count() == 0);
            Interpreter.Input(move, board);
            return move;
        }

        private string PromptInput()
        {
            Console.WriteLine("\n" + colourText + " turn!");
            Console.Write("Enter move: ");
            return Console.ReadLine();
        }
    }
}
