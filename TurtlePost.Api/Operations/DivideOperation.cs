using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class DivideOperation : BinaryOperation<double, double, double>
    {
        DivideOperation()
        {
        }

        public static DivideOperation Instance = new DivideOperation();

        protected override double Operate(double top, double bottom, Interpreter interpreter,
            ref Diagnostic diagnostic) => bottom / top;
    }
}