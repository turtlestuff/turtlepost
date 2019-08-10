using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class GreaterThanOrEqualToOperation : BinaryOperation<double, double, bool>
    {
        GreaterThanOrEqualToOperation()
        {
        }

        public static GreaterThanOrEqualToOperation Instance { get; } = new GreaterThanOrEqualToOperation();

        protected override bool Operate(double top, double bottom, Interpreter interpreter,
            ref Diagnostic diagnostic) => bottom >= top;
    }
}