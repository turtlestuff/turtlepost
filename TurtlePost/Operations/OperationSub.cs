using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class OperationSub : Operation
    {
        OperationSub()
        {
        }

        public static OperationSub Instance { get; } = new OperationSub();

        public override void Operate(Stack<Object> stack)
        {
            double v2 = (double)stack.Pop();
            double v1 = (double)stack.Pop();
            stack.Push(v1 - v2);

        }
    }
}
