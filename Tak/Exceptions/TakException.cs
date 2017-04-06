using System;

namespace Tak.Exceptions
{
    public class TakException : Exception
    {
        public TakException(string message) : base(message) { }
    }
}
