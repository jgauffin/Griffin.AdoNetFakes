using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Griffin.AdoNetFakes
{
    public static class DataTableExtensions
    {
        /// <summary>
        /// Convert a collection of T items to a DataTable
        /// </summary>
        /// <typeparam name="T">The type to base the DataTable on</typeparam>
        /// <param name="items">The items to populate the DataTable with</param>
        /// <param name="bindingFlags">The BindingFlags to use to determine which Properties to use as columns</param>
        /// <param name="includedPropertyNames">An optional list of Property Names to include as columns</param>
        /// <returns>A DataTable representing T containing the items</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> items,
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty,
            IList<string> includedPropertyNames = null
        )
        {
            var dt = new DataTable();
            dt.Clear();

            var properties = typeof(T).GetProperties(bindingFlags);
            if (includedPropertyNames != null && includedPropertyNames.Any())
            {
                properties = properties
                    .Where(p => includedPropertyNames.Contains(p.Name))
                    .ToArray();
            }

            foreach (var propertyInfo in properties)
            {
                dt.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
            }

            foreach (var item in items)
            {
                var row = dt.NewRow();

                foreach (var propertyInfo in properties)
                {
                    row[propertyInfo.Name] = propertyInfo.GetValue(item);
                }

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
