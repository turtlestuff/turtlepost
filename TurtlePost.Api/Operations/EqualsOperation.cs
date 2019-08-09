using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class EqualsOperation : Operation
    {
        EqualsOperation()
        {
        }

        public static EqualsOperation Instance { get; } = new EqualsOperation();

        public override void Operate(Interpreter interpreter)
        {
            var v1 = interpreter.UserStack.Pop()!;
            var v2 = interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(v2.Equals(v1));
        }
    }
}