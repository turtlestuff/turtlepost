using System;

namespace TurtlePost.Operations
{
    class ClearOperation : Operation
    {
        ClearOperation() { }

        public static ClearOperation Instance { get; } = new ClearOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            Console.Clear();
        }
    }
}
