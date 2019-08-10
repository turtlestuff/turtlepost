namespace TurtlePost.Operations
{
    class AndOperation : BinaryOperation<bool, bool, bool>
    {
        AndOperation()
        {
        }

        public static AndOperation Instance { get; } = new AndOperation();

        protected override bool Operate(bool top, bool bottom, Interpreter interpreter,
            ref Diagnostic diagnostic) => bottom && top;
    }
}