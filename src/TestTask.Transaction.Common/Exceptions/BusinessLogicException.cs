using System;
using System.Collections.Generic;
using System.Text;

namespace TestTask.Transaction.Common.Exceptions
{
    [Serializable]
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException(string message)
            : base(message)
        {
        }
    }
}
