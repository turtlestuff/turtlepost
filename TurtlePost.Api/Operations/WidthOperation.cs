using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Api.Operations
{
    class WidthOperation : Operation
    {
        WidthOperation() { }

        public static WidthOperation Instance { get; } = new WidthOperation();

        public override void Operate(Interpreter interpreter)
        {
            interpreter.UserStack.Push((double)Console.WindowWidth);
        }
    }
}
