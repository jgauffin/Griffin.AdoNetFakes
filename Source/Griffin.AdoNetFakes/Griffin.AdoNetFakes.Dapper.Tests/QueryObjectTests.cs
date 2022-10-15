using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Griffin.AdoNetFakes.Dapper.Tests.Data;
using Should;
using Xunit;

namespace Griffin.AdoNetFakes.Dapper.Tests;

public class QueryObjectTests
{
    public static Person Person1 = new()
    {
        Id = 1,
        Name = "Bob Robertson",
        DateOfBirth = DateTime.Parse("1980-06-15")
    };

    public static Person Person2 = new()
    {
        Id = 2,
        Name = "Dave Dangerous",
        DateOfBirth = DateTime.Parse("1990-04-21")
    };

    public static Address Address1 = new()
    {
        Id = 1,
        Type = AddressType.Home,
        Line1 = "3 Magnolia Lane"
    };

    public static Address Address2 = new()
    {
        Id = 2,
        Type = AddressType.Home,
        Line1 = "72 Wisteria Cottage"
    };

    public static Address Address3 = new()
    {
        Id = 3,
        Type = AddressType.Business,
        Line1 = "22b Baker Street"
    };

    public static IList<object[]> PersonList = new List<object[]>
    {
        new object[] {Person1.Id, Person1.Name, Person1.DateOfBirth},
        new object[] {Person2.Id, Person2.Name, Person2.DateOfBirth}
    };

    public static IList<string> PersonColumns = new[]
    {
        nameof(Person.Id), nameof(Person.Name), nameof(Person.DateOfBirth)
    };

    public static IList<object[]> AddressesList = new List<object[]>
    {
        new object[] {Address1.Id, Address1.Type, Address1.Line1, Address1.Line2, Address1.Line3},
        new object[] {Address2.Id, Address2.Type, Address2.Line1, Address2.Line2, Address2.Line3},
        new object[] {Address3.Id, Address3.Type, Address3.Line1, Address3.Line2, Address3.Line3}
    };

    public static IList<string> AddressColumns = new[]
    {
        nameof(Address.Id), nameof(Address.Type), nameof(Address.Line1), nameof(Address.Line2),
        nameof(Address.Line3)
    };

    public QueryObjectTests()
    {
        Connection = new FakeConnection();
        Connection.Open();
    }

    private FakeConnection Connection { get; }

    [Fact]
    public void QueryFirst_returns_results()
    {
        // Arrange
        var rows = PersonList.Where(x => x.First().Equals(1)).ToList();
        var table = new FakeTable(rows, PersonColumns);

        var readerResult = new ReaderCommandResult {Result = new FakeDataReader(table)};

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
    public async Task QueryFirstAsync_returns_results()
    {
        // Arrange
        var rows = PersonList.Where(x => x.First().Equals(1)).ToList();
        var table = new FakeTable(rows, PersonColumns);

        var readerResult = new ReaderCommandResult { Result = new FakeDataReader(table) };

        var command = new FakeCommand(readerResult);

        Connection.Setup(command);

        // Act
        var result = await Connection.QueryFirstAsync<Person>("SELECT TOP 1 * FROM [person] WHERE [Id] = {0}", 1); //.ToList();

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
        var table = new FakeTable(rows, PersonColumns);

        var readerResult = new ReaderCommandResult {Result = new FakeDataReader(table)};

        var command = new FakeCommand(readerResult);

        Connection.Setup(command);

        // Act
        var results = Connection.Query<Person>("SELECT TOP 1 * FROM [person] WHERE [Id] = {0}", 1)
            .ToList();

        // Assert
        results.ShouldNotBeNull();
        results.Count.ShouldEqual(rows.Count);
        results.Skip(0).First().Id.ShouldEqual(rows.Skip(0).First().First());
        results.Skip(0).First().Name.ShouldEqual(rows.Skip(0).First().Skip(1).First());
        results.Skip(0).First().DateOfBirth.ShouldEqual(rows.Skip(0).First().Skip(2).First());
        results.Skip(1).First().Id.ShouldEqual(rows.Skip(1).First().First());
        results.Skip(1).First().Name.ShouldEqual(rows.Skip(1).First().Skip(1).First());
        results.Skip(1).First().DateOfBirth.ShouldEqual(rows.Skip(1).First().Skip(2).First());
    }

    [Fact]
    public void QueryMultiple_returns_results()
    {
        // Arrange
        var personTable = new FakeTable(PersonList, PersonColumns);
        var addressesTable = new FakeTable(AddressesList, AddressColumns);
        ;

        var queryResult = new ReaderCommandResult
        {
            Result = new FakeDataReader(
                new DataTable[] {personTable, addressesTable})
        };

        var command = new FakeCommand(new[] {queryResult});

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
