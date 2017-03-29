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
        private Colour colour;

        public virtual void doMove()
        {
            // Default?
        }

        public int getNbrCapStones()
        {
            return nbrCapStones;
        }

        public int getNbrFlatStones()
        {
            return nbrFlatStones;
        }
    }
}
