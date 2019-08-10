using System;

namespace TurtlePost.Operations
{
    class FloorOperation : UnaryOperation<double, double>
    {
        FloorOperation() { }

        public static FloorOperation Instance { get; } = new FloorOperation();
        
        protected override double Operate(double value, Interpreter interpreter, ref Diagnostic diagnostic) =>
            Math.Floor(value);
    }
}
