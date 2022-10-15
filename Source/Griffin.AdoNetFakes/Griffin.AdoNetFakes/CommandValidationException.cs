using System;

namespace Griffin.AdoNetFakes;

/// <summary>
///     Thrown with the automatic validation fails
/// </summary>
public class CommandValidationException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandValidationException" /> class.
    /// </summary>
    /// <param name="errorMessage">Tells why validation failed.</param>
    /// <param name="command">Command that failed.</param>
    public CommandValidationException(string errorMessage, FakeCommand command)
        : base(errorMessage)
    {
        Command = command;
    }

    /// <summary>
    ///     Gets command that failed.
    /// </summary>
    public FakeCommand Command { get; set; }
}
