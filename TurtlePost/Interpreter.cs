using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LabelBag = System.Collections.Generic.Dictionary<string, TurtlePost.Label>;

namespace TurtlePost
{
    public class Interpreter
    {
        internal static Regex labelFinder = new Regex(@"@(\w)*:", RegexOptions.Compiled);

        readonly Stack<object?> userStack = new Stack<object?>();
        readonly Stack<int> programStack = new Stack<int>();
        readonly GlobalBag globals = new GlobalBag();

        public void Interpret(string code)
        {
            var labels = new LabelBag();
            // Search for labels in code
            var matches = labelFinder.Matches(code);
            foreach (Match i in matches)
            {
                string s = i.Value[1..^1];
                if (!labels.TryAdd(s, new Label(s, i.Index)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Duplicate label found: {0}:{1}", s, i.Index);
                    Console.ResetColor();
                }
            }

#if DEBUG
            Utils.PrintLabels(labels);
#endif
            var parser = new Parser(code, globals, labels);
            try
            {
                while (true)
                {
                    var op = parser.NextOperation();
                    if (op == null) break;

                    op.Operate(userStack);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
                Console.ResetColor();
            }

#if DEBUG
            Utils.PrintGlobals(globals);
            Utils.PrintStack(userStack);
#endif
        }
    }
}