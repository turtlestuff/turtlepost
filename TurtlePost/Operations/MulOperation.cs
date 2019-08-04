using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class MulOperation : Operation
    {
        MulOperation()
        {
        }

        public static MulOperation Instance { get; } = new MulOperation();

        public override void Operate(Interpreter interpreter)
        {
            var v1 = (double) interpreter.UserStack.Pop()!;
            var v2 = (double) interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(v1 * v2);
        }
    }
}