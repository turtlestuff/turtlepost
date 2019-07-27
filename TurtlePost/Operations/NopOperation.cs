using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class NopOperation : Operation
    {
        NopOperation()
        {
        }

        public static NopOperation Instance { get; } = new NopOperation();

        public override void Operate(Stack<object?> stack, GlobalBag _)
        {
        }
    }
}