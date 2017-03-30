using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tak.Game;

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
    }
}
