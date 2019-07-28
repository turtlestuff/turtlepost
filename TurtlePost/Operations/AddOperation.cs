using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class AddOperation : Operation
    {
        AddOperation()
        {
        }

        public static AddOperation Instance { get; } = new AddOperation();

        public override void Operate(Stack<object?> stack)
        {
            double v1 = (double) stack.Pop()!;
            double v2 = (double) stack.Pop()!;
            stack.Push(v1 + v2);
        }
    }
}
