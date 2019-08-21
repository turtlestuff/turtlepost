using System.Collections.Generic;

namespace TurtlePost.Operations
{
    class DeleteOperation : Operation
    {
        DeleteOperation()
        {
        }

        public static DeleteOperation Instance { get; } = new DeleteOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopA<double>(ref diagnostic, out var index)) return;
            if (!interpreter.TryPopA<List>(ref diagnostic, out var list)) return;
            list.RemoveAt((int) index, ref diagnostic);
        }
    }
}