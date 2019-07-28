using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class SubOperation : Operation
    {
        SubOperation()
        {
        }

        public static SubOperation Instance { get; } = new SubOperation();

        public override void Operate(Stack<object?> stack)
        {
            var v2 = (double) stack.Pop()!;
            var v1 = (double) stack.Pop()!;
            stack.Push(v1 - v2);
        }
    }
}