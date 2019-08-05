using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class SwapOperation : Operation
    {
        SwapOperation() {  }

        public static SwapOperation Instance { get; } = new SwapOperation();

        public override void Operate(Interpreter interpreter)
        {
            object? val1 = interpreter.UserStack.Pop();
            object? val2 = interpreter.UserStack.Pop();
            interpreter.UserStack.Push(val1);
            interpreter.UserStack.Push(val2);
        }
    }
}
