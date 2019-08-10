using System;

namespace TurtlePost.Operations
{
    class RoundOperation : UnaryOperation<double, double>
    {
        RoundOperation() { }

        public static RoundOperation Instance { get; } = new RoundOperation();
        
        protected override double Operate(double value, Interpreter interpreter, ref Diagnostic diagnostic) =>
            Math.Round(value);
    }
}
