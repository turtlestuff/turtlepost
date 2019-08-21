namespace TurtlePost.Operations
{
    class GetOperation : Operation
    {
        GetOperation()
        {
        }

        public static GetOperation Instance { get; } = new GetOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopA<double>(ref diagnostic, out var index)) return;
            if (!interpreter.TryPopA<List>(ref diagnostic, out var list)) return;
            if (list.GetAt((int) index, ref diagnostic, out var obj))
                interpreter.UserStack.Push(obj);
        }
    }
}