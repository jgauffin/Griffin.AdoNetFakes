namespace Griffin.AdoNetFakes
{
    /// <summary>
    /// Used by <see cref="FakeCommand.Setup"/> to be able to return a value from <see cref="FakeCommand.ExecuteScalar"/>
    /// </summary>
    public class ScalarCommandResult : CommandResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarCommandResult"/> class.
        /// </summary>
        /// <param name="commandText">The command text. Read the remarks section</param>
        /// <param name="parameters">The parameters. Read the remarks section</param>
        /// <remarks>
        /// Use this constructor together with <see cref="FakeCommand.Setup" /> to enable
        /// automatic validation of executed commands.
        /// </remarks>
        public ScalarCommandResult(string commandText, ParameterCollection parameters)
            : base(commandText, parameters)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarCommandResult"/> class.
        /// </summary>
        /// <remarks>
        /// Use this constructor if you are going to just return a result but don't want automatic validation
        /// </remarks>
        public ScalarCommandResult()
        {

        }

        /// <summary>
        /// Object result
        /// </summary>
        public object Result { get; set; }
    }
}