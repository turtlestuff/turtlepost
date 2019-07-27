using System;
using System.Collections.Generic;
using System.Text;
using TurtlePost.Operations;

namespace TurtlePost
{
    public class Interpreter
    {
        readonly Stack<dynamic> userStack = new Stack<dynamic>();
        readonly Stack<int> programStack = new Stack<int>();

        public void Interpret(string code)
        {
            foreach (var op in new Parser(code).Do())
            {
                op.Operate(userStack);
            }
#if DEBUG
            foreach(var i in userStack)
            {
                Console.Write(i + " | ");
            }
#endif

        }
    }
}
