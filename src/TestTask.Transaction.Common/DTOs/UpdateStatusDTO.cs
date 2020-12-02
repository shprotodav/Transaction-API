using System;
using System.Collections.Generic;
using System.Text;
using TestTask.Transaction.Common.DTOs.Base;

namespace TestTask.Transaction.Common.DTOs
{
    public class UpdateStatusDTO : BaseItemDTO
    {
        public string Status { get; set; }
    }
}
