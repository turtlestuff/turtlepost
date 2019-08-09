using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TurtlePost.Operations
{
    class ParseOperation : Operation
    {
        ParseOperation()
        {
        }

        public static ParseOperation Instance { get; } = new ParseOperation();

        public override void Operate(Interpreter interpreter)
        {
            var value = (string) interpreter.UserStack.Pop()!;
            interpreter.UserStack.Push(double.Parse(value, CultureInfo.InvariantCulture));
        }
    }
}