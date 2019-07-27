using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class OperationMul : Operation
    {
        OperationMul()
        {

        }

        public static OperationMul Instance { get; } = new OperationMul();

        public override void Operate(Stack<object> stack, Dictionary<Global, object> globals)
        {
            double v1 = (double)stack.Pop();
            double v2 = (double)stack.Pop();
            stack.Push(v1 * v2);
        }

    }
}
