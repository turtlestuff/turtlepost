using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class PushOperation : Operation
    {
        PushOperation()
        {
        }

        public static PushOperation Instance { get; } = new PushOperation();

        public override void Operate(Interpreter interpreter)
        {
            var global = (Global) interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(global.Value);
        }
    }
}