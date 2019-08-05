using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class StringOperation : Operation
    {
        StringOperation()
        {
        }

        public static StringOperation Instance { get; } = new StringOperation();

        public override void Operate(Interpreter interpreter)
        {
            interpreter.UserStack.Push(interpreter.UserStack.Pop()!.ToString());
        }
    }
}