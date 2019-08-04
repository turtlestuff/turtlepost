using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class CallOperation : Operation
    {
        CallOperation() { }

        public static CallOperation Instance { get; } = new CallOperation();

        public override void Operate(Interpreter interpreter)
        {
            Label label = (Label)interpreter.UserStack.Pop()!;
            interpreter.ProgramStack.Push(interpreter.Enumerator.Position);
            interpreter.Enumerator.SetPosition(label.SourcePosition-1);
        }
    }
}
