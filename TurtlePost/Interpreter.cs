using System;
using System.Collections.Generic;
using System.Text;
using TurtlePost.Operations;

namespace TurtlePost
{
    public class Interpreter
    {
        readonly Stack<dynamic> userStack = new Stack<dynamic>();
        readonly Dictionary<string, Operation> operations = new Dictionary<string, Operation>();
        readonly Stack<int> programStack = new Stack<int>();

        public Interpreter()
        {
            operations.Add("add", new OperationAdd());
        }

        public void Interpret(string code)
        {
            string parsing;
            ParserModes currentMode = ParserModes.Default;
            foreach(char c in code)
            {
                switch (currentMode)
                {
                    case ParserModes.Default:
                        switch (c)
                        {
                            case '"':
                                currentMode = ParserModes.String;
                                continue;
                            case '/':
                                currentMode = ParserModes.Comment;
                                continue;
                            case '@':
                                currentMode = ParserModes.Label;
                                continue;
                            case ' ':
                            case '\n':
                            case '\r':
                                currentMode = ParserModes.Default;
                                continue;
                            default:
                                currentMode = ParserModes.Operation;
                                continue;
                        }
                    default:
                        Console.WriteLine(currentMode);
                        break;
                }
               
            }
#if DEBUG
            Console.Write("Stack: ");
            foreach (dynamic i in userStack)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(i);
                Console.ResetColor();
                Console.Write(" | ");
            }
            Console.WriteLine();
#endif  
        }
    }

    enum ParserModes
    {
        Operation,String,Label,Comment,Default
    }
}
