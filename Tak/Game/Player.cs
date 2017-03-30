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
        int nbrCapStones;
        int nbrFlatStones;
        Colour colour;

        public Colour Colour { get { return colour; } }
        public int NbrCapStones { get { return nbrCapStones; } }
        public int NbrFlatStones { get { return nbrFlatStones; } }

        protected Player(Colour colour, int nbrCapStones, int nbrFlatStones)
        {
            this.colour = colour;
            this.nbrCapStones = nbrCapStones;
            this.nbrFlatStones = nbrFlatStones;
        }

        public abstract void DoMove();
    }
}
