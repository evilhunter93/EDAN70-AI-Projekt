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
            //Stack<int> s = new Stack<int>();
            //for (int i = 0; i < 10; i++)
            //    s.Push(i);
            //foreach (int i in s.Take(5))
            //    Console.Write(i + " ");
            //Console.WriteLine();
            //foreach (int i in s)
            //    Console.Write(i + " ");

            // Players: Human, MiniMaxAI, LearningAI
            TakGame game = new TakGame(5, "Human", "MiniMaxAI", 3);
            game.Run();

            Console.Write("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}
