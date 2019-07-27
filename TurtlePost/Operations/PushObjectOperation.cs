using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class PushObjectOperation : Operation
    {
        public object Object { get; }

        //peekaboo! this one is only used internally!
        public PushObjectOperation(object obj)
        {
            Object = obj;
        }

        public override void Operate(Stack<object?> stack, GlobalBag _)
        {
            stack.Push(Object);
        }
    }
}