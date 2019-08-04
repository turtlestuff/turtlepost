using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class DuplicateOperation : Operation
    {
        DuplicateOperation() { }

        public static DuplicateOperation Instance { get; } = new DuplicateOperation();

        public override void Operate(Interpreter interpreter)
        {
            object? duplicating = interpreter.UserStack.Pop();
            interpreter.UserStack.Push(duplicating);
            interpreter.UserStack.Push(duplicating);
        }
    }
}
