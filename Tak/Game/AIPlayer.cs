using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    class AIPlayer : Player
    {
        public AIPlayer(Colour c, int nCap, int nFlat) : base(c, nCap, nFlat) { }

        public override void DoMove()
        {
            throw new NotImplementedException();
        }
    }
}
