using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class NotOperation : UnaryOperation<bool, bool>
    {
        NotOperation()
        {
        }

        public static NotOperation Instance { get; } = new NotOperation();

        protected override bool Operate(bool value, Interpreter interpreter, ref Diagnostic diagnostic) => !value;
    }
}