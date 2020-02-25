namespace TurtlePost.Operations
{
    class WriteOperation : Operation
    {
        WriteOperation()
        {
        }

        public static WriteOperation Instance { get; } = new WriteOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopA<Global>(ref diagnostic, out var global)) return;
            if (!interpreter.TryPopA<object>(ref diagnostic, out var value)) return;
            global.Value = value;
        }
    }
}