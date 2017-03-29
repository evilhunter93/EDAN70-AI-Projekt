using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    class Human : Player
    {
        public override void DoMove()
        {
            string move = PromptInput();
            Console.WriteLine("\nYour input was [{0}]", move);
        }

        private string PromptInput()
        {
            Console.WriteLine("\nYour turn!");
            Console.Write("Enter move: ");

            return Console.ReadLine();
        }
    }
}
