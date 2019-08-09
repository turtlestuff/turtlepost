using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class GreaterThanOrEqualToOperation : Operation
    {
        GreaterThanOrEqualToOperation()
        {
        }

        public static GreaterThanOrEqualToOperation Instance { get; } = new GreaterThanOrEqualToOperation();

        public override void Operate(Interpreter interpreter)
        {
            var v1 = (double) interpreter.UserStack.Pop()!;
            var v2 = (double) interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(v2 >= v1);
        }
    }
}