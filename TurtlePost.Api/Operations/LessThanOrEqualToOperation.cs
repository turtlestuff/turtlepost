using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class LessThanOrEqualToOperation : BinaryOperation<double, double, bool>
    {
        LessThanOrEqualToOperation()
        {
        }

        public static LessThanOrEqualToOperation Instance { get; } = new LessThanOrEqualToOperation();

        protected override bool Operate(double top, double bottom, Interpreter interpreter,
            ref Diagnostic diagnostic) => bottom <= top;
    }
}