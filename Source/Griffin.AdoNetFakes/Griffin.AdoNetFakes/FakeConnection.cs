using System;
using System.Collections.Generic;
using System.Data;

namespace Griffin.AdoNetFakes
{
    /// <summary>
    ///     Fake connection
    /// </summary>
    /// <see cref="Setup"/>
    public class FakeConnection : IDbConnection
    {
        private readonly Queue<FakeCommand> _commandsToReturn = new Queue<FakeCommand>();
        private readonly List<FakeCommand> _commands = new List<FakeCommand>();
        private readonly List<FakeTransaction> _transactions = new List<FakeTransaction>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="FakeConnection" /> class.
        /// </summary>
        public FakeConnection()
        {
        }

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
        public virtual string DataSource
        {
            get { return "FakeProvider"; }
        }

        /// <summary>
        ///     Gets "42" :)
        /// </summary>
        public virtual string ServerVersion
        {
            get { return "42"; }
        }

        /// <summary>
        ///     Gets current state
        /// </summary>
        public ConnectionState CurrentState { get; set; }

        /// <summary>
        ///     Gets all commands that was created for this connection
        /// </summary>
        public List<FakeCommand> Commands
        {
            get { return _commands; }
        }

        #region IDbConnection Members

        /// <summary>
        /// Enqueue a command which should be returned by <see cref="CreateCommand"/>.
        /// </summary>
        /// <param name="command"></param>
        /// <remarks>You can enqueue several commands, just invoke this method several times.</remarks>
        public void Setup(FakeCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            _commandsToReturn.Enqueue(command);
        }


        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>
        ///     Invokes <see cref="Reset" /> and sets <see cref="IsDisposed" />
        /// </remarks>
        public virtual void Dispose()
        {
            Reset();
            IsDisposed = true;
        }


        /// <summary>
        ///     Begins a database transaction.
        /// </summary>
        /// <returns>
        ///     An object representing the new transaction.
        /// </returns>
        /// <seealso cref="Factory.CreateTransaction(FakeConnection)" />
        public virtual IDbTransaction BeginTransaction()
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
        public virtual IDbTransaction BeginTransaction(IsolationLevel il)
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
        public virtual void Close()
        {
            CurrentState = ConnectionState.Closed;
            Reset();
        }


        /// <summary>
        ///     Changes the current database for an open Connection object.
        /// </summary>
        /// <param name="databaseName">The name of the database to use in place of the current database.</param>
        /// <seealso cref="CurrentDatabase" />
        public virtual void ChangeDatabase(string databaseName)
        {
            CurrentDatabase = databaseName;
        }

        /// <summary>
        ///     Creates and returns a Command object associated with the connection. Uses commands from <see cref="Setup" /> if configured,
        ///     otherwise <see cref="Factory.CreateCommand" />.
        /// </summary>
        /// <returns>
        ///     A Command object associated with the connection.
        /// </returns>
        /// <remarks>
        ///     Adds command to <see cref="Commands" />.
        /// </remarks>
        /// <seealso cref="Factory.CreateCommand" />
        public virtual IDbCommand CreateCommand()
        {
            var command = _commandsToReturn.Count > 0
                              ? _commandsToReturn.Dequeue()
                              : Factory.Instance.CreateCommand(this);

            Commands.Add(command);
            return command;
        }


        /// <summary>
        ///     Opens a database connection with the settings specified by the ConnectionString property of the provider-specific Connection object.
        /// </summary>
        public virtual void Open()
        {
            CurrentState = ConnectionState.Open;
        }

        /// <summary>
        ///     Gets or sets the string used to open a database.
        /// </summary>
        /// <returns>A string containing connection settings.</returns>
        public virtual string ConnectionString { get; set; }

        /// <summary>
        ///     Gets the time to wait while trying to establish a connection before terminating the attempt and generating an error.
        /// </summary>
        /// <returns>The time (in seconds) to wait for a connection to open. The default value is 15 seconds.</returns>
        public int ConnectionTimeout { get; set; }

        /// <summary>
        ///     Gets the name of the current database or the database to be used after a connection is opened.
        /// </summary>
        /// <returns>The name of the current database or the name of the database to be used once a connection is open. The default value is an empty string.</returns>
        public virtual string Database
        {
            get { return CurrentDatabase; }
        }

        /// <summary>
        ///     Gets the current state of the connection.
        /// </summary>
        /// <returns>
        ///     One of the <see cref="T:System.Data.ConnectionState" /> values.
        /// </returns>
        public virtual ConnectionState State
        {
            get { return CurrentState; }
        }

        /// <summary>
        /// Gets all transactions which have been created for this connection.
        /// </summary>
        public FakeTransaction[] Transactions
        {
            get { return _transactions.ToArray(); }
        }

        #endregion

        /// <summary>
        ///     reset state flags
        /// </summary>
        public virtual void Reset()
        {
            CurrentState = ConnectionState.Closed;
        }
    }
}