using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tak.Game;

namespace TakTest
{
    [TestClass]
    public class InterpreterTest
    {

        [TestMethod]
        public void PlaceStone()
        {
            GameBoard board1 = new GameBoard(5);
            GameBoard board2 = new GameBoard(5);
            Interpreter inter1 = new Interpreter(board1);

            board1.Turn = Colour.Black;
            board2.Turn = Colour.Black;
            inter1.input("a1");
            board2.PlaceStone(0, 0, new Flatstone(Colour.Black));

            Assert.AreEqual(board1, board2);
        }

        [TestMethod]
        public void MoveStone()
        {
            GameBoard board1 = new GameBoard(5);
            GameBoard board2 = new GameBoard(5);
            Interpreter inter1 = new Interpreter(board1);
            board1.Turn = Colour.White;
            board2.Turn = Colour.White;

            board1.PlaceStone(1, 0, new Flatstone(Colour.Black));
            board1.PlaceStone(0, 0, new Flatstone(Colour.White));

            board2.PlaceStone(1, 0, new Flatstone(Colour.Black));
            board2.PlaceStone(1, 0, new Flatstone(Colour.White), true);

            inter1.input("a1+");

            Assert.AreEqual(board1, board2);
        }

        [TestMethod]
        public void MoveStackSimple()
        {
            GameBoard board1 = new GameBoard(5);
            GameBoard board2 = new GameBoard(5);
            Interpreter inter1 = new Interpreter(board1);
            board1.Turn = Colour.White;
            board2.Turn = Colour.White;

            board1.PlaceStone(0, 0, new Flatstone(Colour.Black));
            board1.PlaceStone(0, 0, new Flatstone(Colour.White), true);
            board1.PlaceStone(0, 0, new Flatstone(Colour.White), true);
            board1.PlaceStone(0, 0, new Flatstone(Colour.Black), true);
            board1.PlaceStone(0, 0, new Flatstone(Colour.White), true);
            board1.PlaceStone(0, 0, new Flatstone(Colour.Black), true);
            board1.PlaceStone(0, 0, new Flatstone(Colour.White), true);
            board1.PlaceStone(1, 0, new Flatstone(Colour.Black));
            board1.PlaceStone(3, 0, new Flatstone(Colour.White));


            board2.PlaceStone(0, 0, new Flatstone(Colour.Black));
            board2.PlaceStone(0, 0, new Flatstone(Colour.White), true);
            board2.PlaceStone(1, 0, new Flatstone(Colour.Black));
            board2.PlaceStone(1, 0, new Flatstone(Colour.White), true);
            board2.PlaceStone(2, 0, new Flatstone(Colour.Black));
            board2.PlaceStone(2, 0, new Flatstone(Colour.White), true);
            board2.PlaceStone(3, 0, new Flatstone(Colour.White));
            board2.PlaceStone(3, 0, new Flatstone(Colour.Black), true);
            board2.PlaceStone(3, 0, new Flatstone(Colour.White), true);

            inter1.input("5a1>122");

            Assert.AreEqual(board1, board2);
        }
    }
}
