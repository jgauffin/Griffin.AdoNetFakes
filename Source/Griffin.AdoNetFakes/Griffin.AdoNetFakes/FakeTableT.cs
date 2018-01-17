using System.Linq;
using System.Reflection;

namespace Griffin.AdoNetFakes
{
    public class FakeTable<T> : FakeTable
    {
        public static BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty;

        public FakeTable()
            : this(DefaultBindingFlags)
        {
        }

        public FakeTable(BindingFlags bindingFlags)
        {
            var properties = typeof(T).GetProperties(bindingFlags);

            properties
                .ToList()
                .ForEach(pi => Columns.Add(pi.Name, pi.PropertyType));
        }

        public void AddRow(T instance)
        {
            AddRow(instance, DefaultBindingFlags);
        }

        public void AddRow(T instance, BindingFlags bindingFlags)
        {
            var properties = typeof(T).GetProperties(bindingFlags);

            var row = NewRow();

            for (var i=0; i < Columns.Count; ++i)
            {
                var columnName = Columns[i].ColumnName;
                var propertyInfo = properties.FirstOrDefault(pi => pi.Name.Equals(columnName));

                var value = propertyInfo == null
                    ? null
                    : propertyInfo.GetValue(instance, null);

                row[i] = value;
            }

            Rows.Add(row);
        }
    }
}
