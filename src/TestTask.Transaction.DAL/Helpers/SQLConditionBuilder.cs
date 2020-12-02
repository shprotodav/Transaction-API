using System;

namespace TestTask.Transaction.DAL.Helpers
{
    public static class SQLConditionBuilder
    {
        public static string Build<T>(T[] items, Func<T, string> template, string separatorCondition)
        {
            var condition = "";
            for (var i = 0; i < items.Length; i++)
            {
                var map = items[i];
                condition += template(map);
                if (i != items.Length - 1)
                {
                    condition += $" {separatorCondition} ";
                }
            }
            return condition;
        }
    }
}
