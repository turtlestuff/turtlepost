using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class DropOperation : Operation
    {
        DropOperation() { }

        public static DropOperation Instance { get; } = new DropOperation();

        public override void Operate(Interpreter interpreter)
        {
            interpreter.UserStack.Pop();
        }

    }
}
