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

            board1.Turn = Colour.Black;
            board2.Turn = Colour.Black;
            board2.PlaceStone(0, 0, new Flatstone(Colour.Black));
            board2.PlaceStone(4, 4, new Flatstone(Colour.Black));
            Stone stone = new Flatstone(Colour.Black);
            stone.Standing = true;
            board2.PlaceStone(1, 3, stone);
            board2.PlaceStone(2, 2, new Capstone(Colour.Black));
            board2.PlaceStone(3, 1, new Flatstone(Colour.Black));

            Interpreter.Input("a1", board1);
            Interpreter.Input("e5", board1);
            Interpreter.Input("Sb4", board1);
            Interpreter.Input("Cc3", board1);
            Interpreter.Input("d2", board1);

            Assert.AreEqual(board1, board2);
        }

        [TestMethod]
        public void MoveStone()
        {
            GameBoard board1 = new GameBoard(5);
            GameBoard board2 = new GameBoard(5);

            board1.Turn = Colour.White;
            board2.Turn = Colour.White;

            board1.PlaceStone(2, 2, new Flatstone(Colour.Black));
            board1.PlaceStone(2, 1, new Flatstone(Colour.White));

            board2.PlaceStone(2, 2, new Flatstone(Colour.Black));
            board2.PlaceStone(2, 2, new Flatstone(Colour.White), true);

            Interpreter.Input("c2+", board1);
            Assert.AreEqual(board1, board2);

            board1 = new GameBoard(5);
            board1.Turn = Colour.White;

            board1.PlaceStone(2, 2, new Flatstone(Colour.Black));
            board1.PlaceStone(2, 3, new Flatstone(Colour.White));

            Interpreter.Input("c4-", board1);
            Assert.AreEqual(board1, board2);

            board1 = new GameBoard(5);
            board1.Turn = Colour.White;

            board1.PlaceStone(2, 2, new Flatstone(Colour.Black));
            board1.PlaceStone(1, 2, new Flatstone(Colour.White));

            Interpreter.Input("b3>", board1);
            Assert.AreEqual(board1, board2);

            board1 = new GameBoard(5);
            board1.Turn = Colour.White;

            board1.PlaceStone(2, 2, new Flatstone(Colour.Black));
            board1.PlaceStone(3, 2, new Flatstone(Colour.White));

            Interpreter.Input("d3<", board1);
            Assert.AreEqual(board1, board2);
        }

        [TestMethod]
        public void MoveStackSimple()
        {
            GameBoard board1 = new GameBoard(5);
            GameBoard board2 = new GameBoard(5);
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

            Interpreter.Input("5a1>122", board1);

            Assert.AreEqual(board1, board2);
        }
    }
}
