using System.Data;

namespace Griffin.AdoNetFakes
{
    /// <summary>
    /// Fake transaction
    /// </summary>
    public class FakeTransaction : IDbTransaction
    {
        private readonly IDbConnection _connection;
        private readonly IsolationLevel _isolationLevel;

        public FakeTransaction(IDbConnection connection)
        {
            _connection = connection;
        }

        public FakeTransaction(IDbConnection connection, IsolationLevel il)
        {
            _connection = connection;
            _isolationLevel = il;
        }

        /// <summary>
        /// Gets or sets if <see cref="Dispose"/> was called.
        /// </summary>
        public bool IsDisposed { get; set; }

        /// <summary>
        /// Gets or sets if <see cref="Commit"/> was called
        /// </summary>
        public bool IsCommitted { get; set; }

        /// <summary>
        /// Gets or sets if <see cref="Rollback"/> was called.
        /// </summary>
        public bool IsRolledBack { get; set; }

        #region IDbTransaction Members

        /// <summary>
        /// Specifies the Connection object to associate with the transaction.
        /// </summary>
        /// <returns>The Connection object to associate with the transaction.</returns>
        public virtual IDbConnection Connection
        {
            get { return _connection; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <seealso cref="IsDisposed"/>
        public virtual void Dispose()
        {
            IsDisposed = true;
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        /// <exception cref="T:System.Exception">An error occurred while trying to commit the transaction. </exception>
        ///   
        /// <exception cref="T:System.InvalidOperationException">The transaction has already been committed or rolled back.-or- The connection is broken. </exception>
        /// <seealso cref="IsCommitted"/>
        public virtual void Commit()
        {
            IsCommitted = true;
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        /// <exception cref="T:System.Exception">An error occurred while trying to commit the transaction. </exception>
        ///   
        /// <exception cref="T:System.InvalidOperationException">The transaction has already been committed or rolled back.-or- The connection is broken. </exception>
        /// <seealso cref="IsRolledBack"/>
        public virtual void Rollback()
        {
            IsRolledBack = true;
        }

        /// <summary>
        /// Specifies the <see cref="T:System.Data.IsolationLevel"/> for this transaction.
        /// </summary>
        /// <returns>The <see cref="T:System.Data.IsolationLevel"/> for this transaction. The default is ReadCommitted.</returns>
        public virtual IsolationLevel IsolationLevel
        {
            get { return _isolationLevel; }
        }

        #endregion

        /// <summary>
        /// Reset flags
        /// </summary>
        public virtual void Reset()
        {
            IsCommitted = false;
            IsRolledBack = false;
        }
    }
}