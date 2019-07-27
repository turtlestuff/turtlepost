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

        public override void Operate(Stack<Object> stack, Dictionary<Global, Object> globals)
        {
            double v1 = (double)stack.Pop();
            double v2 = (double)stack.Pop();
            stack.Push(v1 + v2);
        }
    }
}
