using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    abstract class Player
    {
        int nbrCapStones;
        int nbrFlatStones;

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
