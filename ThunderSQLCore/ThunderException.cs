using System;
using System.Collections.Generic;
using System.Text;

namespace ThunderSQLCore
{
    public class ThunderException : Exception
    {
        public ThunderException(string message) : base(message) { }

        public ThunderException(string message, Exception inner) : base(message, inner) { }
    }
}
