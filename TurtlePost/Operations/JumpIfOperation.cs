using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class JumpIfOperation : Operation
    {
        JumpIfOperation() { }

        public static JumpIfOperation Instance { get; } = new JumpIfOperation();

        public override void Operate(Interpreter interpreter)
        {
            Label label = (Label)interpreter.UserStack.Pop()!;
            bool cond = (bool)interpreter.UserStack.Pop()!;
            if (cond)
            {
                interpreter.Enumerator.SetPosition(label.SourcePosition - 1);
            }       
        }
    }
}
