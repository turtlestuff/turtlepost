using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class SubOperation : BinaryOperation<double, double, double>
    {
        SubOperation()
        {
        }

        public static SubOperation Instance { get; } = new SubOperation();

        protected override double Operate(double top, double bottom, Interpreter interpreter,
            ref Diagnostic diagnostic) => bottom - top;
    }
}