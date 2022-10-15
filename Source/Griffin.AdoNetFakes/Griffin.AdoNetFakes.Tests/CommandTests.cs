using Xunit;

namespace Griffin.AdoNetFakes.Tests;

public class CommandTests
{
    [Fact]
    public void Validation_NoText()
    {
        var sut = new FakeCommand(new[] {new ScalarCommandResult("SELECT 1", null) {Result = "Hello"}});

        Assert.Throws<CommandValidationException>(() => sut.ExecuteScalar());
    }

    [Fact]
    public void Validation_Correct()
    {
        var sut = new FakeCommand(new[] {new ScalarCommandResult("SELECT 1", null) {Result = "Hello"}});

        sut.CommandText = "SELECT 1";
        var actual = sut.ExecuteScalar();

        Assert.Equal("Hello", actual);
    }

    [Fact]
    public void Validation_Correct_WithParameter()
    {
        var args = new FakeDbParameterCollection(new FakeParameter("myName", "World"));
        var result = new ScalarCommandResult("SELECT 1 FROM Y WHERE Name = @myName", args) {Result = "Hello"};
        var sut = new FakeCommand(new[] {result});

        sut.CommandText = "SELECT 1 FROM Y WHERE Name = @myName";
        sut.AddParameter("myName", "World");
        var actual = sut.ExecuteScalar();

        Assert.Equal("Hello", actual);
    }

    [Fact]
    public void Validation_Invalid_parameter_value()
    {
        var args = new FakeDbParameterCollection(new FakeParameter("myName", "World correct"));
        var result = new ScalarCommandResult("SELECT 1 FROM Y WHERE Name = @myName", args) {Result = "Hello"};
        var sut = new FakeCommand(new[] {result});

        sut.CommandText = "SELECT 1 FROM Y WHERE Name = @myName";
        sut.AddParameter("myName", "World");
        Assert.Throws<CommandValidationException>(() => sut.ExecuteScalar());
    }

    [Fact]
    public void Validation_Missing_Parameter()
    {
        var args = new FakeDbParameterCollection(new FakeParameter("myName", "World"));
        var result = new ScalarCommandResult("SELECT 1 FROM Y WHERE Name = @myName", args) {Result = "Hello correct"};
        var sut = new FakeCommand(new[] {result});

        sut.CommandText = "SELECT 1 FROM Y WHERE Name = @myName";
        Assert.Throws<CommandValidationException>(() => sut.ExecuteScalar());
    }

    [Fact]
    public void Validation_Missing_Parameter2()
    {
        var args = new FakeDbParameterCollection(new FakeParameter("status", 1));
        var result = new ScalarCommandResult("UPDATE User SET Status = @status", args) {Result = "Hello correct"};
        var sut = new FakeCommand(new[] {result});

        sut.CommandText = "UPDATE User SET Status = @status";
        sut.AddParameter("status", 1);
        sut.AddParameter("id", 22);
        Assert.Throws<CommandValidationException>(() => sut.ExecuteScalar());
    }

    [Fact]
    public void Validation_Unmatched_Parameter()
    {
        var args = new FakeDbParameterCollection(new FakeParameter("some", 4));
        var result = new ScalarCommandResult("SELECT 1 FROM Y WHERE Name = @myName", args) {Result = "Hello correct"};
        // you can also sue the Setup method instead of the constructor
        var sut = new FakeCommand(new[] {result});

        sut.CommandText = "SELECT 1 FROM Y WHERE Name = @myName";
        sut.AddParameter("myName", "World");
        Assert.Throws<CommandValidationException>(() => sut.ExecuteScalar());
    }

    [Fact]
    public void Validation_Invalid_CommandText()
    {
        // you can also sue the Setup method instead of the constructor
        var sut = new FakeCommand(new[] {new ScalarCommandResult("SELECT 1", null) {Result = "Hello"}});

        sut.CommandText = "SELECT 2";
        Assert.Throws<CommandValidationException>(() => sut.ExecuteScalar());
    }

    [Fact]
    public void ExecuteScalar()
    {
        var sut = new FakeCommand();
        sut.Setup(new ScalarCommandResult {Result = "Hello"});

        sut.CommandText = "SELECT 2";
        var result = sut.ExecuteScalar();
    }

    [Fact]
    public void ExecuteMany()
    {
        var sut = new FakeCommand();
        sut.Setup(new ScalarCommandResult {Result = "Hello"});
        sut.Setup(new NonQueryCommandResult {Result = 0});

        sut.CommandText = "SELECT 2";
        var result = sut.ExecuteScalar();

        sut.CommandText = "UPDATE XXXX SET UserId = @userId";
        sut.AddParameter("userId", 1);
        var result2 = sut.ExecuteNonQuery();

        Assert.Equal("Hello", result);
        Assert.Equal("UPDATE XXXX SET UserId = @userId", sut.CommandsList[1].CommandText);
        Assert.Equal(1, sut.CommandsList[1].Parameters[0].Value);
    }

    [Fact]
    public void ReturnTable()
    {
        var sut = new FakeCommand();
        sut.Setup(new ScalarCommandResult {Result = "Hello"});
        sut.Setup(new NonQueryCommandResult {Result = 0});

        sut.CommandText = "SELECT 2";
        var result = sut.ExecuteScalar();

        sut.CommandText = "UPDATE XXXX SET UserId = @userId";
        sut.AddParameter("userId", 1);
        var result2 = sut.ExecuteNonQuery();

        Assert.Equal("Hello", result);
        Assert.Equal("UPDATE XXXX SET UserId = @userId", sut.CommandsList[1].CommandText);
        Assert.Equal(1, sut.CommandsList[1].Parameters[0].Value);
    }
}
