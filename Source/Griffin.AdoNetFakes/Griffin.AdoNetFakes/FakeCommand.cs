using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Griffin.AdoNetFakes;

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
///         initialize the <see cref="CommandResult.CommandText" /> and/or <see cref="CommandResult.Parameters" /> their
///         values
///         will be compared with that was actually executed. Any difference will generate a
///         <see
///             cref="CommandValidationException" />
///         .
///     </para>
///     <para>
///         Finally we do save all executed commands in the <see cref="CommandsList" /> property. You can use it to
///         validate
///         anything invoked using this command instance.
///     </para>
/// </remarks>
public class FakeCommand : DbCommand, IDbCommand
{
    private readonly List<CommandResult> _inCommands = new();
    private readonly List<CommandResult> _outCommands = new();
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
        FakeParameters = Factory.Instance.CreateParameterCollection(this);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FakeCommand" /> class.
    /// </summary>
    /// <param name="fakeConnection">The fake db connection.</param>
    /// <param name="results">Results for every command execution that this command will get.</param>
    public FakeCommand(FakeConnection fakeConnection, IEnumerable<CommandResult> results)
    {
        _connection = fakeConnection;
        FakeParameters = Factory.Instance.CreateParameterCollection(this);
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
        if (readerResult == null)
        {
            throw new ArgumentNullException("readerResult");
        }

        _inCommands.Add(new ReaderCommandResult {Result = new FakeDataReader(readerResult)});
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
    public IList<CommandResult> CommandsList => _outCommands;

    public override bool DesignTimeVisible { get; set; }

    /// <summary>
    ///     Gets all parameters for last invoked commandResult.
    /// </summary>
    public FakeDbParameterCollection FakeParameters { get; }

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
    public bool IsDisposed { get; private set; }

    public override CommandType CommandType { get; set; }

    public void AddParameter(string name, object value)
    {
        FakeParameters.Add(new FakeParameter(name, value));
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
        Parameters.Clear();
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
        if (commandResult == null)
        {
            throw new ArgumentNullException("commandResult");
        }

        _inCommands.Add(commandResult);
    }

    #region IDbCommand Members

    /// <summary>
    ///     Gets current transaction (if any)
    /// </summary>
    public virtual FakeTransaction FakeTransaction => (FakeTransaction)((IDbCommand)this).Transaction;

    /// <summary>
    ///     A callback being invoked just in the beginning of the Execute methods.
    /// </summary>
    /// <remarks>Use it to prepare the command further (such as modifying the parameters)</remarks>
    public Action<FakeCommand> BeforeExecuting { get; set; }

    /// <summary>
    ///     Gets or sets factory used to create new parameters for <see cref="CreateParameter" />.
    /// </summary>
    /// <remarks>
    ///     The <c>int</c> is an zero based index which is increased every time <see cref="CreateParameter" /> has been called.
    ///     You can use it to figure out which is created.
    /// </remarks>
    public Func<int, FakeParameter> ParameterFactory { get; set; }

    public override object ExecuteScalar()
    {
        return ((IDbCommand)this).ExecuteScalar();
    }

    /// <summary>
    ///     Gets or sets the <see cref="T:System.Data.IDbConnection" /> used by this instance of the
    ///     <see
    ///         cref="T:System.Data.IDbCommand" />
    ///     .
    /// </summary>
    /// <returns>The connection to the data source.</returns>
    IDbConnection IDbCommand.Connection
    {
        get => _connection;
        set => _connection = (FakeConnection)value;
    }

    protected override DbConnection DbConnection { get; set; }

    protected override DbParameterCollection DbParameterCollection => FakeParameters;

    protected override DbTransaction DbTransaction { get; set; }

    /// <summary>
    ///     Gets the <see cref="T:System.Data.IDataParameterCollection" />.
    /// </summary>
    /// <returns>The parameters of the SQL statement or stored procedure.</returns>
    IDataParameterCollection IDbCommand.Parameters => Parameters;

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
    ///     One of the <see cref="T:System.Data.UpdateRowSource" /> values. The default is Both unless the commandResult is
    ///     automatically generated. Then the default is None.
    /// </returns>
    /// <exception cref="T:System.ArgumentException">
    ///     The value entered was not one of the <see cref="T:System.Data.UpdateRowSource" /> values.
    /// </exception>
    public override UpdateRowSource UpdatedRowSource { get; set; }

    /// <summary>
    ///     Attempts to cancels the execution of an <see cref="T:System.Data.IDbCommand" />.
    /// </summary>
    /// <remarks>
    ///     Sets the <see cref="IsCancelled" /> property to true
    /// </remarks>
    public override void Cancel()
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
    IDbDataParameter IDbCommand.CreateParameter()
    {
        var p = ParameterFactory != null
            ? ParameterFactory(_parameterIndex)
            : Factory.Instance.CreateParameter(this);

        _parameterIndex++;
        return p;
    }

    protected override DbParameter CreateDbParameter()
    {
        return new FakeParameter();
    }

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        return (DbDataReader)((IDbCommand)this).ExecuteReader(behavior);
    }

    /// <summary>
    ///     Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number
    ///     of rows affected.
    /// </summary>
    /// <returns>
    ///     The number of rows affected.
    /// </returns>
    /// <exception cref="T:System.InvalidOperationException">The connection does not exist.-or- The connection is not open. </exception>
    /// <remarks>
    ///     <para>
    ///         If you use <see cref="Setup" /> this method expects that you have used <see cref="NonQueryCommandResult" />. If
    ///         not, it will use
    ///         <see cref="Factory.CreateNonQueryResult" /> to generate the result.
    ///     </para>
    ///     <para>
    ///         The executed command is stored in the <see cref="CommandsList" />
    ///     </para>
    /// </remarks>
    public override int ExecuteNonQuery()
    {
        BeforeExecuting?.Invoke(this);
        _outCommands.Add(new NonQueryCommandResult(CommandText, new FakeDbParameterCollection(FakeParameters)));

        var inCommand = ValidateCommand<NonQueryCommandResult>();
        return inCommand?.Result ?? Factory.Instance.CreateNonQueryResult(this);
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
    ///         If you use <see cref="Setup" /> this method expects that you have used <see cref="ReaderCommandResult" />. If
    ///         not, it will use
    ///         <see cref="Factory.CreateReader" /> to generate the result.
    ///     </para>
    ///     <para>
    ///         The executed command is stored in the <see cref="CommandsList" />
    ///     </para>
    /// </remarks>
    IDataReader IDbCommand.ExecuteReader()
    {
        BeforeExecuting?.Invoke(this);

        _outCommands.Add(new ReaderCommandResult(CommandText, new FakeDbParameterCollection(FakeParameters)));
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
    ///         If you use <see cref="Setup" /> this method expects that you have used <see cref="ReaderCommandResult" />. If
    ///         not, it will use
    ///         <see cref="Factory.CreateReader" /> to generate the result.
    ///     </para>
    ///     <para>
    ///         The executed command is stored in the <see cref="CommandsList" />
    ///     </para>
    /// </remarks>
    IDataReader IDbCommand.ExecuteReader(CommandBehavior behavior)
    {
        BeforeExecuting?.Invoke(this);

        _outCommands.Add(new ReaderCommandResult(CommandText, new FakeDbParameterCollection(FakeParameters)));

        var inCommand = ValidateCommand<ReaderCommandResult>();
        return inCommand == null ? Factory.Instance.CreateReader(this) : inCommand.Result;
    }

    /// <summary>
    ///     Executes the query, and returns the first column of the first row in the resultset returned by the query. Extra
    ///     columns or rows are ignored.
    /// </summary>
    /// <returns>
    ///     The first column of the first row in the resultset.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         If you use <see cref="Setup" /> this method expects that you have used <see cref="ScalarCommandResult" />. If
    ///         not, it will use
    ///         <see cref="Factory.CreateScalarResult" /> to generate the result.
    ///     </para>
    ///     <para>
    ///         The executed command is stored in the <see cref="CommandsList" />
    ///     </para>
    /// </remarks>
    object IDbCommand.ExecuteScalar()
    {
        BeforeExecuting?.Invoke(this);

        _outCommands.Add(new ScalarCommandResult(CommandText, new FakeDbParameterCollection(FakeParameters)));

        var inCommand = ValidateCommand<ScalarCommandResult>();
        return inCommand == null ? Factory.Instance.CreateScalarResult(this, FakeParameters) : inCommand.Result;
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
    public override void Prepare()
    {
        IsPrepared = true;
    }

    public override string CommandText { get; set; }
    public override int CommandTimeout { get; set; }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    protected override void Dispose(bool isDisposing)
    {
        IsDisposed = true;
    }

    private T ValidateCommand<T>() where T : CommandResult
    {
        if (_inCommands.Count == 0)
        {
            return null;
        }

        if (_commandIndex >= _inCommands.Count)
        {
            return null;
        }

        if (string.IsNullOrEmpty(CommandText))
        {
            throw new CommandValidationException("CommandText is null or empty", this);
        }

        var expected = _inCommands[_commandIndex];
        if (!string.IsNullOrEmpty(expected.CommandText) && !CommandText.Equals(expected.CommandText))
        {
            throw new CommandValidationException(
                $"CommandText differs (expected, actual): \r\n{expected.CommandText}\r\n{CommandText}", this);
        }

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
                    msg +=
                        $"Parameter '{Parameters[i].ParameterName}': Did not expect this parameter, got value: '{Parameters[i].Value}'\r\n";
                    failed = true;
                }
                else if (i >= Parameters.Count)
                {
                    msg +=
                        $"Parameter '{expected.Parameters[i].ParameterName}': Expected value '{expected.Parameters[i].Value}', parameter was not specified\r\n";
                    failed = true;
                }
                else if (!expected.Parameters[i].Value.Equals(Parameters[i].Value))
                {
                    msg +=
                        $"Parameter '{expected.Parameters[i].ParameterName}': Expected: '{expected.Parameters[i].Value}', got: '{Parameters[i].Value}'\r\n";
                    failed = true;
                }
                else
                {
                    msg += $"Parameter '{expected.Parameters[i].ParameterName}': Correct.\r\n";
                }
            }

            if (failed)
            {
                throw new CommandValidationException(msg, this);
            }
        }

        if (expected is not T casted)
        {
            throw new InvalidOperationException(
                $"Expected Setup() command with index {_commandIndex} to be of type '{typeof(T).Name}'.");
        }

        ++_commandIndex;
        return casted;
    }

    #endregion
}
