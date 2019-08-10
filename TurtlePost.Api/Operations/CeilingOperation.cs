using System;

namespace TurtlePost.Operations
{
    class CeilingOperation : UnaryOperation<double, double>
    {
        CeilingOperation()
        {
        }

        public static CeilingOperation Instance { get; } = new CeilingOperation();
        
        protected override double Operate(double value, Interpreter interpreter, ref Diagnostic diagnostic) =>
            Math.Ceiling(value);
    }
}
