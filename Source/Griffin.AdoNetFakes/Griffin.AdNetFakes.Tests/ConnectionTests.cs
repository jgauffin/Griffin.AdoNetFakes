using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Griffin.AdoNetFakes.Tests
{
    public class ConnectionTests
    {
        [Fact]
        public void AutomaticResult()
        {
            var table = new FakeTable(new[]
                {
                    new object[] {1, "jonas"},
                    new object[] {2, "arne"},
                });
            var connection = new FakeConnection();
            connection.Setup(new FakeCommand(table));

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, UserName FROM users";
            var result = cmd.ExecuteReader();
            
            Assert.True(result.Read());
            Assert.Equal(1, (int)result[0]);
            Assert.Equal("jonas", result[1]);
        }

        [Fact]
        public void SpecificCommand()
        {
            var connection = new FakeConnection();
            var cmdToReturn = new FakeCommand(new ScalarCommandResult("SELECT count(*) FROM users", null){Result = 201});
            connection.Setup(cmdToReturn);

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT count(*) FROM users";
            var result = cmd.ExecuteScalar();

           Assert.Equal(201, result);
        }
    }
}
