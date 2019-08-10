using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class CallOperation : Operation
    {
        CallOperation()
        {
        }

        public static CallOperation Instance { get; } = new CallOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopA<Label>(ref diagnostic, out var label)) return;
            interpreter.CallStack.Push(interpreter.Enumerator.Position);
            interpreter.Enumerator.Position = label.Position - 1;
        }
    }
}
