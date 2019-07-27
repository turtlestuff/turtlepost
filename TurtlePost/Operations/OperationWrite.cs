using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class OperationWrite : Operation
    {
        OperationWrite()
        {
        }

        public static OperationWrite Instance { get; } = new OperationWrite();

        public override void Operate(Stack<Object> stack, Dictionary<Global, Object> globals)
        {
            Global globalPointer = (Global)stack.Pop();
            Object value = stack.Pop();
            if(globals.TryGetValue(globalPointer, out _))
            {
                //the global already exists. now we just write to it
                globals[globalPointer] = value;

            }
            else
            {
                //the global doesn't exist, so we create it
                globals.Add(globalPointer, value);
            }
        }
    }
}
