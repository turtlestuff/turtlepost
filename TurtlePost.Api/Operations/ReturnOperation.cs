
using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class ReturnOperation : Operation
    {
        ReturnOperation() { }

        public static ReturnOperation Instance = new ReturnOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopStackFrame(ref diagnostic, out var location)) return;
            interpreter.Enumerator.Position = location;
        }
    }
}
