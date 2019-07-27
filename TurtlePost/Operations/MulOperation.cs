using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class MulOperation : Operation
    {
        MulOperation()
        {
        }

        public static MulOperation Instance { get; } = new MulOperation();

        public override void Operate(Stack<object?> stack, GlobalBag _)
        {
            var v1 = (double) stack.Pop()!;
            var v2 = (double) stack.Pop()!;
            stack.Push(v1 * v2);
        }
    }
}