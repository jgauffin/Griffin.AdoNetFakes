using System.Data;
using System.Data.Common;

namespace Griffin.AdoNetFakes;

/// <summary>
///     Fake transaction
/// </summary>
public class FakeTransaction : DbTransaction
{
    private readonly IDbConnection _connection;

    public FakeTransaction(IDbConnection connection)
    {
        _connection = connection;
    }

    public FakeTransaction(IDbConnection connection, IsolationLevel il)
    {
        _connection = connection;
        IsolationLevel = il;
    }

    /// <summary>
    ///     Gets or sets if <see cref="Dispose" /> was called.
    /// </summary>
    public bool IsDisposed { get; set; }

    /// <summary>
    ///     Gets or sets if <see cref="Commit" /> was called
    /// </summary>
    public bool IsCommitted { get; set; }

    /// <summary>
    ///     Gets or sets if <see cref="Rollback" /> was called.
    /// </summary>
    public bool IsRolledBack { get; set; }

    /// <summary>
    ///     Reset flags
    /// </summary>
    public virtual void Reset()
    {
        IsCommitted = false;
        IsRolledBack = false;
    }

    #region IDbTransaction Members

    /// <summary>
    ///     Specifies the Connection object to associate with the transaction.
    /// </summary>
    /// <returns>The Connection object to associate with the transaction.</returns>
    public FakeConnection FakeConnection => (FakeConnection)_connection;

    protected override DbConnection DbConnection => (DbConnection)_connection;

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        IsDisposed = true;
        if (!IsCommitted)
        {
            IsRolledBack = true;
        }
    }

    /// <summary>
    ///     Commits the database transaction.
    /// </summary>
    /// <exception cref="T:System.Exception">An error occurred while trying to commit the transaction. </exception>
    /// <exception cref="T:System.InvalidOperationException">
    ///     The transaction has already been committed or rolled back.-or- The
    ///     connection is broken.
    /// </exception>
    /// <seealso cref="IsCommitted" />
    public override void Commit()
    {
        IsCommitted = true;
    }

    /// <summary>
    ///     Rolls back a transaction from a pending state.
    /// </summary>
    /// <exception cref="T:System.Exception">An error occurred while trying to commit the transaction. </exception>
    /// <exception cref="T:System.InvalidOperationException">
    ///     The transaction has already been committed or rolled back.-or- The
    ///     connection is broken.
    /// </exception>
    /// <seealso cref="IsRolledBack" />
    public override void Rollback()
    {
        IsRolledBack = true;
    }

    /// <summary>
    ///     Specifies the <see cref="T:System.Data.IsolationLevel" /> for this transaction.
    /// </summary>
    /// <returns>The <see cref="T:System.Data.IsolationLevel" /> for this transaction. The default is ReadCommitted.</returns>
    public override IsolationLevel IsolationLevel { get; }

    #endregion
}
