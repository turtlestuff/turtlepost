namespace TurtlePost.Operations
{
    class SwapOperation : Operation
    {
        SwapOperation() {  }

        public static SwapOperation Instance { get; } = new SwapOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopA<object>(ref diagnostic, out var top)) return;
            if (!interpreter.TryPopA<object>(ref diagnostic, out var bottom)) return;
            interpreter.UserStack.Push(top);
            interpreter.UserStack.Push(bottom);
        }
    }
}
