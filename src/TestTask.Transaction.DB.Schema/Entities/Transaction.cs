namespace TestTask.Transaction.DB.Schema.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string ClientName { get; set; }
        public decimal Amount { get; set; }
        
    }
}
