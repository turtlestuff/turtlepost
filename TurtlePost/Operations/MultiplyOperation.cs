using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class MultiplyOperation : Operation
    {
        MultiplyOperation()
        {
        }

        public static MultiplyOperation Instance { get; } = new MultiplyOperation();

        public override void Operate(Interpreter interpreter)
        {
            var v1 = (double) interpreter.UserStack.Pop()!;
            var v2 = (double) interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(v1 * v2);
        }
    }
}