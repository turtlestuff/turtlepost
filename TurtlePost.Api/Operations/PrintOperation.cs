using System;

namespace TurtlePost.Operations
{
    class PrintOperation : Operation
    {
        PrintOperation()
        {
        }

        public static PrintOperation Instance { get; } = new PrintOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopAny(ref diagnostic, out var value)) return;
            Console.Write(value);
        }
    }
}