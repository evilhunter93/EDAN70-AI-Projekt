using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    class HumanPlayer : Player
    {
        public HumanPlayer(Colour c, Interpreter i) : base(c, i) { }

        public override void DoMove()
        {
            string move = PromptInput();
            //Console.WriteLine("\nYour input was [{0}]", move);
        }

        private string PromptInput()
        {
            Console.WriteLine("\n" + colourText + " turn!");
            Console.Write("Enter move: ");
            return Console.ReadLine();
        }
    }
}
