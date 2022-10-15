using System;
using System.Data;
using Griffin.AdoNetFakes.Tests.SimpleData;
using Shouldly;
using Xunit;

namespace Griffin.AdoNetFakes.Tests;

public class FakeDataReaderTests
{
    [Fact]
    public void Construct_With_Rows_And_Columns_leaves_SchemaType_as_default_RowData()
    {
        var rows = new[] {new object[] {1, "Dave", DateTime.UtcNow}};

        var table = new FakeTable(rows);

        var sut = new FakeDataReader(table);

        var rowCount = 0;
        while (sut.Read())
        {
            ++rowCount;
        }

        rowCount.ShouldBeGreaterThan(0);
        rowCount.ShouldBe(rows.Length);
    }

    [Fact]
    public void Construct_With_DataTable_leaves_SchemaType_as_default_RowData()
    {
        var table = new DataTable();
        table.Columns.Add(new DataColumn(nameof(SimpleObject.Id)));
        table.Columns.Add(new DataColumn(nameof(SimpleObject.Name)));
        table.Columns.Add(new DataColumn(nameof(SimpleObject.DateOfBirth)));
        table.Rows.Add(1, "Bob", new DateTime(1980, 06, 15));

        var sut = new FakeDataReader(table);

        var rowCount = 0;
        while (sut.Read())
        {
            ++rowCount;
        }

        rowCount.ShouldBeGreaterThan(0);
        rowCount.ShouldBe(table.Rows.Count);
    }

    [Fact]
    public void Construct_With_FakeTable_sets_SchemaType_to_DataTable()
    {
        var table = new FakeTable<SimpleObject>();
        table.AddRow(new SimpleObject
        {
            Id = 1,
            Name = "Bob",
            DateOfBirth = new DateTime(1980, 06, 15)
        });

        var sut = new FakeDataReader(table);

        var rowCount = 0;
        while (sut.Read())
        {
            ++rowCount;
        }

        rowCount.ShouldBeGreaterThan(0);
        rowCount.ShouldBe(table.Rows.Count);
    }
}
