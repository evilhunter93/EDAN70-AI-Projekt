﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Exceptions
{
    class IllegalInputException : TakException
    {
        public IllegalInputException(string message) : base(message) { }
    }
}
