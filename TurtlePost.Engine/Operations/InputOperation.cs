﻿using System;

namespace TurtlePost.Operations
{
    class InputOperation : Operation
    {
        InputOperation()
        {
        }

        public static InputOperation Instance { get; } = new InputOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            interpreter.UserStack.Push(Console.ReadLine() ?? "");
        }
    }
}