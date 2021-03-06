﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Exceptions;

namespace Tak.Game
{
    public class Interpreter
    {
        delegate char IncrementCharCount(int n = 1);

        public static void Input(string input, GameBoard board)
        {
            if (input == null)
                throw new IllegalInputException("Input can not be null.");

            if (input.ToLower() == "exit")
                Environment.Exit(0);

            if (input.ToLower() == "valid")
            {
                IEnumerable<string> validMoves = board.ValidMoves(board.Turn);
                //Console.WriteLine(validMoves);
                foreach (string move in validMoves)
                    Console.Write("{0} ", move);
                throw new TakException("\nPrinting valid moves.");
            }


            Stone stoneType;
            int x;
            int y;
            int charCount = 0;
            IncrementCharCount increment = n =>
            {
                try
                {
                    return input[charCount += n];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IllegalInputException("Malformed input.");
                }
            };
            int amount = 1;
            char ch;
            ch = input[charCount];

            switch (ch)
            {
                case 'S':
                    charCount++;
                    stoneType = new Flatstone(board.Turn);
                    stoneType.Standing = true;
                    break;
                case 'C':
                    charCount++;
                    stoneType = new Capstone(board.Turn);
                    break;
                default:
                    stoneType = new Flatstone(board.Turn);
                    break;
            }
            ch = input[charCount];
            if ((ch > 'z') || (ch < 'a'))
            {
                if (ch > '0')
                {
                    amount = ch - '0';
                    ch = increment();
                }
                else
                {
                    //exception
                    throw new IllegalInputException("Illegal character given, please specify a lowercase letter for the x-axis");
                }
            }
            x = ch - 'a'; // [A..Z] -> [0..25]
            ch = increment();
            y = ch - '0' - 1; //  [1..X] -> [0..X-1]
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
                String stackPlace;
                StoneStack stack;
                int[] move = new int[1];
                if (input.Length > 3)
                {
                    if (input.Length - charCount < 3)
                        throw new IllegalInputException("Malformed pick-up/put-down format.");

                    ch = increment(2);
                    stackPlace = input.Substring(charCount);
                    move = new int[stackPlace.Length];
                    int amountCh = 0;

                    for (int i = 0; i < stackPlace.Length; i++)
                    {
                        int num = stackPlace[i] - '0';
                        move[i] = num;
                        amountCh += num;
                    }
                    if (amountCh != amount)
                    {
                        throw new IllegalInputException("Amount specified does not correlate with amount put down");
                    }
                    stack = board.PickUpStack(x, y, amount);
                }
                else
                {
                    move[0] = 1;
                    stack = board.PickUpStack(x, y);
                }
                if (ml)
                {
                    foreach (var n in move)
                    {
                        x--;
                        for (int j = 0; j < n; j++)
                        {
                            board.PlaceStone(x, y, stack.PopStone(), true);
                        }
                    }
                }
                else if (mr)
                {
                    foreach (var n in move)
                    {
                        x++;
                        for (int j = 0; j < n; j++)
                        {
                            board.PlaceStone(x, y, stack.PopStone(), true);
                        }
                    }
                }
                else if (mu)
                {
                    foreach (var n in move)
                    {
                        y++;
                        for (int j = 0; j < n; j++)
                        {
                            board.PlaceStone(x, y, stack.PopStone(), true);
                        }
                    }
                }
                else
                {
                    foreach (var n in move)
                    {
                        y--;
                        for (int j = 0; j < n; j++)
                        {
                            board.PlaceStone(x, y, stack.PopStone(), true);
                        }
                    }
                }
            }
            else
            {
                if (input.Length > 3 || (input.Length > 2 && !stoneType.Standing) || input.Length < 2)
                {
                    //exception
                    throw new IllegalInputException("Illegal input format for placing new stones");
                }
                board.PlaceStone(x, y, stoneType);
            }
        }
    }
}
