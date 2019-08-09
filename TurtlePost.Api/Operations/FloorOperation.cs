using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Api.Operations
{
    class FloorOperation : Operation
    {
        FloorOperation() { }

        public static FloorOperation Instance { get; } = new FloorOperation();

        public override void Operate(Interpreter interpreter)
        {
            double round = (double)interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(Math.Floor(round));
        }
    }
}
