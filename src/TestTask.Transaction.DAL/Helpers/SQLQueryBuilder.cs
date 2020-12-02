namespace TestTask.Transaction.DAL.Helpers
{
    public class QueryRule
    {
        public string Value { get; set; }
        public string Key { get; set; }
    }

    public static class SQLQueryBuilder
    {
        public static string BuildInsertQuery(string query)
        {
            var insertIndex = query.ToLower().IndexOf("values");
            return query.Insert(insertIndex, " OUTPUT INSERTED.[ID] ");
        }

        public static string BuildDeleteQuery(string tableName)
        {
            return $"DELETE FROM [dbo].[{tableName}] WHERE ID = @id";
        }
    }
}
