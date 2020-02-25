using System;

namespace TurtlePost.Operations
{
    class HeightOperation : Operation
    {
        HeightOperation() { }

        public static HeightOperation Instance { get; } = new HeightOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            interpreter.UserStack.Push((double) Console.WindowHeight);
        }
    }
}
