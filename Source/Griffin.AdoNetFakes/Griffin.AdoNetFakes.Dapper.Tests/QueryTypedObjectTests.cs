using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Griffin.AdoNetFakes.Dapper.Tests.Data;
using Should;
using Xunit;

namespace Griffin.AdoNetFakes.Dapper.Tests
{
    public class QueryTypedObjectTests
    {
        private FakeConnection Connection { get; }

        public static Person Person1 = new Person { Id = 1, Name = "Bob Robertson", DateOfBirth = DateTime.Parse("1980-06-15") };
        public static Person Person2 = new Person { Id = 2, Name = "Dave Dangerous", DateOfBirth = DateTime.Parse("1990-04-21") };

        public static Address Address1 = new Address { Id = 1, Type = AddressType.Home, Line1 = "3 Magnolia Lane" };
        public static Address Address2 = new Address { Id = 2, Type = AddressType.Home, Line1 = "72 Wisteria Cottage" };
        public static Address Address3 = new Address { Id = 3, Type = AddressType.Business, Line1 = "22b Baker Street" };

        public static IList<Person> PersonList = new[]
        {
            Person1,
            Person2
        };

        public static IList<Address> AddressesList = new[]
        {
            Address1,
            Address2,
            Address3
        };

        public QueryTypedObjectTests()
        {
            Connection = new FakeConnection();
            Connection.Open();
        }

        [Fact]
        public void QueryFirst_returns_results()
        {
            // Arrange
            var rows = PersonList.Where(p => p.Id == 1);
            var table = new FakeTable<Person>(rows);

            var readerResult = new ReaderCommandResult
            {
                Result = new FakeDataReader(table)
            };

            var command = new FakeCommand(readerResult);

            Connection.Setup(command);

            // Act
            var result = Connection.QueryFirst<Person>("SELECT TOP 1 * FROM [person] WHERE [Id] = {0}", 1); //.ToList();

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(Person1.Id);
            result.Name.ShouldEqual(Person1.Name);
            result.DateOfBirth.ShouldEqual(Person1.DateOfBirth);
        }

        [Fact]
        public void Query_returns_results()
        {
            // Arrange
            var rows = PersonList;
            var table = new FakeTable<Person>(rows);

            var readerResult = new ReaderCommandResult
            {
                Result = new FakeDataReader(table)
            };

            var command = new FakeCommand(readerResult);

            Connection.Setup(command);

            // Act
            var results = Connection.Query<Person>("SELECT TOP 1 * FROM [person] WHERE [Id] = {0}", 1)
                .ToList();

            // Assert
            results.ShouldNotBeNull();
            results.Count.ShouldEqual(rows.Count);
            results.Skip(0).First().Id.ShouldEqual(rows.Skip(0).First().Id);
            results.Skip(0).First().Name.ShouldEqual(rows.Skip(0).First().Name);
            results.Skip(0).First().DateOfBirth.ShouldEqual(rows.Skip(0).First().DateOfBirth);
            results.Skip(1).First().Id.ShouldEqual(rows.Skip(1).First().Id);
            results.Skip(1).First().Name.ShouldEqual(rows.Skip(1).First().Name);
            results.Skip(1).First().DateOfBirth.ShouldEqual(rows.Skip(1).First().DateOfBirth);
        }

        [Fact]
        public void QueryMultiple_returns_results()
        {
            // Arrange
            var personTable = new FakeTable<Person>(PersonList);
            var addressesTable = new FakeTable<Address>(AddressesList);;

            var queryResult = new ReaderCommandResult()
            {
                Result = new FakeDataReader(
                    new DataTable[]
                    {
                        personTable,
                        addressesTable
                    })
            };

            var command = new FakeCommand(new [] { queryResult });

            Connection.Setup(command);

            // Act
            var results = Connection.QueryMultiple("[usp_SomeStoredProcedure] {0}", 1);
            var person = results.ReadFirst<Person>();
            var addresses = results.Read<Address>().ToList();

            // Assert
            person.ShouldNotBeNull();
            addresses.ShouldNotBeNull().ShouldNotBeEmpty();
            addresses.Count.ShouldEqual(addressesTable.Rows.Count);

            person.Id.ShouldEqual(Person1.Id);
            person.Name.ShouldEqual(Person1.Name);
            person.DateOfBirth.ShouldEqual(Person1.DateOfBirth);

            addresses[0].Id.ShouldEqual(Address1.Id);
            addresses[0].Type.ShouldEqual(Address1.Type);
            addresses[0].Line1.ShouldEqual(Address1.Line1);
            addresses[0].Line2.ShouldEqual(Address1.Line2);
            addresses[0].Line3.ShouldEqual(Address1.Line3);

            addresses[1].Id.ShouldEqual(Address2.Id);
            addresses[1].Type.ShouldEqual(Address2.Type);
            addresses[1].Line1.ShouldEqual(Address2.Line1);
            addresses[1].Line2.ShouldEqual(Address2.Line2);
            addresses[1].Line3.ShouldEqual(Address2.Line3);

            addresses[2].Id.ShouldEqual(Address3.Id);
            addresses[2].Type.ShouldEqual(Address3.Type);
            addresses[2].Line1.ShouldEqual(Address3.Line1);
            addresses[2].Line2.ShouldEqual(Address3.Line2);
            addresses[2].Line3.ShouldEqual(Address3.Line3);
        }
    }
}
