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
            TakGame game = new TakGame(5, "Human", "AI");
            game.Run();

            Console.Write("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}
