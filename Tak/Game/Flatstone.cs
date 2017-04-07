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

        public override bool Equals(object obj)
        {
            var other = obj as Stone;
            if (other == null)
                return false;

            return Equals(other);
        }

        public override bool Equals(Stone other)
        {
            if (!(other is Flatstone))
                return false;

            if (Colour != other.Colour)
                return false;

            if (Standing == other.Standing)
                return true;

            return false;
        }
    }
}
