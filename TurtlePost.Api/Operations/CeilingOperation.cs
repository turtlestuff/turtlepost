using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Api.Operations
{
    class CeilingOperation : Operation
    {
        CeilingOperation() { }

        public static CeilingOperation Instance { get; } = new CeilingOperation();

        public override void Operate(Interpreter interpreter)
        {
            double round = (double)interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(Math.Ceiling(round));
        }
    }
}
