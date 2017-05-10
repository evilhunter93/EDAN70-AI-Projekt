using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Colour
{
    Black, White
}

namespace Tak.Game
{
    abstract class Player
    {
        protected GameBoard board;
        protected Colour colour;
        protected string colourText;

        public Colour Colour { get { return colour; } }

        protected Player(GameBoard board, Colour colour)
        {
            this.board = board;
            this.colour = colour;
            colourText = (colour == Colour.White) ? "White" : "Black";
        }

        public abstract void DoMove();
    }
}
