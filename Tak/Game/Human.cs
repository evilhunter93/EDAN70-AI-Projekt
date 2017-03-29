using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    class Human : Player
    {
        public override void doMove()
        {
            Console.WriteLine("\nYour turn!");
            Console.Write("Enter move: ");

            string move = Console.ReadLine();
            Console.WriteLine("\nYour input was [{0}]", move);
        }
    }
}
