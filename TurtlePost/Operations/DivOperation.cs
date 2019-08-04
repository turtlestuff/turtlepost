using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class DivOperation : Operation
    {
        DivOperation()
        {
        }

        public static DivOperation Instance = new DivOperation();

        public override void Operate(Interpreter interpreter)
        {
            var v2 = (double) interpreter.UserStack.Pop()!;
            var v1 = (double) interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(v1 / v2);
        }
    }
}