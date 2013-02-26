using System.Collections.Generic;
using System.Data;

namespace Griffin.AdoNetFakes
{
    /// <summary>
    ///     Makes it easier to initialize a new data table
    /// </summary>
    /// <remarks>DataTables are used to represent the data which are fetched from the DB.</remarks>
    public class FakeTable : DataTable
    {
        public FakeTable()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="columns">Defines all columns that are returned</param>
        /// <param name="rows">The actual result. Each item represents a row with it's column values.</param>
        public FakeTable(IEnumerable<string> columns, IEnumerable<object[]> rows)
        {
            foreach (var column in columns)
            {
                Columns.Add(column);
            }

            foreach (var row in rows)
            {
                var r = NewRow();
                for (var i = 0; i < Columns.Count; i++)
                {
                    r[i] = row[i];
                }
                Rows.Add(r);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="rows">The actual result. Each item represents a row with it's column values.</param>
        public FakeTable(IEnumerable<object[]> rows)
        {
            var created = false;
            foreach (var row in rows)
            {
                if (!created)
                {
                    for (int i = 0; i < row.Length; i++)
                    {
                        Columns.Add("Col" + (i+1), row[i].GetType());
                    }
                    created = true;
                }

                var r = NewRow();
                for (var i = 0; i < Columns.Count; i++)
                {
                    r[i] = row[i];
                }
                Rows.Add(r);
            }
        }
    }
}