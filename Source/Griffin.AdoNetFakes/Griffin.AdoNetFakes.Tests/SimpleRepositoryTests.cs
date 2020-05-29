using System;
using System.Linq;
using Griffin.AdoNetFakes.Tests.SimpleData;
using Xunit;

namespace Griffin.AdoNetFakes.Tests
{
    public class SimpleDataTests
    {
        private readonly FakeConnectionFactory _connectionFactory;
        private readonly SimpleRepository _repository;

        public SimpleDataTests()
        {
            _connectionFactory = new FakeConnectionFactory();

            _repository = new SimpleRepository(_connectionFactory);
        }

        [Fact]
        public void Get_returns_null_when_no_data_setup()
        {
            // Arrange
            var columns = new[] { nameof(SimpleObject.Id), nameof(SimpleObject.Name), nameof(SimpleObject.DateOfBirth) };
            var rows = Enumerable.Empty<object[]>();
            var table = new FakeTable(rows, columns);

            var cmdToReturn = new FakeCommand(table);
            _connectionFactory.Connection.Setup(cmdToReturn);

            // Act
            var instance = _repository.Get();

            // Assert
            Assert.Null(instance);
        }

        [Fact]
        public void Get_returns_first_instance_when_data_setup()
        {
            // Arrange
            var columns = new[] { nameof(SimpleObject.Id), nameof(SimpleObject.Name), nameof(SimpleObject.DateOfBirth) };
            var rows = new[]
            {
                //new SimpleObject { Id = 1, Name = "Bob Robertson", DateOfBirth = DateTime.Parse("1950-07-26") }
                new object[] { 1, "Bob Robertson", DateTime.Parse("1950-07-26") }
            };
            var table = new FakeTable(rows, columns);

            var cmdToReturn = new FakeCommand(table);
            _connectionFactory.Connection.Setup(cmdToReturn);

            // Act
            var instance = _repository.Get();

            // Assert
            Assert.NotNull(instance);
            Assert.Equal(rows[0][0], instance.Id);
        }
    }
}
