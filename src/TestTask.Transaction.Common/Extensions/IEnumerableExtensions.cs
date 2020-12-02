using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace TestTask.Transaction.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> records, params string[] ignoredProperties)
        {
            DataTable result = new DataTable();
            PropertyDescriptorCollection propertyDescriptors = TypeDescriptor.GetProperties(typeof(T));

            foreach (PropertyDescriptor propertyDescriptor in propertyDescriptors)
            {
                Type propertyType = propertyDescriptor.PropertyType;

                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    propertyType = Nullable.GetUnderlyingType(propertyType);

                result.Columns.Add(propertyDescriptor.Name, propertyType);
            }

            object[] values = new object[propertyDescriptors.Count];

            foreach (T record in records)
            {
                for (int i = 0; i != propertyDescriptors.Count; i++)
                    values[i] = propertyDescriptors[i].GetValue(record);

                result.Rows.Add(values);
            }
            return result;
        }
    }
}
