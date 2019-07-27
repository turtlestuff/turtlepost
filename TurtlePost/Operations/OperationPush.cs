using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class OperationPush : Operation
    {  
        public dynamic Object { get; }

        public OperationPush(dynamic obj)
        {
            Object = obj;
        }

        public override void Operate(Stack<dynamic> stack)
        {
            stack.Push(Object);
        }
    }
}
