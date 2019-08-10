using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class StringOperation : Operation
    {
        StringOperation()
        {
        }

        public static StringOperation Instance { get; } = new StringOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopAny(ref diagnostic, out var value)) return;
            interpreter.UserStack.Push(value?.ToString() ?? string.Empty);
        }
    }
}