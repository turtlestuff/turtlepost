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
            if (!interpreter.TryPopA<Global>(ref diagnostic, out var global)) return;
            interpreter.UserStack.Push(global.Value);
        }
    }
}