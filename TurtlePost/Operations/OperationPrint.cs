using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class OperationPrintLine : Operation
    {
        OperationPrintLine()
        {
        }

        public static OperationPrintLine Instance { get; } = new OperationPrintLine();
       

        public override void Operate(Stack<object> stack, Dictionary<Global, object> globals)
        {
            Console.WriteLine(stack.Pop());

        }
    }
}
