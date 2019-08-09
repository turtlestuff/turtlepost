using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Api.Operations
{
    class CosineOperation : Operation
    {
        CosineOperation() { }

        public static CosineOperation Instance { get; } = new CosineOperation();

        public override void Operate(Interpreter interpreter)
        {
            double x = (double)interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(Math.Cos(x));
        }

    }
}
