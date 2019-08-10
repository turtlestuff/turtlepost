using System;

namespace TurtlePost.Operations
{
    class WidthOperation : Operation
    {
        WidthOperation() { }

        public static WidthOperation Instance { get; } = new WidthOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            interpreter.UserStack.Push((double) Console.WindowWidth);
        }
    }
}
