using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    public class Flatstone : Stone
    {
        public override bool Standing { set { standing = value; } }

        public Flatstone(Colour c) : base(c, false) { }
    }
}
