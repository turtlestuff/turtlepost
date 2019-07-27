using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class OperationPushVar : Operation
    {
        OperationPushVar()
        {
        }

        public static OperationPushVar Instance { get; } = new OperationPushVar();

        public override void Operate(Stack<Object> stack, Dictionary<Global, Object> globals)
        {
            Global globalPointer = (Global)stack.Pop();
            if (globals.TryGetValue(globalPointer, out var val))
            {
                //the global already exists. 
                stack.Push(val);

            }
            else
            {
                //the global doesn't exist, so we create it
                globals.Add(globalPointer, null);
                stack.Push(null);
                    

            }
        }
    }
}
