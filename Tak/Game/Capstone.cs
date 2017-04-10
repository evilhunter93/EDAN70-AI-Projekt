using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    public class Capstone : Stone
    {
        public Capstone(Colour c) : base(c, true) { }


        public override bool Equals(object obj)
        {
            var other = obj as Stone;
            if (other == null)
                return false;

            return Equals(other);
        }

        public override bool Equals(Stone other)
        {
            if (!(other is Capstone))
                return false;

            if (Colour == other.Colour)
                return true;

            return false;
        }
    }
}