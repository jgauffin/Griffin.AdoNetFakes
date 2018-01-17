using System;
using Xunit;

namespace Griffin.AdoNetFakes.Tests
{
    public class FakeTableTTests
    {
        [Fact]
        public void Construct_With_Type_sets_columns()
        {
            var sut = new FakeTable<Person>();

            Assert.Equal(3, sut.Columns.Count);
            Assert.Equal("Id", sut.Columns[0].ColumnName);
            Assert.Equal("Name", sut.Columns[1].ColumnName);
            Assert.Equal("DateOfBirth", sut.Columns[2].ColumnName);
            Assert.Equal(typeof(int), sut.Columns[0].DataType);
            Assert.Equal(typeof(string), sut.Columns[1].DataType);
            Assert.Equal(typeof(DateTime), sut.Columns[2].DataType);
        }

        [Fact]
        public void AddRowFromInstance_add_row_data_Correctly()
        {
            var row1 = new Person {Id = 1, Name = "Bob", DateOfBirth = new DateTime(1980, 06, 15)};

            var sut = new FakeTable<Person>();
            sut.AddRowFromInstance(row1);

            Assert.Equal(1, sut.Rows.Count);
            Assert.Equal(row1.Id, sut.Rows[0]["Id"]);
            Assert.Equal(row1.Name, sut.Rows[0]["Name"]);
            Assert.Equal(row1.DateOfBirth, sut.Rows[0]["DateOfBirth"]);
        }
    }
}
