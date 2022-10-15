using System;
using System.Data;
using System.Linq;
using Shouldly;
using Xunit;

namespace Griffin.AdoNetFakes.Tests;

public class FakeTableTests
{
    [Fact]
    public void Construct_With_Rows_defaults_column_names_and_determines_types()
    {
        var rows = new[] {new object[] {1, "Dave", DateTime.UtcNow}};

        var sut = new FakeTable(rows);

        sut.Rows.Count.ShouldBe(rows.Length);
        sut.Columns.Count.ShouldBe(rows.First().Length);

        sut.Columns[0].ColumnName.ShouldBe("Col1");
        sut.Columns[0].DataType.ShouldBe(typeof(int));
        sut.Columns[1].ColumnName.ShouldBe("Col2");
        sut.Columns[1].DataType.ShouldBe(typeof(string));
        sut.Columns[2].ColumnName.ShouldBe("Col3");
        sut.Columns[2].DataType.ShouldBe(typeof(DateTime));

        sut.Rows[0][0].ShouldBe(rows[0][0]);
        sut.Rows[0][1].ShouldBe(rows[0][1]);
        sut.Rows[0][2].ShouldBe(rows[0][2]);
    }

    [Fact]
    public void Construct_With_Rows_and_column_names_uses_column_names_and_determines_types()
    {
        var rows = new[] {new object[] {1, "Dave", DateTime.UtcNow}};

        var columnNames = new[] {"Id", "Name", "DateOfBirth"};

        var sut = new FakeTable(rows, columnNames);

        sut.Rows.Count.ShouldBe(rows.Length);
        sut.Columns.Count.ShouldBe(rows.First().Length);

        sut.Columns[0].ColumnName.ShouldBe(columnNames[0]);
        sut.Columns[0].DataType.ShouldBe(typeof(int));
        sut.Columns[1].ColumnName.ShouldBe(columnNames[1]);
        sut.Columns[1].DataType.ShouldBe(typeof(string));
        sut.Columns[2].ColumnName.ShouldBe(columnNames[2]);
        sut.Columns[2].DataType.ShouldBe(typeof(DateTime));

        sut.Rows[0][0].ShouldBe(rows[0][0]);
        sut.Rows[0][1].ShouldBe(rows[0][1]);
        sut.Rows[0][2].ShouldBe(rows[0][2]);
    }

    /// <summary>
    ///     Constructs the with data columns uses names and types.
    /// </summary>
    [Fact]
    public void Construct_With_DataColumns_uses_names_and_types()
    {
        var rows = new[] {new object[] {1, "Dave", DateTime.UtcNow}};

        var dataColumns = new[]
        {
            new DataColumn("Id", typeof(int)), new DataColumn("Name", typeof(string)),
            new DataColumn("DateOfBirth", typeof(DateTime))
        };

        var sut = new FakeTable(rows, dataColumns);

        sut.Rows.Count.ShouldBe(rows.Length);
        sut.Columns.Count.ShouldBe(rows.First().Length);

        sut.Columns[0].ColumnName.ShouldBe(dataColumns[0].ColumnName);
        sut.Columns[0].DataType.ShouldBe(typeof(int));
        sut.Columns[1].ColumnName.ShouldBe(dataColumns[1].ColumnName);
        sut.Columns[1].DataType.ShouldBe(typeof(string));
        sut.Columns[2].ColumnName.ShouldBe(dataColumns[2].ColumnName);
        sut.Columns[2].DataType.ShouldBe(typeof(DateTime));

        sut.Rows[0][0].ShouldBe(rows[0][0]);
        sut.Rows[0][1].ShouldBe(rows[0][1]);
        sut.Rows[0][2].ShouldBe(rows[0][2]);
    }
}
