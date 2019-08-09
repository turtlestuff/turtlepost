using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class HelpOperation : Operation
    {
        HelpOperation() { }

        public static HelpOperation Instance { get; } = new HelpOperation();

        public override void Operate(Interpreter interpreter)
        {
            Console.WriteLine(string.Join(" ", interpreter.Operations.Keys));
        }
    }
}
