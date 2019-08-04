using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class AddOperation : Operation
    {
        AddOperation()
        {
        }

        public static AddOperation Instance { get; } = new AddOperation();

        public override void Operate(Interpreter interpreter)
        {
            double v1 = (double) interpreter.UserStack.Pop()!;
            double v2 = (double) interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(v1 + v2);
        }
    }
}
