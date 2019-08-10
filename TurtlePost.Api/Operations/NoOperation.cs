using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class NoOperation : Operation
    {
        NoOperation()
        {
        }

        public static NoOperation Instance { get; } = new NoOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
        }
    }
}