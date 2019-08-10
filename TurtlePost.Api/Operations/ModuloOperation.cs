using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class ModuloOperation : BinaryOperation<double, double, double>
    {
        ModuloOperation()
        {
        }

        public static ModuloOperation Instance = new ModuloOperation();

        protected override double Operate(double top, double bottom, Interpreter interpreter,
            ref Diagnostic diagnostic) => bottom % top;
    }
}