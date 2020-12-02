using System;
using System.Collections.Generic;
using System.Text;

namespace TestTask.Transaction.Common.Exceptions
{
    [Serializable]
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message)
           : base(message)
        {
        }
    }
}
