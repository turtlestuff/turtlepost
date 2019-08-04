
using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class ReturnOperation : Operation
    {
        ReturnOperation() { }

        public static ReturnOperation Instance = new ReturnOperation();

        public override void Operate(Interpreter interpreter)
        {
            interpreter.Enumerator.SetPosition(interpreter.ProgramStack.Pop()!);
        }
    }
}
