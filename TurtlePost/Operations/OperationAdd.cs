using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class OperationAdd : Operation
    {
        OperationAdd()
        {
        }

        public static OperationAdd Instance { get; } = new OperationAdd();

        public override void Operate(Stack<dynamic> stack)
        {
            double v1 = stack.Pop();
            double v2 = stack.Pop();
            stack.Push(v1 + v2);
        }
    }
}
