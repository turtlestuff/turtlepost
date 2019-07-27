using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class OperationPrint : Operation
    {
        OperationPrint()
        {
        }

        public static OperationPrint Instance { get; } = new OperationPrint();
       

        public override void Operate(Stack<object> stack, Dictionary<Global, object> globals)
        {
            Console.Write(stack.Pop());

        }
    }
}
