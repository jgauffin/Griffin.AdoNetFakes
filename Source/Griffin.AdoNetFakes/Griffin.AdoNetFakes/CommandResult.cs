namespace Griffin.AdoNetFakes;

/// <summary>
///     A commandResult executed on a <see cref="FakeCommand" />
/// </summary>
public class CommandResult
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandResult" /> class.
    /// </summary>
    /// <param name="commandText">The command text.</param>
    /// <param name="parameters">The parameters.</param>
    /// <remarks>
    ///     Use this constructor together with <see cref="FakeCommand.Setup" /> to enable
    ///     automatic validation of executed commands.
    /// </remarks>
    public CommandResult(string commandText, FakeDbParameterCollection parameters)
    {
        CommandText = commandText;
        Parameters = parameters;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandResult" /> class.
    /// </summary>
    public CommandResult()
    {
    }

    /// <summary>
    ///     Gets text for this commandResult
    /// </summary>
    /// <remarks>Set it to enable automatic validation of the text. Else leave it empty</remarks>
    public string CommandText { get; set; }

    /// <summary>
    ///     Gets or sets parameters used in this query
    /// </summary>
    /// <remarks>Set it to enable validation of any missing/incorrect parameter(s). </remarks>
    public FakeDbParameterCollection Parameters { get; set; }
}
