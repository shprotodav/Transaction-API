using TestTask.Transaction.Common.DTOs.Base;

namespace TestTask.Transaction.Common.DTOs
{
    public class TransactionDTO : BaseItemDTO
    {
        public override long TransactionId { get; set; } // 'override' - for MERGE procedure!!
        public string Status { get; set; }
        public string Type { get; set; }
        public string ClientName { get; set; }
        public decimal Amount { get; set; }

    }
}
