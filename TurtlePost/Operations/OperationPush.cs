using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class OperationPush : Operation
    {  
        public dynamic Object { get; }
        //peekaboo! this one is only used internally!
        public OperationPush(Object obj)
        {
            Object = obj;
        }

        public override void Operate(Stack<Object> stack)
        {
            stack.Push(Object);
        }
    }
}
