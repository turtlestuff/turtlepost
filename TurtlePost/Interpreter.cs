using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TurtlePost
{
    public class Interpreter
    {
        internal static Regex labelFinder = new Regex(@"@(\w)*:", RegexOptions.Compiled);

        readonly Stack<object?> userStack = new Stack<object?>();
        readonly Stack<int> programStack = new Stack<int>();
        readonly GlobalBag globals = new GlobalBag();
        Dictionary<Label, int>? labels; //label,pos
        public void Interpret(string code)
        {
            labels = new Dictionary<Label, int>();
            //setup label defs
            var matches = labelFinder.Matches(code);
            foreach (Match i in matches)
            {
                string s = i.Value;
                s = s.Substring(1, s.Length - 2);
                if (!labels.TryAdd(new Label(s), i.Index))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Duplicate label found: {0}:{1}", s, i.Index);
                    Console.ResetColor();
                }
            }

#if DEBUG
            Console.Write("Labels: ");
            foreach (KeyValuePair<Label, int> p in labels)
            {
                Console.Write(p.Key.Name + ":" + p.Value + ", ");
            }
            Console.WriteLine();
#endif

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