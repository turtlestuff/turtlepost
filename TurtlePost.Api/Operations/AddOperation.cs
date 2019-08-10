using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class AddOperation : BinaryOperation<double, double, double>
    {
        AddOperation()
        {
        }

        public static AddOperation Instance { get; } = new AddOperation();

        protected override double Operate(double top, double bottom, Interpreter interpreter,
            ref Diagnostic diagnostic) => bottom + top;
    }
}
