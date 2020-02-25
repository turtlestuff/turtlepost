using System;

namespace TurtlePost.Operations
{
    class CursorOperation : Operation
    {
        CursorOperation() { }

        public static CursorOperation Instance { get; } = new CursorOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopA<double>(ref diagnostic, out var y)) return;
            if (!interpreter.TryPopA<double>(ref diagnostic, out var x)) return;
            Console.SetCursorPosition((int) x, (int) y);
        }

    }
}
