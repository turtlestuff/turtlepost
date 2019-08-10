namespace TurtlePost.Operations
{
    class MultiplyOperation : BinaryOperation<double, double, double>
    {
        MultiplyOperation()
        {
        }

        public static MultiplyOperation Instance { get; } = new MultiplyOperation();

        protected override double Operate(double top, double bottom, Interpreter interpreter,
            ref Diagnostic diagnostic) => bottom * top;
    }
}