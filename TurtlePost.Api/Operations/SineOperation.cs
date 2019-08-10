using System;

namespace TurtlePost.Operations
{
    class SineOperation : UnaryOperation<double, double>
    {
        SineOperation() { }

        public static SineOperation Instance { get; } = new SineOperation();
        
        protected override double Operate(double value, Interpreter interpreter, ref Diagnostic diagnostic) =>
            Math.Sin(value);
    }
}
