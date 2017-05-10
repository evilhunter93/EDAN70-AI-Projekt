using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Exceptions;

namespace Tak.Game
{
    public class StoneStack : IEquatable<StoneStack>
    {
        private Stack<Stone> stones;
        private bool test;

        public bool Test
        {
            get { return test; }
            set { test = value; }
        }

        public Colour Owner { get { return stones.Peek().Colour; } }

        public Stone Top { get { return stones.Peek(); } }

        public int Count { get { return stones.Count; } }

        public StoneStack()
        {
            stones = new Stack<Stone>();
            test = false;
        }

        public void NewStone(Stone stone)
        {
            if (Count > 0)
                throw new IllegalMoveException("\nCan not place new stone on an occupied space.");
            else if (!test)
                stones.Push(stone);
        }

        public void AddStone(Stone stone, bool pickUp = false)
        {
            if (!pickUp)
            {
                if (Count > 0)
                {
                    if (Top is Capstone)
                        throw new IllegalMoveException("\nCan not place stone on capstone.");

                    if (!(stone is Capstone))
                    {
                        if (Top.Standing)
                            throw new IllegalMoveException("\nCan not place non-capstone on standingstone.");
                    }
                    else if (!test)
                        stones.Peek().Standing = false;
                }
            }
            if (!test)
                stones.Push(stone);
        }

        public Stone PopStone()
        {
            if (stones.Count == 0)
                throw new IllegalMoveException("\nCan not pop from empty StoneStack.");
            else
                return stones.Pop();
        }

        public StoneStack Separate(int nbr)
        {
            if (nbr < 1)
                throw new IllegalMoveException("\nCould not pick up " + nbr + " stones.");

            if (nbr > Count)
                throw new IllegalMoveException("\nCould not pick up " + nbr + " stones, stack contains " + Count);

            StoneStack temp = new StoneStack();
            if (!test)
                for (int i = 0; i < nbr; i++)
                {
                    temp.AddStone(stones.Pop(), true);
                }
            else
                foreach (Stone s in stones.Take(nbr))
                    temp.AddStone(s, true);
            return temp;
        }

        public override bool Equals(object obj)
        {
            var other = obj as StoneStack;
            if (other == null)
                return false;

            return Equals(other);
        }

        public bool Equals(StoneStack other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (stones.Count != other.Count)
                return false;

            Stone[] array1 = new Stone[Count];
            array1 = stones.ToArray();
            Stone[] array2 = new Stone[Count];
            array2 = other.stones.ToArray();

            for (int i = 0; i < Count; i++)
                if (!array1[i].Equals(array2[i]))
                    return false;

            return true;
        }
    }
}