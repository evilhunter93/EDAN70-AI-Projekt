using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tak.Game;
using Tak.Exceptions;
using System.Diagnostics;

namespace TakTests
{
    [TestClass]
    public class GameBoardTest
    {
        [TestMethod]
        [ExpectedException(typeof(IllegalMoveException))]
        public void DuplicateNewStonePlacement()
        {
            GameBoard board1 = new GameBoard(5);
            board1.PlaceStone(0, 0, new Flatstone(Colour.Black));
            board1.PlaceStone(0, 0, new Flatstone(Colour.Black));
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalMoveException))]
        public void DeepCopyEquality()
        {
            GameBoard board1 = new GameBoard(5);
            board1.PlaceStone(0, 0, new Flatstone(Colour.Black));

            GameBoard board2 = new GameBoard(5);
            board2.Stacks = board1.Stacks;
            board2.PlaceStone(0, 0, new Flatstone(Colour.Black));
        }

        [TestMethod]
        public void DeepCopyIndependence()
        {
            GameBoard board1 = new GameBoard(5);
            GameBoard board2 = new GameBoard(5);
            board2.Stacks = board1.Stacks;
            board2.PlaceStone(0, 0, new Flatstone(Colour.Black));
            board1.PlaceStone(0, 0, new Flatstone(Colour.Black));
        }

        [TestMethod]
        public void FullBoardVictory()
        {
            int size = 5;
            GameBoard board = new GameBoard(size);
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if ((j < size - 1 && i < size - 1) || (j == size - 1 && i == size - 1))
                        board.PlaceStone(i, j, new Flatstone(Colour.White));
                    else
                        board.PlaceStone(i, j, new Flatstone(Colour.Black));
            board.EndTurn();
            Assert.AreEqual(board.GameState, GameState.WF);
        }

        [TestMethod]
        public void FullBoardTie()
        {
            int size = 4;
            GameBoard board = new GameBoard(size);
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if ((i + j) % 2 == 0)
                        board.PlaceStone(i, j, new Flatstone(Colour.White));
                    else
                        board.PlaceStone(i, j, new Flatstone(Colour.Black));
            board.EndTurn();
            Assert.AreEqual(board.GameState, GameState.Tie);
        }

        [TestMethod]
        public void RoadVictorySimple()
        {
            int size = 5;
            GameBoard board = new GameBoard(size);
            for (int i = 0; i < size; i++)
                board.PlaceStone(i, 0, new Flatstone(Colour.White));
            board.EndTurn();
            Assert.AreEqual(board.GameState, GameState.WR);
        }

        [TestMethod]
        public void RoadVictoryAdvanced()
        {
            int size = 5;
            GameBoard board = new GameBoard(size);
            for (int i = 0; i < size - 1; i++)
                for (int j = 0; j < size - 1; j++)
                    board.PlaceStone(i, j, new Flatstone(Colour.Black));

            board.PlaceStone(0, 0, new Flatstone(Colour.White), true);
            board.PlaceStone(0, 1, new Flatstone(Colour.White), true);
            board.PlaceStone(1, 1, new Flatstone(Colour.White), true);
            board.PlaceStone(2, 1, new Flatstone(Colour.White), true);
            board.PlaceStone(2, 2, new Flatstone(Colour.White), true);
            board.PlaceStone(2, 3, new Flatstone(Colour.White), true);
            board.PlaceStone(3, 3, new Flatstone(Colour.White), true);
            board.PlaceStone(3, 4, new Flatstone(Colour.White));
            board.PlaceStone(4, 4, new Flatstone(Colour.White));

            board.EndTurn();
            Assert.AreEqual(board.GameState, GameState.WR);
        }

        [TestMethod]
        public void RoadTie()
        {
            int size = 5;
            GameBoard board = new GameBoard(size);
            for (int i = 0; i < size; i++)
            {
                board.PlaceStone(i, 0, new Flatstone(Colour.White));
                board.PlaceStone(i, 1, new Flatstone(Colour.Black));
            }
            board.EndTurn();
            Assert.AreEqual(board.GameState, GameState.Tie);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalMoveException))]
        public void InputStone()
        {
            int size = 5;
            GameBoard board = new GameBoard(size);
            board.Turn = Colour.Black;
            Interpreter parser = new Interpreter(board);
            parser.input("a1");
            board.PlaceStone(0, 0, new Flatstone(Colour.Black));
        }

        [TestMethod]
        public void SizeEquality()
        {
            GameBoard board1 = new GameBoard(5);
            GameBoard board2 = new GameBoard(5);
            GameBoard board3 = new GameBoard(4);

            Assert.AreEqual(board1, board2);
            Assert.AreNotEqual(board2, board3);
        }


        [TestMethod]
        public void TurnEquality()
        {
            GameBoard board1 = new GameBoard(5);
            GameBoard board2 = new GameBoard(5);
            GameBoard board3 = new GameBoard(5);

            board1.Turn = Colour.White;
            board2.Turn = Colour.White;
            board3.Turn = Colour.Black;

            Assert.AreEqual(board1, board2);
            Assert.AreNotEqual(board2, board3);
        }

        [TestMethod]
        public void SimpleBoardEquality()
        {
            GameBoard board1 = new GameBoard(5);
            GameBoard board2 = new GameBoard(5);
            GameBoard board3 = new GameBoard(5);

            board1.PlaceStone(0, 0, new Flatstone(Colour.Black));
            board1.PlaceStone(1, 0, new Flatstone(Colour.Black));
            board1.PlaceStone(3, 3, new Flatstone(Colour.Black));
            board1.PlaceStone(4, 1, new Flatstone(Colour.Black));
            board1.PlaceStone(4, 4, new Flatstone(Colour.Black));

            board1.PlaceStone(0, 4, new Flatstone(Colour.White));
            board1.PlaceStone(1, 1, new Flatstone(Colour.White));
            board1.PlaceStone(1, 4, new Flatstone(Colour.White));
            board1.PlaceStone(3, 1, new Flatstone(Colour.White));
            board1.PlaceStone(3, 2, new Flatstone(Colour.White));

            board2.PlaceStone(0, 0, new Flatstone(Colour.Black));
            board2.PlaceStone(1, 0, new Flatstone(Colour.Black));
            board2.PlaceStone(3, 3, new Flatstone(Colour.Black));
            board2.PlaceStone(4, 1, new Flatstone(Colour.Black));
            board2.PlaceStone(4, 4, new Flatstone(Colour.Black));

            board2.PlaceStone(0, 4, new Flatstone(Colour.White));
            board2.PlaceStone(1, 1, new Flatstone(Colour.White));
            board2.PlaceStone(1, 4, new Flatstone(Colour.White));
            board2.PlaceStone(3, 1, new Flatstone(Colour.White));
            board2.PlaceStone(3, 2, new Flatstone(Colour.White));

            Assert.AreEqual(board1, board2);
            Assert.AreNotEqual(board2, board3);
        }
    }
}
