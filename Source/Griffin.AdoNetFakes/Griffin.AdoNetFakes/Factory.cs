using System;
using System.Data;

namespace Griffin.AdoNetFakes
{
    /// <summary>
    /// Builds all fakes used by this framework (called from within the classes).
    /// </summary>
    /// <remarks>
    /// Inherit it and override the methods if you need
    /// to control the objects which are returned.</remarks>
    public class Factory 
    {
        private static Factory _factory = new Factory();

        /// <summary>
        /// Get current implementation
        /// </summary>
        public static Factory Instance { get { return _factory; } }

        /// <summary>
        /// Assign a custom implementation
        /// </summary>
        /// <param name="factory">The factory.</param>
        public static void Assign(Factory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Create a new command
        /// </summary>
        /// <param name="connection">The connection that want to return a new command.</param>
        /// <returns>Created command</returns>
        public virtual FakeCommand CreateCommand(FakeDbConnection connection)
        {
            return new FakeCommand(connection);
        }

        /// <summary>
        /// Creates a new parameter collection.
        /// </summary>
        /// <param name="command">The command that wants to use a new parameter collection.</param>
        /// <returns>Created collection</returns>
        public virtual ParameterCollection CreateParameterCollection(FakeCommand command)
        {
            return new ParameterCollection(command);
        }

        /// <summary>
        /// Create a new reader
        /// </summary>
        /// <param name="fakeCommand">Command which must return a new reader since <see cref="IDbCommand.ExecuteReader"/> has been invoked..</param>
        /// <returns>A new reader</returns>
        /// <remarks>Default implementation will invoke <see cref="CreateResult"/> to get a new result.
        /// </remarks>
        public virtual FakeDataReader CreateReader(FakeCommand fakeCommand)
        {
            return new FakeDataReader(CreateResult(fakeCommand));
        }

        /// <summary>
        /// Create a new result
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>New result table</returns>
        /// <remarks>Will try to use the <see cref="FakeDbConnection.NextResult"/> property as the result</remarks>
        public virtual DataTable CreateResult(FakeCommand command)
        {
            var con = command.Connection as FakeDbConnection;
            if (con == null)
                throw new InvalidOperationException("The default implementation expects a FakeDbConnection to be able to get the next result");

            return con.NextResult;
        }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="command">The command which creates a parameter.</param>
        /// <returns>New parameter</returns>
        public virtual IDbDataParameter CreateParameter(FakeCommand command)
        {
            return new FakeParameter();
        }

        /// <summary>
        /// Create a new transaction
        /// </summary>
        /// <param name="connection">Connection which wants to create a transaction</param>
        /// <returns>The transaction</returns>
        public virtual FakeTransaction CreateTransaction(FakeDbConnection connection)
        {
            return new FakeTransaction(connection);
        }

        /// <summary>
        /// Creates the transaction.
        /// </summary>
        /// <param name="connection">The connection which wants to create a transaction.</param>
        /// <param name="il">Isolation level.</param>
        /// <returns>The transaction</returns>
        public virtual FakeTransaction CreateTransaction(FakeDbConnection connection, IsolationLevel il)
        {
            return new FakeTransaction(connection, il);
        }

        /// <summary>
        /// Creates scalar result for a <see cref="FakeCommand.ExecuteScalar"/>.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Created result, default is null</returns>
        public object CreateScalarResult(FakeCommand command, ParameterCollection parameters)
        {
            return null;
        }

        /// <summary>
        /// Creates result for a <see cref="FakeCommand.ExecuteNonQuery"/>.
        /// </summary>
        /// <param name="command">Command which is executed</param>
        /// <returns>Default is 1</returns>
        public int CreateNonQueryResult(FakeCommand command)
        {
            return 1;
        }

        /// <summary>
        /// Creates a schema table.
        /// </summary>
        /// <param name="reader">The reader for whom the schema was requested.</param>
        /// <returns>Default is an empty table.</returns>
        public DataTable CreateSchemaTable(FakeDataReader reader)
        {
            return new DataTable();
        }
    }
}