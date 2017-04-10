using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Game;

namespace Tak
{
    class Program
    {
        static void Main(string[] args)
        {
            TakGame game = new TakGame(4, "Human", "Human");
            game.Start();

            Console.Write("\nPress any key to exit...");
            Console.ReadLine();
        }
    }
}
