﻿namespace TurtlePost.Operations
{
    class OverOperation : Operation
    {
        OverOperation() { }

        public static OverOperation Instance { get; } = new OverOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopA<object>(ref diagnostic, out var top)) return;
            if (!interpreter.TryPopA<object>(ref diagnostic, out var bottom)) return;
            interpreter.UserStack.Push(bottom);
            interpreter.UserStack.Push(top);
            interpreter.UserStack.Push(bottom);
        }
    }
}
