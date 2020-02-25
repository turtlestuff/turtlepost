using System;

namespace TurtlePost.Operations
{
    class HelpOperation : Operation
    {
        HelpOperation() { }

        public static HelpOperation Instance { get; } = new HelpOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            Console.WriteLine(string.Join(" ", interpreter.Operations.Keys));
        }
    }
}
