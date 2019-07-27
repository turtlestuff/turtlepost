using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class PushOperation : Operation
    {
        PushOperation()
        {
        }

        public static PushOperation Instance { get; } = new PushOperation();

        public override void Operate(Stack<object?> stack, GlobalBag globals)
        {
            var global = (Global) stack.Pop()!;
            stack.Push(globals[global]);
        }
    }
}