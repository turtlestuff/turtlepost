using System;

namespace TurtlePost.Operations
{
    class PrintLineOperation : Operation
    {
        PrintLineOperation()
        {
        }

        public static PrintLineOperation Instance { get; } = new PrintLineOperation();
        
        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopAny(ref diagnostic, out var value)) return;
            Console.WriteLine(value?.ToString() ?? "null");
        }
    }
}