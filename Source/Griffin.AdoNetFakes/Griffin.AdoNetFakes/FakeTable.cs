using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Griffin.AdoNetFakes;

/// <summary>
///     Makes it easier to initialize a new data table
/// </summary>
/// <remarks>DataTables are used to represent the data which are fetched from the DB.</remarks>
public class FakeTable : DataTable
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FakeTable" /> class.
    /// </summary>
    /// <remarks>
    ///     For descendant class use only
    /// </remarks>
    internal FakeTable()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FakeTable" /> class.
    /// </summary>
    /// <param name="rows">The actual result. Each item represents a row with it's column values.</param>
    /// <param name="columnNames">Defines all columns for the table</param>
    public FakeTable(IEnumerable<object[]> rows, IList<string> columnNames)
        : this(rows, BuildDataColumnsFromSampleRowAndNames(rows.FirstOrDefault(), columnNames))
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FakeTable" /> class.
    /// </summary>
    /// <param name="rows">The actual result. Each item represents a row with it's column values.</param>
    public FakeTable(IEnumerable<object[]> rows)
        : this(rows, BuildDataColumnsFromSampleRow(rows.FirstOrDefault()))
    {
    }

    public FakeTable(IEnumerable<object[]> rows, IList<DataColumn> columns)
    {
        if (rows == null)
        {
            throw new ArgumentNullException(nameof(rows));
        }

        if (columns == null)
        {
            throw new ArgumentNullException(nameof(columns));
        }

        Columns.AddRange(columns.ToArray());

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

    private static IList<DataColumn> BuildDataColumnsFromSampleRow(object[] row)
    {
        var columnNames = row
            .Select((c, i) => GetColumnName(null, i))
            .ToArray();

        return BuildDataColumnsFromSampleRowAndNames(row, columnNames);
    }

    private static IList<DataColumn> BuildDataColumnsFromSampleRowAndNames(object[] row, IList<string> columnNames)
    {
        var columns = new List<DataColumn>();

        if (row != null)
        {
            columns.AddRange(
                row.Select((t, i) => new DataColumn(GetColumnName(columnNames, i), GetObjectType(t)))
            );
        }

        return columns;
    }

    private static string GetColumnName(IList<string> names, int index)
    {
        return index >= 0 && names != null && index < names.Count
            ? names[index]
            : $"Col{index + 1}";
    }

    private static Type GetObjectType(object obj)
    {
        return obj?.GetType() ?? typeof(string);
    }
}
