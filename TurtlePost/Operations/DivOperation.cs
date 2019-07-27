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

        public override void Operate(Stack<object?> stack, GlobalBag _)
        {
            var v2 = (double) stack.Pop()!;
            var v1 = (double) stack.Pop()!;
            stack.Push(v1 / v2);
        }
    }
}