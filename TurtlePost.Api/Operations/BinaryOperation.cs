using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    public abstract class BinaryOperation<TTop, TBottom, TResult> : Operation
    {
        protected abstract TResult Operate(TTop top, TBottom bottom, Interpreter interpreter, ref Diagnostic diagnostic);
        
        public sealed override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopA<TTop>(ref diagnostic, out var top)) return;
            if (!interpreter.TryPopA<TBottom>(ref diagnostic, out var bottom)) return;

            interpreter.UserStack.Push(Operate(top, bottom, interpreter, ref diagnostic));
        }
    }
}
