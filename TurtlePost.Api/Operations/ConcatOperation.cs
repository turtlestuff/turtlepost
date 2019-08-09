using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class ConcatOperation : Operation
    {
        ConcatOperation() { }

        public static ConcatOperation Instance { get; } = new ConcatOperation();

        public override void Operate(Interpreter interpreter)
        {
            string str2 = (string)interpreter.UserStack.Pop()!;
            string str1 = (string)interpreter.UserStack.Pop()!;

            interpreter.UserStack.Push(str1 + str2);

        }
    }
}
