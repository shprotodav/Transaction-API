using System;
using System.Collections.Generic;
using System.Text;

namespace TestTask.Transaction.Common.Exceptions
{
    [Serializable]
    public class IdentityDBException : Exception
    {
        public IdentityDBException(string message)
            : base(message)
        {
        }
    }
}
