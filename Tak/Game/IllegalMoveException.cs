using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    class IllegalMoveException : Exception
    {
        public IllegalMoveException(string message) : base(message) { }
    }
}
