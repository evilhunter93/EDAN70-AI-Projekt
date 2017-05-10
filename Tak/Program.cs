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
            // Players: Human, MiniMaxAI, LearningAI
            TakGame game = new TakGame(5, "Human", "MiniMaxAI", 2);
            game.Run();

            Console.Write("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}
