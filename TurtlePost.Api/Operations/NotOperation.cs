using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class NotOperation : Operation
    {
        NotOperation()
        {
        }

        public static NotOperation Instance { get; } = new NotOperation();

        public override void Operate(Interpreter interpreter)
        {
            var value = (bool) interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(!value);
        }
    }
}