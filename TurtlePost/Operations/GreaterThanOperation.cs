using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class GreaterThanOperation : Operation
    {
        GreaterThanOperation()
        {
        }

        public static GreaterThanOperation Instance { get; } = new GreaterThanOperation();

        public override void Operate(Interpreter interpreter)
        {
            var v1 = (double) interpreter.UserStack.Pop()!;
            var v2 = (double) interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(v2 > v1);
        }
    }
}