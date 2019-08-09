using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Api.Operations
{
    class CursorOperation : Operation
    {
        CursorOperation() { }

        public static CursorOperation Instance { get; } = new CursorOperation();

        public override void Operate(Interpreter interpreter)
        {
            int y = (int)Math.Round((double)interpreter.UserStack.Pop()!); /////?? i dont fucgnfds know
            int x = (int)Math.Round((double)interpreter.UserStack.Pop()!);
            Console.SetCursorPosition(x, y);
        }

    }
}
