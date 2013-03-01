using System;
using System.Collections.Generic;
using System.Data;

namespace Griffin.AdoNetFakes
{
    /// <summary>
    ///     Fakes a db commandResult
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         To control the returned values from <see cref="ExecuteReader(System.Data.CommandBehavior)" />
    ///         and <see cref="ExecuteScalar" /> you have to invoke <see cref="Setup" />. One time for each invocation
    ///         that this command object will receive. Do note that the Setup order has to be the same as the execution
    ///         order.
    ///     </para>
    ///     <para>
    ///         The <see cref="Setup" /> method are also used to validate all commands when they are being executed. So if you
    ///         initialize the <see cref="CommandResult.CommandText" /> and/or <see cref="CommandResult.Parameters" /> their values
    ///         will be compared with that was actually executed. Any difference will generate a
    ///         <see
    ///             cref="CommandValidationException" />
    ///         .
    ///     </para>
    ///     <para>
    ///         Finally we do save all executed commands in the <see cref="CommandsList" /> property. You can use it to validate
    ///         anything invoked using this command instance.
    ///     </para>
    /// </remarks>
    public class FakeCommand : IDbCommand
    {
        private readonly List<CommandResult> _inCommands = new List<CommandResult>();
        private readonly List<CommandResult> _outCommands = new List<CommandResult>();
        private readonly ParameterCollection _parameters;
        private int _commandIndex;
        private FakeConnection _connection;
        private int _parameterIndex;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FakeCommand" /> class.
        /// </summary>
        /// <param name="fakeConnection">The fake db connection.</param>
        public FakeCommand(FakeConnection fakeConnection)
        {
            _connection = fakeConnection;
            _parameters = Factory.Instance.CreateParameterCollection(this);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FakeCommand" /> class.
        /// </summary>
        /// <param name="fakeConnection">The fake db connection.</param>
        /// <param name="results">Results for every command execution that this command will get.</param>
        public FakeCommand(FakeConnection fakeConnection, IEnumerable<CommandResult> results)
        {
            _connection = fakeConnection;
            _parameters = Factory.Instance.CreateParameterCollection(this);
            _inCommands.AddRange(results);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FakeCommand" /> class.
        /// </summary>
        /// <remarks>Creates a new FakeConnection</remarks>
        public FakeCommand()
            : this(new FakeConnection())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FakeCommand" /> class.
        /// </summary>
        /// <remarks>Creates a new FakeConnection</remarks>
        public FakeCommand(DataTable readerResult)
            : this(new FakeConnection())
        {
            if (readerResult == null) throw new ArgumentNullException("readerResult");
            _inCommands.Add(new ReaderCommandResult { Result = new FakeDataReader(readerResult) });
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FakeCommand" /> class.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <remarks>
        ///     Creates a new FakeConnection
        /// </remarks>
        /// <seealso cref="Setup" />
        public FakeCommand(CommandResult result)
            : this(new FakeConnection())
        {
            _inCommands.Add(result);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FakeCommand" /> class.
        /// </summary>
        /// <param name="results">Results for every command execution that this command will get.</param>
        /// <remarks>
        ///     Creates a new FakeConnection
        /// </remarks>
        /// <seealso cref="Setup" />
        public FakeCommand(IEnumerable<CommandResult> results)
            : this(new FakeConnection())
        {
            _inCommands.AddRange(results);
        }

        /// <summary>
        ///     Gets a list of all executed commands.
        /// </summary>
        public IList<CommandResult> CommandsList
        {
            get { return _outCommands; }
        }

        /// <summary>
        ///     .ds.ds
        /// </summary>
        public virtual bool DesignTimeVisible { get; set; }


        /// <summary>
        ///     Gets all parameters for last invoked commandResult.
        /// </summary>
        public ParameterCollection Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        ///     <see cref="Cancel" /> was invoked
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        ///     <see cref="Prepare" /> was invoked.
        /// </summary>
        public bool IsPrepared { get; set; }

        /// <summary>
        ///     <see cref="Dispose" /> was invoked.
        /// </summary>
        public bool Disposed { get; private set; }

        #region IDbCommand Members

        /// <summary>
        ///     Gets current transaction (if any)
        /// </summary>
        public virtual FakeTransaction Transaction
        {
            get { return (FakeTransaction)((IDbCommand)this).Transaction; }
        }

        /// <summary>
        ///     A callback being invoked just in the beginning of the Execute methods.
        /// </summary>
        /// <remarks>Use it to prepare the command further (such as modifying the parameters)</remarks>
        public Action<FakeCommand> BeforExecuting { get; set; }

        /// <summary>
        ///     Gets or sets factory used to create new parameters for <see cref="CreateParameter" />.
        /// </summary>
        /// <remarks>
        ///     The <c>int</c> is an zero based index which is increased every time <see cref="CreateParameter" /> has been called. You can use it to figure out which is created.
        /// </remarks>
        public Func<int, FakeParameter> ParameterFactory { get; set; }

        /// <summary>
        ///     Gets or sets the text commandResult to run against the data source.
        /// </summary>
        /// <returns>The text commandResult to execute. The default value is an empty string ("").</returns>
        public virtual string CommandText { get; set; }

        /// <summary>
        ///     Gets or sets the wait time before terminating the attempt to execute a commandResult and generating an error.
        /// </summary>
        /// <returns>The time (in seconds) to wait for the commandResult to execute. The default value is 30 seconds.</returns>
        /// <exception cref="T:System.ArgumentException">The property value assigned is less than 0. </exception>
        public virtual int CommandTimeout { get; set; }

        /// <summary>
        ///     Indicates or specifies how the <see cref="P:System.Data.IDbCommand.CommandText" /> property is interpreted.
        /// </summary>
        /// <returns>
        ///     One of the <see cref="T:System.Data.CommandType" /> values. The default is Text.
        /// </returns>
        public virtual CommandType CommandType { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="T:System.Data.IDbConnection" /> used by this instance of the
        ///     <see
        ///         cref="T:System.Data.IDbCommand" />
        ///     .
        /// </summary>
        /// <returns>The connection to the data source.</returns>
        public virtual IDbConnection Connection
        {
            get { return _connection; }
            set { _connection = (FakeConnection)value; }
        }

        /// <summary>
        ///     Gets the <see cref="T:System.Data.IDataParameterCollection" />.
        /// </summary>
        /// <returns>The parameters of the SQL statement or stored procedure.</returns>
        IDataParameterCollection IDbCommand.Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        ///     Gets or sets the transaction within which the Command object of a .NET Framework data provider executes.
        /// </summary>
        /// <returns>the Command object of a .NET Framework data provider executes. The default value is null.</returns>
        IDbTransaction IDbCommand.Transaction { get; set; }

        /// <summary>
        ///     Gets or sets how commandResult results are applied to the <see cref="T:System.Data.DataRow" /> when used by the
        ///     <see
        ///         cref="M:System.Data.IDataAdapter.Update(System.Data.DataSet)" />
        ///     method of a
        ///     <see
        ///         cref="T:System.Data.Common.DbDataAdapter" />
        ///     .
        /// </summary>
        /// <returns>
        ///     One of the <see cref="T:System.Data.UpdateRowSource" /> values. The default is Both unless the commandResult is automatically generated. Then the default is None.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        ///     The value entered was not one of the <see cref="T:System.Data.UpdateRowSource" /> values.
        /// </exception>
        public virtual UpdateRowSource UpdatedRowSource { get; set; }

        /// <summary>
        ///     Attempts to cancels the execution of an <see cref="T:System.Data.IDbCommand" />.
        /// </summary>
        /// <remarks>
        ///     Sets the <see cref="IsCancelled" /> property to true
        /// </remarks>
        public virtual void Cancel()
        {
            IsCancelled = true;
        }

        /// <summary>
        ///     Creates a new instance of an <see cref="T:System.Data.IDbDataParameter" /> object.
        /// </summary>
        /// <returns>
        ///     An IDbDataParameter object.
        /// </returns>
        /// <remarks>
        ///     Creates the parameter through <see cref="Factory.CreateParameter" />
        /// </remarks>
        public virtual IDbDataParameter CreateParameter()
        {
            var p = ParameterFactory != null
                       ? ParameterFactory(_parameterIndex)
                       : Factory.Instance.CreateParameter(this);

            _parameterIndex++;
            return p;
        }

        /// <summary>
        ///     Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        /// <returns>
        ///     The number of rows affected.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The connection does not exist.-or- The connection is not open. </exception>
        /// <remarks>
        ///     <para>
        ///         If you use <see cref="Setup" /> this method expects that you have used <see cref="NonQueryCommandResult" />. If not, it will use
        ///         <see cref="Factory.CreateNonQueryResult" /> to generate the result.
        ///     </para>
        ///     <para>
        ///         The executed command is stored in the <see cref="CommandsList" />
        ///     </para>
        /// </remarks>
        public virtual int ExecuteNonQuery()
        {
            if (BeforExecuting != null)
                BeforExecuting(this);
            _outCommands.Add(new NonQueryCommandResult(CommandText, new ParameterCollection(_parameters)));

            var inCommand = ValidateCommand<NonQueryCommandResult>();
            return inCommand == null ? Factory.Instance.CreateNonQueryResult(this) : inCommand.Result;
        }

        /// <summary>
        ///     Executes the <see cref="P:System.Data.IDbCommand.CommandText" /> against the
        ///     <see
        ///         cref="P:System.Data.IDbCommand.Connection" />
        ///     and builds an <see cref="T:System.Data.IDataReader" />.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Data.IDataReader" /> object.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If you use <see cref="Setup" /> this method expects that you have used <see cref="ReaderCommandResult" />. If not, it will use
        ///         <see cref="Factory.CreateReader" /> to generate the result.
        ///     </para>
        ///     <para>
        ///         The executed command is stored in the <see cref="CommandsList" />
        ///     </para>
        /// </remarks>
        public virtual IDataReader ExecuteReader()
        {
            if (BeforExecuting != null)
                BeforExecuting(this);

            _outCommands.Add(new ReaderCommandResult(CommandText, new ParameterCollection(_parameters)));
            var inCommand = ValidateCommand<ReaderCommandResult>();
            return inCommand == null ? Factory.Instance.CreateReader(this) : inCommand.Result;
        }

        /// <summary>
        ///     Executes the <see cref="P:System.Data.IDbCommand.CommandText" /> against the
        ///     <see
        ///         cref="P:System.Data.IDbCommand.Connection" />
        ///     , and builds an <see cref="T:System.Data.IDataReader" /> using one of the
        ///     <see
        ///         cref="T:System.Data.CommandBehavior" />
        ///     values.
        /// </summary>
        /// <param name="behavior">
        ///     One of the <see cref="T:System.Data.CommandBehavior" /> values.
        /// </param>
        /// <returns>
        ///     An <see cref="T:System.Data.IDataReader" /> object.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If you use <see cref="Setup" /> this method expects that you have used <see cref="ReaderCommandResult" />. If not, it will use
        ///         <see cref="Factory.CreateReader" /> to generate the result.
        ///     </para>
        ///     <para>
        ///         The executed command is stored in the <see cref="CommandsList" />
        ///     </para>
        /// </remarks>
        public virtual IDataReader ExecuteReader(CommandBehavior behavior)
        {
            if (BeforExecuting != null)
                BeforExecuting(this);

            _outCommands.Add(new ReaderCommandResult(CommandText, new ParameterCollection(_parameters)));

            var inCommand = ValidateCommand<ReaderCommandResult>();
            return inCommand == null ? Factory.Instance.CreateReader(this) : inCommand.Result;
        }

        /// <summary>
        ///     Executes the query, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <returns>
        ///     The first column of the first row in the resultset.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If you use <see cref="Setup" /> this method expects that you have used <see cref="ScalarCommandResult" />. If not, it will use
        ///         <see cref="Factory.CreateScalarResult" /> to generate the result.
        ///     </para>
        ///     <para>
        ///         The executed command is stored in the <see cref="CommandsList" />
        ///     </para>
        /// </remarks>
        public virtual object ExecuteScalar()
        {
            if (BeforExecuting != null)
                BeforExecuting(this);

            _outCommands.Add(new ScalarCommandResult(CommandText, new ParameterCollection(_parameters)));

            var inCommand = ValidateCommand<ScalarCommandResult>();
            return inCommand == null ? Factory.Instance.CreateScalarResult(this, _parameters) : inCommand.Result;
        }

        /// <summary>
        ///     Creates a prepared (or compiled) version of the commandResult on the data source.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The <see cref="P:System.Data.OleDb.OleDbCommand.Connection" /> is not set.-or- The
        ///     <see
        ///         cref="P:System.Data.OleDb.OleDbCommand.Connection" />
        ///     is not
        ///     <see
        ///         cref="M:System.Data.OleDb.OleDbConnection.Open" />
        ///     .
        /// </exception>
        /// <remarks>
        ///     Sets the <see cref="IsPrepared" /> property
        /// </remarks>
        public virtual void Prepare()
        {
            IsPrepared = true;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Disposed = true;
        }

        private T ValidateCommand<T>() where T : CommandResult
        {
            if (_inCommands.Count == 0)
                return null;

            if (_commandIndex >= _inCommands.Count)
                return null;

            if (string.IsNullOrEmpty(CommandText))
                throw new CommandValidationException("CommandText is null or empty", this);

            var expected = _inCommands[_commandIndex];
            if (!string.IsNullOrEmpty(expected.CommandText) && !CommandText.Equals(expected.CommandText))
                throw new CommandValidationException(
                    "CommandText differs (expected, actual): \r\n" + expected.CommandText + "\r\n" + CommandText, this);

            // We should validate parameters
            if (expected.Parameters != null)
            {
                var max = Math.Max(expected.Parameters.Count, Parameters.Count);
                var msg = "Parameter validation failed.\r\n";
                var failed = false;
                for (var i = 0; i < max; i++)
                {
                    if (i >= expected.Parameters.Count)
                    {
                        msg += string.Format("Parameter '{0}': Did not expect this parameter, got value: '{1}'\r\n",
                                             Parameters[i].ParameterName,
                                             Parameters[i].Value);
                        failed = true;
                    }
                    else if (i >= Parameters.Count)
                    {
                        msg += string.Format("Parameter '{0}': Expected value '{1}', parameter was not specified\r\n",
                                             expected.Parameters[i].ParameterName,
                                             expected.Parameters[i].Value);
                        failed = true;
                    }
                    else if (!expected.Parameters[i].Value.Equals(Parameters[i].Value))
                    {
                        msg += string.Format("Parameter '{0}': Expected: '{1}', got: '{2}'\r\n",
                                             expected.Parameters[i].ParameterName, expected.Parameters[i].Value,
                                             Parameters[i].Value);
                        failed = true;
                    }
                    else
                    {
                        msg += string.Format("Parameter '{0}': Correct.\r\n", expected.Parameters[i].ParameterName);
                    }
                }

                if (failed)
                    throw new CommandValidationException(msg, this);
            }

            var casted = expected as T;
            if (casted == null)
                throw new InvalidOperationException(
                    string.Format("Expected Setup() command with index {0} to be of type '{1}'.", _commandIndex,
                                  typeof(T).Name));

            ++_commandIndex;
            return casted;
        }

        #endregion

        public void AddParameter(string name, object value)
        {
            Parameters.Add(new FakeParameter(name, value));
        }

        /// <summary>
        ///     Reset commandResult state
        /// </summary>
        public virtual void Reset()
        {
            IsPrepared = false;
            IsCancelled = false;
            _outCommands.Clear();
            _inCommands.Clear();
            _parameters.Clear();
            _commandIndex = 0;
        }

        /// <summary>
        ///     Setup a commandResult which will be executed
        /// </summary>
        /// <param name="commandResult">Command</param>
        /// <remarks>
        ///     Use this method if you would like to validate all commands which are executed. If the commandResult text
        ///     or parameters differs when it's being executed then an exception will be thrown.
        /// </remarks>
        public void Setup(CommandResult commandResult)
        {
            if (commandResult == null) throw new ArgumentNullException("commandResult");
            _inCommands.Add(commandResult);
        }
    }
}