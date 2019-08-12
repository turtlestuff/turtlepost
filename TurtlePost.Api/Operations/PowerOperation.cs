using System;

namespace TurtlePost.Operations
{
    public class PowerOperation : BinaryOperation<double,double,double>
    {
        PowerOperation() { }
        
        public static PowerOperation Instance { get; } = new PowerOperation();

        protected override double Operate(double top, double bottom, Interpreter interpreter, ref Diagnostic diagnostic)
            => Math.Pow(bottom, top);
    }
}