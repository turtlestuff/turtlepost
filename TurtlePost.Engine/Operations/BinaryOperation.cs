namespace TurtlePost.Operations
{
    /// <summary>
    /// A helper class that represents a binary TurtlePost operation.
    /// </summary>
    /// <typeparam name="TTop">The type of the top (first) parameter that will be popped.</typeparam>
    /// <typeparam name="TBottom">The type of the top (first) parameter that will be popped.</typeparam>
    /// <typeparam name="TResult">The type of the resulting object that will be pushed to the user stack.</typeparam>
    /// <remarks>
    /// An operation like 'add' takes in two inputs, performs an operation, and returns one in result. <see cref="BinaryOperation{TTop, TBottom, TResult}"/> provides an abstraction to make writing these kinds of operations easier by popping and type checking the required parameters.   
    /// </remarks>
    public abstract class BinaryOperation<TTop, TBottom, TResult> : Operation
    {
        /// <summary>
        /// Performs the operation.
        /// </summary>
        /// <param name="top">The top (first) parameter popped from the user stack.</param>
        /// <param name="bottom">The top (first) parameter popped from the user stack.</param>
        /// <param name="interpreter">The interpreter which invoked this operation.</param>
        /// <param name="diagnostic">A diagnostic which can be used to report errors.</param>
        /// <returns>The resulting value which will be pushed to the stack.</returns>
        protected abstract TResult Operate(TTop top, TBottom bottom, Interpreter interpreter, ref Diagnostic diagnostic);
        
        /// <inheritdoc />
        public sealed override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (interpreter is null)
                throw new System.ArgumentNullException(nameof(interpreter));

            if (!interpreter.TryPopA<TTop>(ref diagnostic, out var top)) return;
            if (!interpreter.TryPopA<TBottom>(ref diagnostic, out var bottom)) return;

            interpreter.UserStack.Push(Operate(top, bottom, interpreter, ref diagnostic));
        }
    }
}
