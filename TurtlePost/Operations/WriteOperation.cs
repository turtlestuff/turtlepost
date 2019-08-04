using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class WriteOperation : Operation
    {
        WriteOperation()
        {
        }

        public static WriteOperation Instance { get; } = new WriteOperation();

        public override void Operate(Interpreter interpreter)
        {
            var global = (Global) interpreter.UserStack.Pop()!;
            var value = interpreter.UserStack.Pop();
            global.Value = value;
        }
    }
}