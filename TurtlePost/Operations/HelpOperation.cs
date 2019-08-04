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
            foreach(KeyValuePair<string,Operation> op in Interpreter.Operations)
            {
                Console.Write("{0} ", op.Key);
            }
        }
    }
}
