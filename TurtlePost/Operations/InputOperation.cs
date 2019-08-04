using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class InputOperation : Operation
    {
        InputOperation()
        {
        }

        public static InputOperation Instance { get; } = new InputOperation();

        public override void Operate(Interpreter interpreter)
        {
            interpreter.UserStack.Push(Console.ReadLine());
        }
    }
}