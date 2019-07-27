using System;
using System.Collections.Generic;
using System.Text;
using TurtlePost.Operations;

namespace TurtlePost
{
    public class Interpreter
    {
        readonly Stack<Object> userStack = new Stack<Object>();
        readonly Stack<int> programStack = new Stack<int>();

        public void Interpret(string code)
        {
            Parser parser = new Parser(code);
            while (true)
            {
                Operation? op = parser.Do();
                if(op == null){
                    break;
                }
                else
                {
                    op.Operate(userStack); //since it is only a reference type, we are only
                                           //passing a pointer over :)
                }
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
