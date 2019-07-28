using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class PrintOperation : Operation
    {
        PrintOperation()
        {
        }

        public static PrintOperation Instance { get; } = new PrintOperation();


        public override void Operate(Stack<object?> stack)
        {
            Console.Write(stack.Pop());
        }
    }
}