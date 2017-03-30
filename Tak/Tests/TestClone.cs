using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Game;

namespace Tak.Tests
{
    class TestClone
    {
        public void Test0()
        {
            GameBoard board1 = new GameBoard(5);
            board1.PlaceStone(0, 0, new Flatstone());
            try
            {
                board1.PlaceStone(0, 0, new Flatstone());
                Console.WriteLine("\nTest0: Failed!");
            }
            catch (IllegalMoveException) { Console.WriteLine("\nTest0: Passed!"); }
            // Check board1 and board2

        }

        public void Test1()
        {
            GameBoard board1 = new GameBoard(5);
            board1.PlaceStone(0, 0, new Flatstone());
            GameBoard board2 = new GameBoard(5);
            board2.Stacks = board1.Stacks;
            try
            {
                board2.PlaceStone(0, 0, new Flatstone()); // Error
                Console.WriteLine("\nTest1: Failed!");
            }
            catch (IllegalMoveException) { Console.WriteLine("\nTest1: Passed!"); }
            // Check board1 and board2

        }

        public void Test2()
        {
            GameBoard board1 = new GameBoard(5);
            GameBoard board2 = new GameBoard(5);
            board2.Stacks = board1.Stacks;
            board2.PlaceStone(0, 0, new Flatstone());
            try
            {
                board1.PlaceStone(0, 0, new Flatstone());
                Console.WriteLine("\nTest2: Passed!");
            }
            catch (IllegalMoveException) { Console.WriteLine("\nTest2: Failed!"); }
        }

    }


}
