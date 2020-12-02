using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TestTask.Transaction.Common.Exceptions;

namespace TestTask.Transaction.DAL.Helpers
{
    public static class DapperExtension
    {
        public static async Task<IEnumerable<TReturn>> QueryNotEmptyAsync<TFirst, TSecond, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TReturn> map, object param = null)
        {
            var data = await cnn.QueryAsync(sql, map, param);

            CheckData(data);

            return data;
        }

        public static async Task<IEnumerable<TReturn>> QueryDefaultEmptyAsync<TFirst, TSecond, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TReturn> map, object param = null)
        {
            var data = await cnn.QueryAsync(sql, map, param);

            if (data.AsList().Count == 0)
            {
                return new List<TReturn>();
            }

            return data;
        }

        public static async Task<T[]> QueryArrayAsync<T>(this IDbConnection cnn, string sql, object param = null)
        {
            return (await cnn.QueryAsync<T>(sql, param)).ToArray();
        }

        public static async Task<List<T>> QueryListAsync<T>(this IDbConnection cnn, string sql, object param = null)
        {
            return (await cnn.QueryAsync<T>(sql, param)).ToList();
        }

        public static IEnumerable<T> QueryNotEmpty<T>(this IDbConnection cnn, string sql, object param = null)
        {
            var data = cnn.Query<T>(sql, param);

            CheckData(data);

            return data;
        }

        private static void CheckData<T>(IEnumerable<T> data)
        {
            if (data.AsList().Count == 0)
            {
                throw new NotFoundException("Nothig was found");
            }
        }
    }

}
