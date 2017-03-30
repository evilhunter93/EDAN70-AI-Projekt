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
            int charCount = 0;
            foreach (var ch in input)
            {
                charCount++;
                if (charCount == 1)
                {
                    switch (ch)
                    {
                        case 'S':
                            stoneType = new Flatstone();
                            stoneType.Standing = true;
                            break;
                        case 'C':
                            stoneType = new Capstone();
                            break;
                        default:
                            charCount = 2;
                            stoneType = new Flatstone();
                            break;
                    }
                }
            }
        }
    }
}
