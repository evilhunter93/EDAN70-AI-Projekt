using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Game;
using Tak.Exceptions;

namespace Tak.GUI
{
    public class ASCIIGUI
    {
        private const int STONE_SYM_LENGTH = 2; // Number of characters in a stone symbol
        private const int SQUARE_SIZE = 4;      // Number of rows and columns in one square (determines how many stone symbols fit in one square)

        private const char COL_SEPARATOR_SYM = '|';
        private const char ROW_SEPARATOR_SYM = '-';
        private const char BLANK_SYM = ' ';

        private GameBoard gameBoard;
        private int size;
        private string blank;
        private string rowSeparator;
        private string colSeparator;

        public ASCIIGUI(GameBoard gameBoard, int size)
        {
            this.gameBoard = gameBoard;
            this.size = size;
            blank = new string(BLANK_SYM, STONE_SYM_LENGTH);
            colSeparator = new string(COL_SEPARATOR_SYM, 1);
            rowSeparator = new string(ROW_SEPARATOR_SYM, SQUARE_SIZE * size * (STONE_SYM_LENGTH + 1) + (size - 1) * (colSeparator.Length + 1));

        }

        public void Draw()
        {
            string text = "";
            StoneStack[,] stacks = gameBoard.Stacks;
            Stone s;

            for (int row = 0; row < size; row++)
            {
                for (int squareRow = 0; squareRow < SQUARE_SIZE; squareRow++)
                {
                    for (int col = 0; col < size; col++)
                    {
                        for (int squareCol = 0; squareCol < SQUARE_SIZE; squareCol++)
                        {
                            if (stacks[row, col].Count > 0)
                            {
                                s = stacks[row, col].PopStone();
                                text += StoneSymbol(s);
                                if (squareRow == SQUARE_SIZE - 1 && squareCol == SQUARE_SIZE - 1 && stacks[row, col].Count > 0)
                                    text += "+";
                                else
                                    text += " ";
                            }
                            else
                            {
                                text += blank + " ";
                            }
                        }
                        if (col < size - 1)
                            text += colSeparator + " ";
                    }
                    text += "\n";
                }
                if (row < size - 1)
                    text += rowSeparator;
                text += "\n";
            }

            Console.Clear();
            Console.WriteLine(text);
        }

        public void Write(string gameOverText)
        {
            Console.WriteLine(gameOverText);
        }

        private string StoneSymbol(Stone s)
        {
            string symbol = "";

            if (s.Colour == Colour.White)
                symbol += "W";
            else if (s.Colour == Colour.Black)
                symbol += "B";
            else
                throw new TakException("Colour not recognized.");

            if (s is Flatstone)
                if (s.Standing)
                    symbol += "S";
                else
                    symbol += "F";
            else if (s is Capstone)
                symbol += "C";
            else
                throw new TakException("Stone not recognized.");

            return symbol;
        }
    }
}
