using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class OperationNone : Operation
    {
        OperationNone()
        {
        }

        public static OperationNone Instance { get; } = new OperationNone();

        public override void Operate(Stack<dynamic> stack)
        {
        }
    }
}
