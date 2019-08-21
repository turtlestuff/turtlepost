namespace TurtlePost.Operations
{
    class PopOperation : UnaryOperation<List, object?>
    {
        PopOperation()
        {
        }

        public static PopOperation Instance { get; } = new PopOperation();

        protected override object? Operate(List value, Interpreter interpreter, ref Diagnostic diagnostic) =>
            value.TryPopA<object>(ref diagnostic, out var var) ? var : null;
    }
}
