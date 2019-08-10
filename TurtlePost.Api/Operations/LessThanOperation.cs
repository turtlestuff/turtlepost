namespace TurtlePost.Operations
{
    class LessThanOperation : BinaryOperation<double, double, bool>
    {
        LessThanOperation()
        {
        }

        public static LessThanOperation Instance { get; } = new LessThanOperation();

        protected override bool Operate(double top, double bottom, Interpreter interpreter,
            ref Diagnostic diagnostic) => bottom < top;
    }
}