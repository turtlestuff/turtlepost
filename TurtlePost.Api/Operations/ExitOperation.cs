using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class ExitOperation : Operation
    {
        ExitOperation()
        {
        }

        public static ExitOperation Instance { get; } = new ExitOperation();

        public override void Operate(Interpreter interpreter)
        {
            Environment.Exit(0);
        }
    }
}