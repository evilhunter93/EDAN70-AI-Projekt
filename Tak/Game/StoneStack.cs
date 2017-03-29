using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    class StoneStack
    {
        private Stack<Stone> stones;

        public StoneStack()
        {
            stones = new Stack<Stone>();
        }

        public bool addStone(Stone stone)
        {
            if (stone is Capstone || stones.Count == 0)
            {
                stones.Push(stone);
                return true;
            }
            else if (stones.Peek().Standing == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool removeStone()
        {
            if (stones.Count != 0)
            {
                stones.Pop();
                return true;
            }
            return false;
        }

        public StoneStack separate(int nbr)
        {
            StoneStack temp = new StoneStack();
            for (int i = 0; i < nbr; i++)
            {
                temp.addStone(stones.Pop());
            }
            return temp;
        }
    }
}