namespace TurtlePost.Operations
{
    class DropOperation : Operation
    {
        DropOperation()
        {
        }

        public static DropOperation Instance { get; } = new DropOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            interpreter.TryPopA<object>(ref diagnostic, out _);
        }
    }
}
