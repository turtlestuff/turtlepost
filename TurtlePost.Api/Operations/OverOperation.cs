using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurtlePost.Operations
{
    class OverOperation : Operation
    {
        OverOperation() { }

        public static OverOperation Instance { get; } = new OverOperation();

        public override void Operate(Interpreter interpreter)
        {
            object? o = interpreter.UserStack.ElementAt(interpreter.UserStack.Count - 1);
            interpreter.UserStack.Push(o);
        }
    }
}
