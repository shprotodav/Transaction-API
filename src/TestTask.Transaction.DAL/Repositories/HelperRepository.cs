using Dapper;
using System.Threading.Tasks;
using TestTask.Transaction.Common.DTOs.Base;
using TestTask.Transaction.Common.Exceptions;
using TestTask.Transaction.DAL.Helpers;

namespace TestTask.Transaction.DAL.Repositories
{
    public interface IHelperRepository
    {
        Task<T> InsertAsync<T>(string query, T model) where T : IIdentifier;
        Task DeleteAsync(long id, string tableName);
    }

    public class HelperRepository : IHelperRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public HelperRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<T> InsertAsync<T>(string query, T model)
           where T : IIdentifier
        {
            var editedQuery = SQLQueryBuilder.BuildInsertQuery(query);

            using (var connection = _connectionFactory.GetOpenedConnection())
            {
                model.TransactionId = await connection.QuerySingleAsync<long>(editedQuery, model);
                return model;
            }
        }

        public async Task DeleteAsync(long id, string tableName)
        {
            using (var connection = _connectionFactory.GetOpenedConnection())
            {
                var affectedrows = await connection.ExecuteAsync(SQLQueryBuilder.BuildDeleteQuery(tableName), new { id });
                CheckRows(id, tableName, affectedrows);
            }
        }

        private void CheckRows(long id, string tableName, int affectedrows)
        {
            if (affectedrows == 0)
            {
                throw new BusinessLogicException($"Deletion error from table {tableName}. Id = {id}");
            }
        }
    }
}
