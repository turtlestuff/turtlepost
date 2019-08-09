using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class AndOperation : Operation
    {
        AndOperation()
        {
        }

        public static AndOperation Instance { get; } = new AndOperation();

        public override void Operate(Interpreter interpreter)
        {
            var v1 = (bool) interpreter.UserStack.Pop()!;
            var v2 = (bool) interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(v2 && v1);
        }
    }
}