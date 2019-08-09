using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Api.Operations
{
    class HeightOperation : Operation
    {
        HeightOperation() { }

        public static HeightOperation Instance { get; } = new HeightOperation();

        public override void Operate(Interpreter interpreter)
        {
            interpreter.UserStack.Push((double)Console.WindowHeight);
        }
    }
}
