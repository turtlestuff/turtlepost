namespace TurtlePost.Operations
{
    class PushOperation : Operation
    {
        PushOperation()
        {
        }

        public static PushOperation Instance { get; } = new PushOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopA<object>(ref diagnostic, out var value)) return;
            if (interpreter.TryPopA<List>(ref diagnostic, out var list))
                list.Push(value);
        }
    }
}
