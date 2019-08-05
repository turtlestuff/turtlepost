using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class OrOperation : Operation
    {
        OrOperation()
        {
        }

        public static OrOperation Instance { get; } = new OrOperation();

        public override void Operate(Interpreter interpreter)
        {
            var v2 = (bool) interpreter.UserStack.Pop()!;
            var v1 = (bool) interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(v1 || v2);
        }
    }
}