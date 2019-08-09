using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Api.Operations
{
    class SineOperation : Operation
    {
        SineOperation() { }

        public static SineOperation Instance { get; } = new SineOperation();

        public override void Operate(Interpreter interpreter)
        {
            double x = (double)interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(Math.Sin(x));
        }

    }
}
