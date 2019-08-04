using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LabelBag = System.Collections.Generic.Dictionary<string, TurtlePost.Label>;
using TurtlePost.Operations;
using System.Globalization;

namespace TurtlePost
{
    public class Interpreter
    {
        internal static Regex labelFinder = new Regex(@"@(\w)*:", RegexOptions.Compiled);
        string code = "";
        LabelBag labels = new LabelBag();
        readonly Stack<object?> userStack = new Stack<object?>();
        readonly Stack<int> programStack = new Stack<int>();
        readonly GlobalBag globals = new GlobalBag();
        CodeEnumerator enumerator = new CodeEnumerator("");
        StringBuilder buffer = new StringBuilder();

        public void Interpret(string code)
            {
            enumerator = new CodeEnumerator(code);
            this.code = code;
            labels.Clear();
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
            try
            {
                while (true)
                {
                    var op = NextOperation();
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
#endif

            Utils.PrintStack(userStack);
                
        }
      

        internal static ImmutableDictionary<string, Operation> Operations { get; } = new Dictionary<string, Operation>
        {
            // math
            { "add",     AddOperation.Instance },
            { "sub",     SubOperation.Instance },
            { "mul",     MulOperation.Instance },
            { "div",     DivOperation.Instance },
            { "mod",     ModOperation.Instance },
            
            // globals
            { "write",   WriteOperation.Instance },
            { "push",    PushOperation.Instance },

            // i/o
            { "println", PrintLineOperation.Instance },
            { "print",   PrintOperation.Instance },
            { "input",   InputOperation.Instance },
            
            // misc
            { "exit",    ExitOperation.Instance },
            { "nop",     NopOperation.Instance }
        }.ToImmutableDictionary();

        public Operation? NextOperation()
        {
            // Break on EOF
            if (!enumerator.MoveNext()) return null;

            switch (enumerator.Current)
            {
                case ' ':
                case '\n':
                case '\r':
                    return NopOperation.Instance;
                case '&':
                    return ParseGlobal();
                case '"':
                    return ParseString();
                case '/':
                    return SkipComment();
                case '.':
                case char c when char.IsDigit(c):
                    return ParseNumber();
                case '@':
                    return ParseLabel();
                default:
                    return ParseOperation();
            }
        }

        void ReadToNextDelimiter(char c = ' ')
        {
            buffer.Clear();
            do
            {
                buffer.Append(enumerator.Current);
                if (!enumerator.MoveNext()) break;
            } while (enumerator.Current != c);

        }

        private Operation ParseNumber()
        {
            ReadToNextDelimiter();
            return new PushObjectOperation(double.Parse(buffer.ToString(), provider: CultureInfo.InvariantCulture)); ;
        }

        Operation ParseOperation()
        {
            ReadToNextDelimiter();

            // TODO: Figure out allocation neutral way to do this
            return Operations[buffer.ToString()];
        }

        PushObjectOperation ParseString()
        {
            enumerator.MoveNext(); // Skip " character
            ReadToNextDelimiter('"');

            // We don't want to hold onto the old code string just to store a string variable
            // Thus we copy the new string object out of it so we can save memory
            return new PushObjectOperation(buffer.ToString());
        }

        NopOperation SkipComment()
        {
            enumerator.MoveNext();  // Skip / character
            ReadToNextDelimiter('/');
            return NopOperation.Instance;
        }

        PushObjectOperation ParseGlobal()
        {
            enumerator.MoveNext(); // Skip & character
            ReadToNextDelimiter();
            return new PushObjectOperation(globals[buffer.ToString()]);
        }

        Operation ParseLabel()
        {
            enumerator.MoveNext(); // Skip @ character
            ReadToNextDelimiter();
            if (buffer[^1] == ':')
            {
                // Label declaration; do not do anything
                return NopOperation.Instance;
            }

            return new PushObjectOperation(labels[buffer.ToString()]);
        }


    }
}