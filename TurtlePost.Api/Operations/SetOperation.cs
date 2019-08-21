using System.Collections.Generic;

namespace TurtlePost.Operations
{
    class SetOperation : Operation
    {
        SetOperation()
        {
        }

        public static SetOperation Instance { get; } = new SetOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopA<double>(ref diagnostic, out var index)) return;
            if (!interpreter.TryPopA<object?>(ref diagnostic, out var obj)) return;
            if (!interpreter.TryPopA<List>(ref diagnostic, out var list)) return;
            list.SetAt((int) index, ref diagnostic, obj);
        }
    }
}