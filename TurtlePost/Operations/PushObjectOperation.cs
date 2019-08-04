using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class PushObjectOperation : Operation
    {
        public object? Object { get; }

        //peekaboo! this one is only used internally!
        public PushObjectOperation(object? obj)
        {
            Object = obj;
        }

        public override void Operate(Interpreter interpreter)
        {
            interpreter.UserStack.Push(Object);
        }
    }
}