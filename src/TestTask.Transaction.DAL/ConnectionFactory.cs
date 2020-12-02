using Microsoft.Extensions.Options;
using System;
using System.Data.SqlClient;
using TestTask.Transaction.Common;

namespace TestTask.Transaction.DAL
{
    public interface IConnectionFactory
	{
		SqlConnection GetOpenedConnection();
	}

	public class ConnectionFactory : IConnectionFactory
	{
		private readonly string _connectionString;

		public ConnectionFactory(IOptions<AppSettings> appSettings)
		{
			_connectionString = appSettings.Value.ConnectionString;
		}

		public SqlConnection GetOpenedConnection()
		{
			Func<string, SqlConnection> getConnection = x =>
			{
				switch (x)
				{
					case "Dev": return new SqlConnection(_connectionString);
					default:
						return new SqlConnection(_connectionString);
				}
			};

			var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			var connection = getConnection(env);
			connection.Open();
			return connection;
		}


	}
}
