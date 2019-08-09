using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Api.Operations
{
    class TangentOperation : Operation
    {
        TangentOperation() { }

        public static TangentOperation Instance { get; } = new TangentOperation();

        public override void Operate(Interpreter interpreter)
        {
            double x = (double)interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(Math.Tan(x));
        }

    }
}
