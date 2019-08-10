using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    public abstract class UnaryOperation<TValue, TResult> : Operation
    {
        protected abstract TResult Operate(TValue value, Interpreter interpreter, ref Diagnostic diagnostic);
        
        public sealed override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopA<TValue>(ref diagnostic, out var value)) return;

            interpreter.UserStack.Push(Operate(value, interpreter, ref diagnostic));
        }
    }
}
