using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class OperationDiv : Operation
    {
        OperationDiv()
        {

        }

        public static OperationDiv Instance = new OperationDiv();

        public override void Operate(Stack<object> stack, Dictionary<Global, object> globals)
        {
            double v2 = (double)stack.Pop();
            double v1 = (double)stack.Pop();
            stack.Push(v1 / v2);
        }

    }
}
