using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class JumpIfOperation : Operation
    {
        JumpIfOperation()
        {
        }

        public static JumpIfOperation Instance { get; } = new JumpIfOperation();

        public override void Operate(Interpreter interpreter)
        {
            var label = (Label) interpreter.UserStack.Pop()!;
            var cond = (bool) interpreter.UserStack.Pop()!;
            if (cond)
            {
                interpreter.Enumerator.TrySetPosition(label.Position - 1);
            }       
        }
    }
}
