using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TestTask.Transaction.Common.DTOs;

namespace TestTask.Transaction.Common.Helpers
{
    public class TransactionUnformatted
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string ClientName { get; set; }
        public string Amount { get; set; }

        public TransactionDTO ToDTO()
        {
            decimal tmp;
            Decimal.TryParse(this.Amount, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, CultureInfo.CreateSpecificCulture("en-US"), out tmp);

            return (new TransactionDTO
            {
                TransactionId = Convert.ToInt64(this.TransactionId),
                Status = this.Status,
                Type = this.Type,
                ClientName = this.ClientName,
                Amount = tmp
            });
        }
        
    }
}
