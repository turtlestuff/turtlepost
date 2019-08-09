using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Api.Operations
{
    class ClearOperation : Operation
    {
        ClearOperation() { }

        public static ClearOperation Instance { get; } = new ClearOperation();

        public override void Operate(Interpreter interpreter)
        {
            Console.Clear();
        }
    }
}
