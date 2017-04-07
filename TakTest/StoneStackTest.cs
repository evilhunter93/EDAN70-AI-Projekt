using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tak.Game;
using Tak.Exceptions;

namespace TakTest
{
    [TestClass]
    public class StoneStackTest
    {
        [TestMethod]
        public void SimpleStackEquality()
        {
            StoneStack stack1 = new StoneStack();
            StoneStack stack2 = new StoneStack();
            StoneStack stack3 = new StoneStack();
            StoneStack stack4 = new StoneStack();

            stack1.NewStone(new Flatstone(Colour.White));
            stack2.NewStone(new Flatstone(Colour.White));
            stack3.NewStone(new Flatstone(Colour.Black));
            stack4.NewStone(new Flatstone(Colour.Black));


            Assert.AreEqual(stack1, stack2);
            Assert.AreEqual(stack3, stack4);
            Assert.AreNotEqual(stack2, stack3);

            // A stack with one stone is not the same as a stone.
            Assert.AreNotEqual(stack2, new Flatstone(Colour.White));
        }

        [TestMethod]
        public void AdvancedStackEquality()
        {
            StoneStack stack1 = new StoneStack();
            StoneStack stack2 = new StoneStack();
            StoneStack stack3 = new StoneStack();

            stack1.NewStone(new Flatstone(Colour.White));
            stack1.AddStone(new Flatstone(Colour.Black));
            stack1.AddStone(new Flatstone(Colour.Black));
            stack1.AddStone(new Flatstone(Colour.White));

            stack2.NewStone(new Flatstone(Colour.White));
            stack2.AddStone(new Flatstone(Colour.Black));
            stack2.AddStone(new Flatstone(Colour.Black));
            stack2.AddStone(new Flatstone(Colour.White));

            stack3.NewStone(new Flatstone(Colour.Black));
            stack3.AddStone(new Flatstone(Colour.White));
            stack3.AddStone(new Flatstone(Colour.Black));
            stack3.AddStone(new Flatstone(Colour.White));

            Assert.AreEqual(stack1, stack2);
            Assert.AreNotEqual(stack2, stack3);

        }

        [TestMethod]
        public void FlatEquality()
        {
            Stone stone1 = new Flatstone(Colour.White);
            Stone stone2 = new Flatstone(Colour.White);
            Stone stone3 = new Flatstone(Colour.Black);
            Stone stone4 = new Flatstone(Colour.Black);

            Assert.AreEqual(stone1, stone2);
            Assert.AreEqual(stone3, stone4);
            Assert.AreNotEqual(stone2, stone3);
        }

        [TestMethod]
        public void StandingEquality()
        {
            Stone stone1 = new Flatstone(Colour.White);
            Stone stone2 = new Flatstone(Colour.White);
            Stone stone3 = new Flatstone(Colour.Black);
            Stone stone4 = new Flatstone(Colour.Black);

            stone2.Standing = true;
            stone3.Standing = true;
            stone4.Standing = true;

            Assert.AreNotEqual(stone1, stone2);
            Assert.AreNotEqual(stone2, stone3);
            Assert.AreEqual(stone3, stone4);
        }

        [TestMethod]
        public void CapEquality()
        {
            Stone stone1 = new Capstone(Colour.White);
            Stone stone2 = new Capstone(Colour.White);
            Stone stone3 = new Capstone(Colour.Black);
            Stone stone4 = new Capstone(Colour.Black);

            Assert.AreEqual(stone1, stone2);
            Assert.AreEqual(stone3, stone4);
            Assert.AreNotEqual(stone2, stone3);
        }
    }
}
