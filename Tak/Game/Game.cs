﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    class Game
    {
        GameBoard board;
        Player p1;
        Player p2;

        public void initialize(int size, String p1, String p2)
        {
            board = new GameBoard(size);
            if (p1 == "Human")
            {
                this.p1 = new HumanPlayer(Colour.White);
            }
            else
            {
                this.p1 = new AIPlayer(Colour.White);
            }
            if (p2 == "Human")
            {
                this.p2 = new HumanPlayer(Colour.Black);
            }
            else
            {
                this.p2 = new AIPlayer(Colour.Black);
            }
        }

    }
}
