namespace TurtlePost.Operations
{
    class GreaterThanOperation : BinaryOperation<double, double, bool>
    {
        GreaterThanOperation()
        {
        }

        public static GreaterThanOperation Instance { get; } = new GreaterThanOperation();

        protected override bool Operate(double top, double bottom, Interpreter interpreter,
            ref Diagnostic diagnostic) => bottom > top;
    }
}