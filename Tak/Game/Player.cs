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

        public Colour Colour { get { return colour; } }

        protected Player(Colour colour)
        {
            this.colour = colour;
        }

        public abstract void DoMove();
    }
}
