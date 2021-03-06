This library is a complete ADO.NET driver which is designed to aid you in unit testing.

It do however require that the library/code you are testing have been coded against the abstractions (either `IDbCommand` etc the base classes `DbCommand` etc). For more information about that approach read [this blog post](http://blog.gauffin.org/2013/01/ado-net-the-right-way/).

# Getting started

You generally start by setting up a connection and configure which commands it return. But to allow you to understand how everything is setup. Let's start with the commands.

## Commands

Commands can be used to both return data and to execute CRUD queries. Hence we need a way to both validate the execution and to return results from the commands.

### Returning result

Let's take the easiest part first. We want our command to return a result from execute scalar:

```csharp
var command = new FakeCommand();
command.Setup(new ScalarCommandResult() { Result = "Hello" });
```

That means that "Hello" will be return in:

```csharp
command.CommandText = "Bla bla";
var result = command.ExecuteScalar();
```

For readers the syntax is slightly more complex. We use DataTables to simulate the recordsets from the database:

```csharp
var table = new FakeTable(new[]
	{
		new object[] {1, "jonas"},
		new object[] {2, "arne"},
	});
```

The data types in the recordset are set as the same datatype as in the datatable.

So the actual command will look like:

```csharp
var command = new FakeCommand(table);
```

or

```csharp
var command = FakeCommand();
command.Setup(new ReaderCommandResult {Result = new FakeDataReader(readerResult)});
```

### Validating execution

We can also validate everything which is executed. Here is an example on how we can validate a command:

```csharp
var args = new ParameterCollection(new FakeParameter("status", 1));

var command = new FakeCommand();
command.Setup(new NonQueryResult("UPDATE User SET Status = @status", args));
```

What we did was to say that we expected the command to be executed with the specified SQL statement and one supplied parameter. Execution example:

```csharp
using (var command = connection.CreateCommand())
{
	command.CommandText = "UPDATE User SET Status = @status";
	command.AddParameter("status", 2);
	command.ExecuteNonQuery(); //<-- will fail since status was set to two.
}
```

If the command would fail (such as being invoked with invalid arguments or incorrect command text) we get a detailed exception.

Example code:

```csharp
command.CommandText = "UPDATE User SET Status = @status");
command.AddParameter("status", 1);
command.AddParameter("id", 22);
```

Exception message:

	Parameter validation failed.
	Parameter 'status': Correct.
	Parameter 'id': Did not expect this parameter, got value: '22'
	
### Enquing execution

A command can be used to invoke several queries with different results. You can therefore enqueue several command results:

```csharp
var command = new FakeCommand();
command.Setup(new NonQueryResult("UPDATE User SET Status = @status", args));
command.Setup(new ScalarResult() { Result = 20});
```

Simply call `Setup()` once for every time the command will be executed.

### Assertions

If you do not want to use the built in validations you can use traditional unit test assertions instead. Each command keeps track of it's executions in the `CommandList` property.

So to assert you can simply use:

```csharp
Assert.Equal(command.CommandList[0].CommandText, "SELECT * FROM Users WHERE Id = @id");
Assert.Equal(command.CommandList[0].Parameters[0], 20);
```	

## Working with connections

The most typical usage of ADO.NET is through connections though. So we need a way to be able to control the commands through our connections.

As for commands we use a `Setup()` method for that:

```csharp
var connection = new FakeConnection();
var cmdToReturn = new FakeCommand(new ScalarCommandResult("SELECT count(*) FROM users", null){Result = 201});
connection.Setup(cmdToReturn);
```

The method can be invoked several times:

```csharp
var connection = new FakeConnection();
var cmdToReturn = new FakeCommand(new ScalarCommandResult("SELECT count(*) FROM users", null){Result = 201});
connection.Setup(cmdToReturn);

var table = new FakeTable(new[]
{
	new object[] {1, "jonas"},
	new object[] {2, "arne"},
});
connection.Setup(new FakeCommand(table));
```

You can also specify the column names:

```csharp
var dataTable = new FakeTable(new[] {"conversation_handle", "message"}, new[]
	{
		new object[] {Guid.NewGuid(), "Hello world"}
	});
var connection = new FakeConnection();
connection.Setup(new FakeCommand(dataTable));
```

You can also now setup FakeTables using a Type to populate the column names and types:

```csharp
var dataTable = FakeTable.From<MyType>();
dataTable.AddRow(new MyType() { Id = 1, Name = "Bob" });

var connection = new FakeConnection();
connection.Setup(new FakeCommand(dataTable));
```

If you need more customizations than that simply use a regular `DataTable`.

## Docs

(my build server is currently down, the DOCS have not been updated for a while). The XmlDoc in the source code is updated though..

Either read the unit tests for a guide or checkout the [MSDN Style docs](http://griffinframework.net/docs/adonetfakes/)

## Installation (nuget)

    PM> Install-Package Griffin.AdoNetFakes