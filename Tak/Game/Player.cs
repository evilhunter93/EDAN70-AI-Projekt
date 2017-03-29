using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum Colour
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

        protected Player(Colour colour, int nbrCapStones, int nbrFlatStones)
        {
            this.colour = colour;
            this.nbrCapStones = nbrCapStones;
            this.nbrFlatStones = nbrFlatStones;
        }

        public abstract void DoMove();

        public int GetNbrCapStones()
        {
            return nbrCapStones;
        }

        public int GetNbrFlatStones()
        {
            return nbrFlatStones;
        }
    }
}
