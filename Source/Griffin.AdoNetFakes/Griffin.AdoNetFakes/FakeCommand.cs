using System.Collections.Generic;
using System.Data;

namespace Griffin.AdoNetFakes
{
    /// <summary>
    /// Fakes a db command
    /// </summary>
    public class FakeCommand : IDbCommand
    {
        private readonly List<string> _commandStrings = new List<string>();
        private readonly ParameterCollection _parameters;
        private FakeDbConnection _connection;
        private List<ParameterCollection> _parameterCollections = new List<ParameterCollection>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCommand"/> class.
        /// </summary>
        /// <param name="fakeDbConnection">The fake db connection.</param>
        public FakeCommand(FakeDbConnection fakeDbConnection)
        {
            _parameters = Factory.Instance.CreateParameterCollection(this);
            _connection = fakeDbConnection;
        }

        /// <summary>
        /// Gets all invoked command strings.
        /// </summary>
        public List<string> CommandStrings
        {
            get { return _commandStrings; }
        }


        /// <summary>
        /// .ds.ds
        /// </summary>
        public virtual bool DesignTimeVisible { get; set; }

        /// <summary>
        /// <see cref="ExecuteNonQuery"/> was invoked.
        /// </summary>
        public bool ExecuteNonQueryInvoked { get; set; }

        /// <summary>
        /// <see cref="ExecuteScalar"/> was invoked
        /// </summary>
        public bool ExecuteScalarInvoked { get; set; }

        /// <summary>
        /// Gets all parameters for each invoked command.
        /// </summary>
        public List<ParameterCollection> ExecutedParameterCollections
        {
            get { return _parameterCollections; }
            set { _parameterCollections = value; }
        }

        /// <summary>
        /// <see cref="Cancel"/> was invoked
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// <see cref="Prepare"/> was invoked.
        /// </summary>
        public bool IsPrepared { get; set; }

        /// <summary>
        /// <see cref="Dispose"/> was invoked.
        /// </summary>
        public bool Disposed { get; private set; }

        #region IDbCommand Members

        /// <summary>
        /// Gets or sets the text command to run against the data source.
        /// </summary>
        /// <returns>The text command to execute. The default value is an empty string ("").</returns>
        public virtual string CommandText { get; set; }
        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        /// <returns>The time (in seconds) to wait for the command to execute. The default value is 30 seconds.</returns>
        ///   
        /// <exception cref="T:System.ArgumentException">The property value assigned is less than 0. </exception>
        public virtual int CommandTimeout { get; set; }
        /// <summary>
        /// Indicates or specifies how the <see cref="P:System.Data.IDbCommand.CommandText"/> property is interpreted.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Data.CommandType"/> values. The default is Text.</returns>
        public virtual CommandType CommandType { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="T:System.Data.IDbConnection"/> used by this instance of the <see cref="T:System.Data.IDbCommand"/>.
        /// </summary>
        /// <returns>The connection to the data source.</returns>
        public virtual IDbConnection Connection
        {
            get { return _connection; }
            set { _connection = (FakeDbConnection)value; }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Data.IDataParameterCollection"/>.
        /// </summary>
        /// <returns>The parameters of the SQL statement or stored procedure.</returns>
        public virtual IDataParameterCollection Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Gets or sets the transaction within which the Command object of a .NET Framework data provider executes.
        /// </summary>
        /// <returns>the Command object of a .NET Framework data provider executes. The default value is null.</returns>
        public virtual IDbTransaction Transaction { get; set; }
        /// <summary>
        /// Gets or sets how command results are applied to the <see cref="T:System.Data.DataRow"/> when used by the <see cref="M:System.Data.IDataAdapter.Update(System.Data.DataSet)"/> method of a <see cref="T:System.Data.Common.DbDataAdapter"/>.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Data.UpdateRowSource"/> values. The default is Both unless the command is automatically generated. Then the default is None.</returns>
        ///   
        /// <exception cref="T:System.ArgumentException">The value entered was not one of the <see cref="T:System.Data.UpdateRowSource"/> values. </exception>
        public virtual UpdateRowSource UpdatedRowSource { get; set; }

        /// <summary>
        /// Attempts to cancels the execution of an <see cref="T:System.Data.IDbCommand"/>.
        /// </summary>
        /// <remarks>Sets the <see cref="IsCancelled"/> property to true</remarks>
        public virtual void Cancel()
        {
            IsCancelled = true;
        }

        /// <summary>
        /// Creates a new instance of an <see cref="T:System.Data.IDbDataParameter"/> object.
        /// </summary>
        /// <returns>
        /// An IDbDataParameter object.
        /// </returns>
        /// <remarks>Creates the parameter through <see cref="Factory.CreateParameter"/></remarks>
        public virtual IDbDataParameter CreateParameter()
        {
            return Factory.Instance.CreateParameter(this);
        }

        /// <summary>
        /// Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        /// <returns>
        /// The number of rows affected.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The connection does not exist.-or- The connection is not open. </exception>
        /// <remarks>
        /// Stores the command text in <see cref="CommandStrings"/>, the parameters in <see cref="ExecutedParameterCollections"/> and finnally sets
        /// <see cref="ExecuteNonQueryInvoked"/> before returning <see cref="Factory.CreateNonQueryResult"/>.
        /// </remarks>
        public virtual int ExecuteNonQuery()
        {
            ExecutedParameterCollections.Add(new ParameterCollection(_parameters));
            CommandStrings.Add(CommandText);
            ExecuteNonQueryInvoked = true;
            return Factory.Instance.CreateNonQueryResult(this);
        }

        /// <summary>
        /// Executes the <see cref="P:System.Data.IDbCommand.CommandText"/> against the <see cref="P:System.Data.IDbCommand.Connection"/> and builds an <see cref="T:System.Data.IDataReader"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Data.IDataReader"/> object.
        /// </returns>
        /// <remarks>
        /// Stores the command text in <see cref="CommandStrings"/>, the parameters in <see cref="ExecutedParameterCollections"/> and finnally sets
        /// <see cref="ExecuteNonQueryInvoked"/> before returning <see cref="Factory.CreateReader"/>.
        /// </remarks>
        public virtual IDataReader ExecuteReader()
        {
            CommandStrings.Add(CommandText);
            ExecutedParameterCollections.Add(new ParameterCollection(_parameters));
            return Factory.Instance.CreateReader(this);
        }

        /// <summary>
        /// Executes the <see cref="P:System.Data.IDbCommand.CommandText"/> against the <see cref="P:System.Data.IDbCommand.Connection"/>, and builds an <see cref="T:System.Data.IDataReader"/> using one of the <see cref="T:System.Data.CommandBehavior"/> values.
        /// </summary>
        /// <param name="behavior">One of the <see cref="T:System.Data.CommandBehavior"/> values.</param>
        /// <returns>
        /// An <see cref="T:System.Data.IDataReader"/> object.
        /// </returns>
        /// <remarks>
        /// Stores the command text in <see cref="CommandStrings"/>, the parameters in <see cref="ExecutedParameterCollections"/> and finnally sets
        /// <see cref="ExecuteNonQueryInvoked"/> before returning <see cref="Factory.CreateNonQueryResult"/>.
        /// </remarks>
        public virtual IDataReader ExecuteReader(CommandBehavior behavior)
        {
            CommandStrings.Add(CommandText);
            ExecutedParameterCollections.Add(new ParameterCollection(_parameters));
            return Factory.Instance.CreateReader(this);
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <returns>
        /// The first column of the first row in the resultset.
        /// </returns>
        /// <remarks>Adds the command text to <see cref="CommandStrings"/>, the parameters to <see cref="ExecutedParameterCollections"/> and finally
        /// sets the <see cref="ExecuteScalarInvoked"/> to true before returning the result of <see cref="Factory.CreateScalarResult"/>.</remarks>
        public virtual object ExecuteScalar()
        {
            CommandStrings.Add(CommandText);
            ExecutedParameterCollections.Add(new ParameterCollection(_parameters));
            ExecuteScalarInvoked = true;
            return Factory.Instance.CreateScalarResult(this, _parameters);
        }

        /// <summary>
        /// Creates a prepared (or compiled) version of the command on the data source.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Data.OleDb.OleDbCommand.Connection"/> is not set.-or- The <see cref="P:System.Data.OleDb.OleDbCommand.Connection"/> is not <see cref="M:System.Data.OleDb.OleDbConnection.Open"/>. </exception>
        /// <remarks>Sets the <see cref="IsPrepared"/> property</remarks>
        public virtual void Prepare()
        {
            IsPrepared = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Disposed = true;
        }

        #endregion

        /// <summary>
        /// Executes the data reader.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <remarks>
        /// Stores command text in <see cref="CommandStrings"/> and all parameters in the <see cref="ExecutedParameterCollections"/> and 
        /// then return a read which is created through the <see cref="Factory.CreateReader"/>.
        /// </remarks>
        public virtual IDataReader ExecuteDataReader(CommandBehavior behavior)
        {
            CommandStrings.Add(CommandText);
            ExecutedParameterCollections.Add(new ParameterCollection(_parameters));
            return Factory.Instance.CreateReader(this);
        }

        /// <summary>
        /// Reset command state
        /// </summary>
        public virtual void Reset()
        {
            IsPrepared = false;
            IsCancelled = false;
            ExecuteNonQueryInvoked = false;
            ExecuteScalarInvoked = false;
        }
    }
}