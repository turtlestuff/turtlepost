using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Api.Operations
{
    class RoundOperation : Operation
    {
        RoundOperation() { }

        public static RoundOperation Instance { get; } = new RoundOperation();

        public override void Operate(Interpreter interpreter)
        {
            double round = (double)interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(Math.Round(round));
        }
    }
}
