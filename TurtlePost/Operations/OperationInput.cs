using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class OperationInput : Operation
    {
        OperationInput()
        {

        }

        public static OperationInput Instance { get; } = new OperationInput();

        public override void Operate(Stack<object> stack, Dictionary<Global, object> globals)
        {
            stack.Push(Console.ReadLine());
        }

    }
}
