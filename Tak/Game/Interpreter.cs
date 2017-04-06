using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Exceptions;

namespace Tak.Game
{
    public class Interpreter
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
            switch (ch)
            {
                case 'S':
                    charCount++;
                    stoneType = new Flatstone(boardModel.Turn);
                    stoneType.Standing = true;
                    break;
                case 'C':
                    charCount++;
                    stoneType = new Capstone(boardModel.Turn);
                    break;
                default:
                    stoneType = new Flatstone(boardModel.Turn);
                    break;
            }
            ch = input[charCount];
            if ((ch > 'z') || (ch < 'a'))
            {
                //exception
                throw new IllegalInputException("Illegal character given, please specify a lowercase letter for the x-axis");
            }
            x = (ch - 'a') - '0';
            ch = input[charCount++];
            y = ch - 1;
            bool ml = input.Contains("<");
            bool mr = input.Contains(">");
            bool mu = input.Contains("+");
            bool md = input.Contains("-");
            bool m = ml || mr || mu || md;
            if (m && (input.Length < 3))
            {
                //exception
                throw new IllegalInputException("Illegal input format for movement of stones, need more arguments");
            }
            if (m && stoneType.Standing)
            {
                //exception
                throw new IllegalInputException("Illegal input format for movement of stones, stone-type should not be specified");
            }
            if (m)
            {
                ch = input[charCount += 2];
                String stackPlace = input.Substring(charCount);
                int amount = 0;
                int[] move = new int[stackPlace.Length];
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
                            boardModel.PlaceStone(x, y, stack.PopStone());
                        }
                    }
                }
                else if (mr)
                {
                    foreach (var n in stackPlace)
                    {
                        x++;
                        for (int j = 0; j < n; j++)
                        {
                            boardModel.PlaceStone(x, y, stack.PopStone());
                        }
                    }
                }
                else if (mu)
                {
                    foreach (var n in stackPlace)
                    {
                        y++;
                        for (int j = 0; j < n; j++)
                        {
                            boardModel.PlaceStone(x, y, stack.PopStone());
                        }
                    }
                }
                else
                {
                    foreach (var n in stackPlace)
                    {
                        y--;
                        for (int j = 0; j < n; j++)
                        {
                            boardModel.PlaceStone(x, y, stack.PopStone());
                        }
                    }
                }
            }
            else
            {
                if (input.Length > 3 || (input.Length > 2 && stoneType.Standing) || input.Length < 2)
                {
                    //exception
                    throw new IllegalInputException("Illegal input format for placing new stones");
                }
                boardModel.PlaceStone(x, y, stoneType);
            }
        }
    }
}
