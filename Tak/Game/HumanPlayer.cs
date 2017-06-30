using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.AI;

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
#if DEBUG
            Console.WriteLine("- DEBUG INFO -");
            Console.WriteLine("Road score: " + Evaluator.RoadScore(board, colour));
            Console.WriteLine("Connected Component: " + Evaluator.ConnectedComponentScore(board, colour));
            Console.WriteLine("GameState: " + (board.GameState == GameState.InProgress ? "In progress" : "Game over"));
            Console.WriteLine("Test: " + (board.Test ? "True" : "False"));
            Console.WriteLine("Proximity score: " + Evaluator.ProximityScore(board, colour));
#endif
            Console.WriteLine("\n" + colourText + " turn!");
            Console.Write("Enter move: ");
            return Console.ReadLine();
        }
    }
}
