using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    class Interpreter
    {
        private GameBoard boardModel;

        public Interpreter(GameBoard boardModel)
        {
            this.boardModel = boardModel;
        }

        public void input(String input)
        {
            Stone stoneType;
            int x;
            int y;
            int charCount = 0;
            char ch;
            bool ml = input.Contains("<");
            bool mr = input.Contains(">");
            bool mu = input.Contains("+");
            bool md = input.Contains("-");
            if (ml || mr || mu || md)
            //todo Check correct length
            {
                ch = input[charCount];
                switch (ch)
                {
                    case 'S':
                        charCount++;
                        stoneType = new Flatstone(Colour.Black); // FIXME: Turn decides colour
                        stoneType.Standing = true;
                        break;
                    case 'C':
                        charCount++;
                        stoneType = new Capstone(Colour.Black); // FIXME: Turn decides colour
                        break;
                    default:
                        stoneType = new Flatstone(Colour.Black); // FIXME: Turn decides colour
                        break;
                }
                ch = input[charCount];
                x = ch - ('a' - '0');
                ch = input[charCount++];
                y = ch;
                ch = input[charCount += 2];
                String stackPlace = input.Substring(charCount);
                int amount = 0;
                int[] move = new int[stackPlace.Length];
                int k = 0;
                foreach (var i in stackPlace)
                {
                    amount += i;
                }
                StoneStack stack = boardModel.PickUpStack(x, y, amount);
                if (ml)
                {
                    foreach (var n in stackPlace)
                    {
                        x--;
                        for (int j = 0; j < n; j++)
                        {
                            boardModel.PlaceStone(x, y, stack.popStone());
                        }
                    }
                }
            }
        }
    }
}
