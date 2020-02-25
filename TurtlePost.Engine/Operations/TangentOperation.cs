using System;

namespace TurtlePost.Operations
{
    class TangentOperation : UnaryOperation<double, double>
    {
        TangentOperation() { }

        public static TangentOperation Instance { get; } = new TangentOperation();

        protected override double Operate(double value, Interpreter interpreter, ref Diagnostic diagnostic) =>
            Math.Tan(value);
    }
}
