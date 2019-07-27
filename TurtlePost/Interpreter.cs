using System;
using System.Collections.Generic;
using System.Text;
using TurtlePost.Operations;

namespace TurtlePost
{
    public class Interpreter
    {
        readonly Stack<object?> userStack = new Stack<object?>();
        readonly Stack<int> programStack = new Stack<int>();
        readonly GlobalBag globals = new GlobalBag();

        public void Interpret(string code)
        {
            var parser = new Parser(code);
            try
            {
                while (true)
                {
                    var op = parser.NextOperation();
                    if (op == null) break;

                    op.Operate(userStack, globals); // pass our globals to operator
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
                Console.ResetColor();
            }
#if DEBUG
            Utils.PrintStack(userStack, true);
#endif
        }
    }
}