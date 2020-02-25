using System;

namespace TurtlePost.Operations
{
    class CosineOperation : UnaryOperation<double, double>
    {
        CosineOperation() { }

        public static CosineOperation Instance { get; } = new CosineOperation();
        
        protected override double Operate(double value, Interpreter interpreter, ref Diagnostic diagnostic) =>
            Math.Cos(value);
    }
}
