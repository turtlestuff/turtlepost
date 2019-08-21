using System;

namespace TurtlePost.Operations
{
    class SquareRootOperation : UnaryOperation<double, double>
    {
        SquareRootOperation()
        {
        }
        
        public static SquareRootOperation Instance { get; } = new SquareRootOperation();

        protected override double Operate(double value, Interpreter interpreter, ref Diagnostic diagnostic) =>
            Math.Sqrt(value);
    }
}