using System;
using System.Collections.Generic;
using System.Text;

namespace TestTask.Transaction.Common.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}
