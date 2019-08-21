namespace TurtlePost.Operations
{
    class ReadOperation : Operation
    {
        ReadOperation()
        {
        }

        public static ReadOperation Instance { get; } = new ReadOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopA<Global>(ref diagnostic, out var global)) return;
            interpreter.UserStack.Push(global.Value);
        }
    }
}