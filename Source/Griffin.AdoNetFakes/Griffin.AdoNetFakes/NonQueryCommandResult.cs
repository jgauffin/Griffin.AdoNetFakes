namespace Griffin.AdoNetFakes
{
    /// <summary>
    ///     Information about an executed command
    /// </summary>
    public class NonQueryCommandResult : CommandResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NonQueryCommandResult" /> class.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        public NonQueryCommandResult(string commandText, ParameterCollection parameters)
        {
            CommandText = commandText;
            Parameters = parameters;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonQueryCommandResult"/> class.
        /// </summary>
        /// <remarks>
        /// Use this constructor if you are going to just return a result but don't want automatic validation
        /// </remarks>
        public NonQueryCommandResult()
        {
            
        }

        /// <summary>
        ///     Gets or sets result for NonQueryCommand.
        /// </summary>
        public int Result { get; set; }
    }
}