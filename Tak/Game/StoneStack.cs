using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    public class StoneStack
    {
        private Stack<Stone> stones;

        public Colour Owner { get { return stones.Peek().Colour; } }

        public Stone Top { get { return stones.Peek(); } }

        public int Count { get { return stones.Count; } }

        public StoneStack()
        {
            stones = new Stack<Stone>();
        }

        public void NewStone(Stone stone)
        {
            if (Count > 0)
                throw new IllegalMoveException("\nCan not place new stone on an occupied space.");
            else
                stones.Push(stone);
        }

        public void AddStone(Stone stone)
        {
            if (Top is Capstone)
                throw new IllegalMoveException("\nCan not place stone on capstone.");

            if (Count > 0)
            {
                if (!(stone is Capstone))
                {
                    if (Top.Standing)
                        throw new IllegalMoveException("\nCan not place non-capstone on standingstone.");
                }
                else
                    stones.Peek().Standing = false;
            }
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
            if (nbr > Count)
                throw new IllegalMoveException("\nCould not pick up " + nbr + " stones, stack contains " + Count);

            StoneStack temp = new StoneStack();
            for (int i = 0; i < nbr; i++)
            {
                temp.AddStone(stones.Pop());
            }
            return temp;
        }
    }
}