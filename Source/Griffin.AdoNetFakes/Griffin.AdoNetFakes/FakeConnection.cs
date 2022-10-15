using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Griffin.AdoNetFakes;

/// <summary>
///     Fake connection
/// </summary>
/// <see cref="Setup" />
public class FakeConnection : DbConnection, IDbConnection
{
    private readonly Queue<FakeCommand> _commandsToReturn = new();
    private readonly List<FakeTransaction> _transactions = new();

    /// <summary>
    ///     Gets if disposed has been invoked.
    /// </summary>
    public bool IsDisposed { get; set; }

    /// <summary>
    ///     Used by <see cref="Database" />
    /// </summary>
    public string CurrentDatabase { get; set; }

    /// <summary>
    ///     Gets "FakeProvider"
    /// </summary>
    public override string DataSource => "FakeProvider";

    /// <summary>
    ///     Controlled by <see cref="ServerVersionToReturn" />.
    /// </summary>
    public override string ServerVersion => ServerVersionToReturn;

    public string ServerVersionToReturn { get; set; } = "42";

    /// <summary>
    ///     Gets current state
    /// </summary>
    public ConnectionState CurrentState { get; set; }

    /// <summary>
    ///     Gets all commands that was created for this connection
    /// </summary>
    public List<FakeCommand> Commands { get; } = new();

    /// <summary>
    ///     reset state flags
    /// </summary>
    public virtual void Reset()
    {
        CurrentState = ConnectionState.Closed;
    }

    #region IDbConnection Members

    /// <summary>
    ///     Enqueue a command which should be returned by <see cref="CreateCommand" />.
    /// </summary>
    /// <param name="command"></param>
    /// <remarks>You can enqueue several commands, just invoke this method several times.</remarks>
    public void Setup(FakeCommand command)
    {
        if (command == null)
        {
            throw new ArgumentNullException("command");
        }

        _commandsToReturn.Enqueue(command);
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <remarks>
    ///     Invokes <see cref="Reset" /> and sets <see cref="IsDisposed" />
    /// </remarks>
    protected override void Dispose(bool isDisposed)
    {
        Reset();
        IsDisposed = true;
    }

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
    {
        return BeginTransaction(isolationLevel);
    }

    /// <summary>
    ///     Begins a database transaction.
    /// </summary>
    /// <returns>
    ///     An object representing the new transaction.
    /// </returns>
    /// <seealso cref="Factory.CreateTransaction(FakeConnection)" />
    IDbTransaction IDbConnection.BeginTransaction()
    {
        var trans = Factory.Instance.CreateTransaction(this);
        _transactions.Add(trans);
        return trans;
    }

    /// <summary>
    ///     Begins a database transaction with the specified <see cref="T:System.Data.IsolationLevel" /> value.
    /// </summary>
    /// <param name="il">
    ///     One of the <see cref="T:System.Data.IsolationLevel" /> values.
    /// </param>
    /// <returns>
    ///     An object representing the new transaction.
    /// </returns>
    /// <seealso cref="Factory.CreateTransaction(FakeConnection, IsolationLevel)" />
    IDbTransaction IDbConnection.BeginTransaction(IsolationLevel il)
    {
        var trans = Factory.Instance.CreateTransaction(this, il);
        _transactions.Add(trans);
        return trans;
    }

    /// <summary>
    ///     Closes the connection to the database.
    /// </summary>
    /// <remarks>
    ///     Sets <see cref="CurrentState" /> to close and invokes <see cref="Reset" />
    /// </remarks>
    public override void Close()
    {
        CurrentState = ConnectionState.Closed;
        Reset();
    }

    /// <summary>
    ///     Changes the current database for an open Connection object.
    /// </summary>
    /// <param name="databaseName">The name of the database to use in place of the current database.</param>
    /// <seealso cref="CurrentDatabase" />
    public override void ChangeDatabase(string databaseName)
    {
        CurrentDatabase = databaseName;
    }

    /// <summary>
    ///     Creates and returns a Command object associated with the connection. Uses commands from <see cref="Setup" /> if
    ///     configured,
    ///     otherwise <see cref="Factory.CreateCommand" />.
    /// </summary>
    /// <returns>
    ///     A Command object associated with the connection.
    /// </returns>
    /// <remarks>
    ///     Adds command to <see cref="Commands" />.
    /// </remarks>
    /// <seealso cref="Factory.CreateCommand" />
    IDbCommand IDbConnection.CreateCommand()
    {
        var command = _commandsToReturn.Count > 0
            ? _commandsToReturn.Dequeue()
            : Factory.Instance.CreateCommand(this);

        Commands.Add(command);
        return command;
    }

    protected override DbCommand CreateDbCommand()
    {
        var command = _commandsToReturn.Count > 0
            ? _commandsToReturn.Dequeue()
            : Factory.Instance.CreateCommand(this);

        Commands.Add(command);
        return command;
    }

    /// <summary>
    ///     Opens a database connection with the settings specified by the ConnectionString property of the provider-specific
    ///     Connection object.
    /// </summary>
    public override void Open()
    {
        CurrentState = ConnectionState.Open;
    }

    public override string ConnectionString { get; set; }

    /// <summary>
    ///     Gets the name of the current database or the database to be used after a connection is opened.
    /// </summary>
    /// <returns>
    ///     The name of the current database or the name of the database to be used once a connection is open. The default
    ///     value is an empty string.
    /// </returns>
    public override string Database => CurrentDatabase;

    /// <summary>
    ///     Gets the current state of the connection.
    /// </summary>
    /// <returns>
    ///     One of the <see cref="T:System.Data.ConnectionState" /> values.
    /// </returns>
    public override ConnectionState State => CurrentState;

    /// <summary>
    ///     Gets all transactions which have been created for this connection.
    /// </summary>
    public FakeTransaction[] Transactions => _transactions.ToArray();

    #endregion
}
