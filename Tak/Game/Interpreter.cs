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
            ch = input[charCount];
            //todo Check correct length
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
            bool ml = input.Contains("<");
            bool mr = input.Contains(">");
            bool mu = input.Contains("+");
            bool md = input.Contains("-");
            if (ml || mr || mu || md)
            {
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
                        StoneStack mStack = stack.Separate(n);
                        for (int j = 0; j < n; j++)
                        {
                            boardModel.PlaceStone(x, y, mStack.PopStone());
                        }
                    }
                }
                else if (mr)
                {
                    foreach (var n in stackPlace)
                    {
                        x--;
                        StoneStack mStack = stack.Separate(n);
                        for (int j = 0; j < n; j++)
                        {
                            boardModel.PlaceStone(x, y, mStack.PopStone());
                        }
                    }
                }
                else if (mu)
                {
                    foreach (var n in stackPlace)
                    {
                        x--;
                        StoneStack mStack = stack.Separate(n);
                        for (int j = 0; j < n; j++)
                        {
                            boardModel.PlaceStone(x, y, mStack.PopStone());
                        }
                    }
                }
                else
                {
                    foreach (var n in stackPlace)
                    {
                        x--;
                        StoneStack mStack = stack.Separate(n);
                        for (int j = 0; j < n; j++)
                        {
                            boardModel.PlaceStone(x, y, mStack.PopStone());
                        }
                    }
                }
            }
            else
            {
                boardModel.PlaceStone(x, y, stoneType);
            }
        }
    }
}
