using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    public abstract class Stone
    {
        private bool standing;

        public abstract bool Standing { get; set; }
    }
}
