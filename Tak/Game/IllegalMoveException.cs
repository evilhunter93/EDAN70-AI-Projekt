﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    public class IllegalMoveException : TakException
    {
        public IllegalMoveException(string message) : base(message) { }
    }
}
