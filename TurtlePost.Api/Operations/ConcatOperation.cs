using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class ConcatOperation : BinaryOperation<string, string, string>
    {
        ConcatOperation()
        {
        }

        public static ConcatOperation Instance { get; } = new ConcatOperation();

        protected override string Operate(string top, string bottom, Interpreter interpreter,
            ref Diagnostic diagnostic) => bottom + top;
    }
}
