using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    public abstract class Stone : IEquatable<Stone>
    {
        private Colour colour;
        protected bool standing;

        public Colour Colour { get { return colour; } }

        public virtual bool Standing
        {
            get { return standing; }
            set { /* FIXME: don't want set here */ }
        }

        protected Stone(Colour colour, bool standing)
        {
            this.colour = colour;
            this.standing = standing;
        }

        public abstract bool Equals(Stone other);
    }
}
