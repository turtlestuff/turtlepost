using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class CallOperation : Operation
    {
        CallOperation()
        {
        }

        public static CallOperation Instance { get; } = new CallOperation();

        public override void Operate(Interpreter interpreter)
        {
            var label = (Label) interpreter.UserStack.Pop()!;
            interpreter.CallStack.Push(interpreter.Enumerator.Position);
            interpreter.Enumerator.TrySetPosition(label.Position-1);
        }
    }
}
