using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class JumpOperation : Operation
    {
        JumpOperation()
        {
        }

        public static JumpOperation Instance { get; } = new JumpOperation();

        public override void Operate(Interpreter interpreter)
        {
            var label = (Label) interpreter.UserStack.Pop()!;
            interpreter.Enumerator.TrySetPosition(label.Position - 1);
        }
    }
}
