using System;
using System.Collections.Generic;
using System.Text;

namespace TestTask.Transaction.DAL.Queries
{
    public class TransactionQueries
    {
        public static readonly string GetAll = $@"
            SELECT [TransactionId]
                  ,[Status]
                  ,[Type]
                  ,[ClientName]
                  ,[Amount]
              FROM [dbo].[Transaction]
        ";

        public static readonly string Create = $@"
            insert into [dbo].[Transaction]
		        ([Status])
		        Values (@Status)
        ";

        public static readonly string UpdateStatus = $@"
            UPDATE [dbo].[Transaction]
               SET [Status] = @Status
             WHERE [TransactionId] = @TransactionId
        ";

        public static readonly string DeleteById = $@"
            DELETE FROM [dbo].[Transaction]
                  WHERE [TransactionId] = @transactionId
        ";

        public static readonly string IsExist = $@"SELECT CASE
                                    WHEN EXISTS (
                                        SELECT 1
                                        FROM [dbo].[Transaction]
			                                where TransactionId = @transactionId)
                                    THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT)
                                    END
        ";
    }
}
