using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class CallIfOperation : Operation
    {
        CallIfOperation()
        {
        }

        public static CallIfOperation Instance { get; } = new CallIfOperation();

        public override void Operate(Interpreter interpreter)   
        {
            var label = (Label) interpreter.UserStack.Pop()!;
            var cond = (bool) interpreter.UserStack.Pop()!;
            if (cond)
            {
                interpreter.CallStack.Push(interpreter.Enumerator.Position);
                interpreter.Enumerator.TrySetPosition(label.Position - 1);
            }
        }
    }
}
