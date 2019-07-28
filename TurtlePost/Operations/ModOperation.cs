using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class ModOperation : Operation
    {
        ModOperation()
        {
        }

        public static ModOperation Instance = new ModOperation();

        public override void Operate(Stack<object?> stack)
        {
            var v2 = (double) stack.Pop()!;
            var v1 = (double) stack.Pop()!;
            stack.Push(v1 % v2);
        }
    }
}