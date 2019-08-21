namespace TurtlePost.Operations
{
    /// <summary>
    /// Represents a TurtlePost operation.
    /// </summary>
    public abstract class Operation
    {
        /// <summary>
        /// Performs the operation.
        /// </summary>
        /// <param name="interpreter">The interpreter which invoked this operation.</param>
        /// <param name="diagnostic">A diagnostic which can be used to report any errors.</param>
        public abstract void Operate(Interpreter interpreter, ref Diagnostic diagnostic);
    }
}
