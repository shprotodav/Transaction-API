namespace TestTask.Transaction.Common.DTOs.Base
{
    public interface IIdentifier
    {
        public long  TransactionId { get; set; }
    }

    public class BaseItemDTO : IIdentifier
    {
        public virtual long TransactionId { get; set; }
    }
}
