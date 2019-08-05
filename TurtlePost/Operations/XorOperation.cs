using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class XorOperation : Operation
    {
        XorOperation()
        {
        }

        public static XorOperation Instance { get; } = new XorOperation();

        public override void Operate(Interpreter interpreter)
        {
            var v2 = (bool) interpreter.UserStack.Pop()!;
            var v1 = (bool) interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(v1 ^ v2);
        }
    }
}