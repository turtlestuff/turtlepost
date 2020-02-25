namespace TurtlePost.Operations
{
    class OrOperation : BinaryOperation<bool, bool, bool>
    {
        OrOperation()
        {
        }

        public static OrOperation Instance { get; } = new OrOperation();

        protected override bool Operate(bool top, bool bottom, Interpreter interpreter,
            ref Diagnostic diagnostic) => bottom || top;
    }
}