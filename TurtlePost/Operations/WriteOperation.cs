using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class WriteOperation : Operation
    {
        WriteOperation()
        {
        }

        public static WriteOperation Instance { get; } = new WriteOperation();

        public override void Operate(Stack<object?> stack, GlobalBag globals)
        {
            var global = (Global) stack.Pop()!;
            var value = stack.Pop();

            globals[global] = value;
        }
    }
}