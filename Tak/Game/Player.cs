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
        Colour colour;
        Interpreter interpreter;

        public Colour Colour { get { return colour; } }

        protected Player(Colour colour, Interpreter interpreter)
        {
            this.colour = colour;
            this.interpreter = interpreter;
        }

        public abstract void DoMove();
    }
}
