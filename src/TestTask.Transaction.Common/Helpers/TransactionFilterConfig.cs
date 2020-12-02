using System.Collections.Generic;
using TestTask.Transaction.Common.DTOs;

namespace TestTask.Transaction.Common.Helpers
{
    public class ColumnConfig
    {
        public bool IsVisible { get; set; }
        public string XlLetter { get; set; }
        public string Value { get; set; }
    }


    public class TransactionFilterConfig
    {
        public ColumnConfig IdColumn { get; set; }
        public ColumnConfig StatusColumn { get; set; }
        public ColumnConfig TypeColumn { get; set; }
        public ColumnConfig ClientNameColumn { get; set; }
        public ColumnConfig AmountColumn { get; set; }

        public TransactionFilterConfig(bool isIdColumn, bool isStatusColumn, bool isTypeColumn,
                                       bool isClientNameColumn, bool isAmountColumn,
                                       string status, string type, string clientName)
        {
            this.IdColumn = new ColumnConfig { IsVisible = isIdColumn };
            this.StatusColumn = new ColumnConfig { IsVisible = isStatusColumn, Value = status };
            this.TypeColumn = new ColumnConfig { IsVisible = isTypeColumn, Value = type };
            this.ClientNameColumn = new ColumnConfig { IsVisible = isClientNameColumn, Value = clientName };
            this.AmountColumn = new ColumnConfig { IsVisible = isAmountColumn };
        }

        public void XlLetterGenerate()
        {
            int letterIndex = 0;
            string[] xlLetters = new string[] { "A", "B", "C", "D", "E" };

            if (IdColumn.IsVisible) IdColumn.XlLetter = xlLetters[letterIndex++];
            if (StatusColumn.IsVisible) StatusColumn.XlLetter = xlLetters[letterIndex++];
            if (TypeColumn.IsVisible) TypeColumn.XlLetter = xlLetters[letterIndex++];
            if (ClientNameColumn.IsVisible) ClientNameColumn.XlLetter = xlLetters[letterIndex++];
            if (AmountColumn.IsVisible) AmountColumn.XlLetter = xlLetters[letterIndex++];
        }
    }
}
