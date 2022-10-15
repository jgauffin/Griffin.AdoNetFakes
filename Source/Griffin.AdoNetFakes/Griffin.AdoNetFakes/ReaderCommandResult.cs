namespace Griffin.AdoNetFakes;

/// <summary>
///     Return value for a <see cref="FakeCommand.ExecuteReader()" />
/// </summary>
public class ReaderCommandResult : CommandResult
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ReaderCommandResult" /> class.
    /// </summary>
    /// <param name="commandText">The command text.</param>
    /// <param name="parameters">The parameters.</param>
    /// <remarks>
    ///     Use this constructor together with <see cref="FakeCommand.Setup" /> to enable
    ///     automatic validation of executed commands.
    /// </remarks>
    public ReaderCommandResult(string commandText, FakeDbParameterCollection parameters)
        : base(commandText, parameters)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ReaderCommandResult" /> class.
    /// </summary>
    /// <remarks>
    ///     Use this constructor if you are going to just return a result but don't want automatic validation
    /// </remarks>
    public ReaderCommandResult()
    {
    }

    /// <summary>
    ///     Reader which will be returned.
    /// </summary>
    public FakeDataReader Result { get; set; }
}
