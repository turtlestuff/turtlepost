using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using TurtlePost.Operations;
using LabelBag = System.Collections.Generic.Dictionary<string, TurtlePost.Label>;

namespace TurtlePost
{
    public partial class Interpreter
    {
        public Interpreter()
        {
            // Do the regex compilation early to minimize perf hit on first evaluation
            LabelRegex.Match("");
        }

        public MovableStringEnumerator Enumerator { get; private set; } = default!;
        
        public string Code { get; private set; } = "";
        
        public Stack<int> CallStack { get; } = new Stack<int>();
        
        public Stack<object?> UserStack { get; } = new Stack<object?>();
        
        public Dictionary<string, Operation> Operations { get; } = new Dictionary<string, Operation>
        {
            // Math
            { "add",     AddOperation.Instance },
            { "sub",     SubOperation.Instance },
            { "mul",     MultiplyOperation.Instance },
            { "div",     DivideOperation.Instance },
            { "mod",     ModuloOperation.Instance },
            
            // Globals
            { "write",   WriteOperation.Instance },
            { "push",    PushOperation.Instance },

            // Strings
            { "concat",  ConcatOperation.Instance },

            // I/O
            { "print",   PrintOperation.Instance },
            { "println", PrintLineOperation.Instance },
            { "input",   InputOperation.Instance },

            // Stack manipulation
            { "dup",     DuplicateOperation.Instance },
            { "drop",    DropOperation.Instance  },
            { "swap",    SwapOperation.Instance },
            { "over",    OverOperation.Instance },


            // Boolean logic
            { "not",     NotOperation.Instance },
            { "and",     AndOperation.Instance },
            { "or",      OrOperation.Instance },
            { "xor",     XorOperation.Instance },
            
            // Comparisons
            { "eq",      EqualsOperation.Instance },
            { "gt",      GreaterThanOperation.Instance },
            { "lt",      LessThanOperation.Instance },
            { "gte",     GreaterThanOrEqualToOperation.Instance },
            { "lte",     LessThanOrEqualToOperation.Instance },
            
            // Conversions
            { "string",  StringOperation.Instance },
            { "parse",   ParseOperation.Instance },

            // Control flow
            { "jump",    JumpOperation.Instance },
            { "call",    CallOperation.Instance },
            { "jumpif",  JumpIfOperation.Instance },
            { "callif",  CallIfOperation.Instance },
            { "ret",     ReturnOperation.Instance },

            // Miscellaneous 
            { "exit",    ExitOperation.Instance },
            { "nop",     NoOperation.Instance },
            { "help",    HelpOperation.Instance },
            { "copying", CopyingOperation.Instance }
        };

        static readonly Regex LabelRegex = new Regex(@"@(\w)*:", RegexOptions.Compiled);
        
        LabelBag labels = new LabelBag();
        
        readonly GlobalBag globals = new GlobalBag();

        public void Interpret(string code, bool printOutput)
        {
#if DEBUG
            Stopwatch sw = default!;
            if (printOutput) 
                sw = Stopwatch.StartNew();
#endif
            SetupInterpreter(code);
#if DEBUG
            if (printOutput)
                Utils.PrintLabels(labels);
#endif
            try
            {
                while (true)
                {
                    // Break on EOF
                    if (!Enumerator.MoveNext()) break;
                    
                    switch (Enumerator.Current)
                    {
                        case '&':
                            ReadGlobal();
                            continue;
                        case '"':
                            ReadString();
                            continue;
                        case '@':
                            ReadLabel();
                            continue;
                        case '/':
                            SkipComment();
                            continue;
                        case '.':
                        case char c when char.IsDigit(c):
                            ReadNumber();
                            continue;
                        case char c when char.IsWhiteSpace(c):
                            continue;
                        default:
                            ReadOperation()?.Operate(this);
                            continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
                Console.ResetColor();
                UserStack.Clear();
            }
            
            if (printOutput)
            {
#if DEBUG
                sw.Stop();
                Utils.PrintGlobals(globals);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Execution time: {0}ms", sw.Elapsed.TotalMilliseconds);
                Console.ResetColor();
#endif
                Utils.PrintStack(UserStack);
                labels.Clear();
                CallStack.Clear();
            }
        }

        void SetupInterpreter(string code)
        {
            Enumerator = new MovableStringEnumerator(code);
            Code = code;
            labels.Clear();
            labels.Add("end", new Label("end", code.Length));
            
            // Search for labels in code
            var matches = LabelRegex.Matches(code);
            foreach (var i in (IEnumerable<Match>) matches)
            {
                var s = i.Value[1..^1];
                if (!labels.TryAdd(s, new Label(s, i.Index)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Duplicate label found: {0}:{1}", s, i.Index);
                    Console.ResetColor();
                }
            }
        }
    }
}