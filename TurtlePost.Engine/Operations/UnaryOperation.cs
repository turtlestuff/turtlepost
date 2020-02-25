namespace TurtlePost.Operations
{
    /// <summary>
    /// A helper class that represents a unary TurtlePost operation.
    /// </summary>
    /// <typeparam name="TValue">The type of the parameter that will be popped.</typeparam>
    /// <typeparam name="TResult">The type of the resulting object that will be pushed to the user stack.</typeparam>
    /// <remarks>
    /// An operation like 'square root' takes in one input, performs an operation, and returns one result. <see cref="UnaryOperation{TValue, TResult}"/> provides an abstraction to make writing these kinds of operations easier by popping and type checking the required parameter.   
    /// </remarks>
    public abstract class UnaryOperation<TValue, TResult> : Operation
    {
        /// <summary>
        /// Performs the operation.
        /// </summary>
        /// <param name="value">The parameter popped from the user stack.</param>
        /// <param name="interpreter">The interpreter which invoked this operation.</param>
        /// <param name="diagnostic">A diagnostic which can be used to report errors.</param>
        /// <returns>The resulting value which will be pushed to the stack.</returns>
        protected abstract TResult Operate(TValue value, Interpreter interpreter, ref Diagnostic diagnostic);
        
        /// <inheritdoc />
        public sealed override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (interpreter is null)
                throw new System.ArgumentNullException(nameof(interpreter));

            if (!interpreter.TryPopA<TValue>(ref diagnostic, out var value)) return;

            interpreter.UserStack.Push(Operate(value, interpreter, ref diagnostic));
        }
    }
}
