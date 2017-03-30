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

        public bool AddStone(Stone stone)
        {
            if (stone is Capstone)
            {
                if (Count > 0)
                    stones.Peek().Standing = false;
                stones.Push(stone);
                return true;
            }
            else if (stones.Count == 0 || stones.Peek().Standing == false)
            {
                stones.Push(stone);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveStone()
        {
            if (stones.Count != 0)
            {
                stones.Pop();
                return true;
            }
            return false;
        }

        public StoneStack Separate(int nbr)
        {
            StoneStack temp = new StoneStack();
            for (int i = 0; i < nbr; i++)
            {
                temp.AddStone(stones.Pop());
            }
            return temp;
        }
    }
}