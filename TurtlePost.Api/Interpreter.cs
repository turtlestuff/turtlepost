using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using TurtlePost.Operations;
using static TurtlePost.I18N;
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
            // Arithmetic
            { "add",     AddOperation.Instance },
            { "sub",     SubOperation.Instance },
            { "mul",     MultiplyOperation.Instance },
            { "div",     DivideOperation.Instance },
            { "mod",     ModuloOperation.Instance },
            
            // Rounding
            { "ceil",    CeilingOperation.Instance },
            { "round",   RoundOperation.Instance },
            { "floor",   FloorOperation.Instance },
            
            // Trigonometric
            { "sin",     SineOperation.Instance },
            { "cos",     CosineOperation.Instance },
            { "tan",     TangentOperation.Instance },

            // Globals
            { "write",   WriteOperation.Instance },
            { "push",    PushOperation.Instance },

            // Strings
            { "concat",  ConcatOperation.Instance },

            // Terminal
            { "print",   PrintOperation.Instance },
            { "println", PrintLineOperation.Instance },
            { "input",   InputOperation.Instance },
            { "cls",     ClearOperation.Instance },
            { "width",   WidthOperation.Instance },
            { "height",  HeightOperation.Instance },
            { "cursor",  CursorOperation.Instance },

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

        bool CheckError(in Diagnostic d)
        {
            switch (d.DiagnosticType)
            {
                case DiagnosticType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("{0} {1}: {2} ['", TR["error"], d.Id, d.Message);
                    Console.Out.Write(d.Span);
                    Console.WriteLine("', {0} {1}]", TR["pos"],
                        (d.SourceLocation ?? Enumerator.Position - d.Span.Length).ToString());
                    Console.ResetColor();
                    UserStack.Clear();
                    return true;
                default:
                    return false;
            }
        }

        public void Interpret(string code, bool printOutput)
        {
            Diagnostic d = default;
#if DEBUG
            Stopwatch sw = default!;
            if (printOutput) 
                sw = Stopwatch.StartNew();
#endif
            SetupInterpreter(code, ref d);
            if (CheckError(in d)) 
                return;
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
                            break;
                        case '"':
                            ReadString(ref d);
                            break;
                        case '@':
                            ReadLabel(ref d);
                            break;
                        case '/':
                            SkipComment();
                            break;
                        case '.':
                        case char c when char.IsDigit(c):
                            ReadNumber(ref d);
                            break;
                        case char c when char.IsWhiteSpace(c):
                            break;
                        default:
                            ReadOperation(ref d)?.Operate(this, ref d);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(TR["internalError"], ex);
                Console.ResetColor();
                UserStack.Clear();
            }

            CheckError(in d);

            if (printOutput)
            {
#if DEBUG
                sw.Stop();
                Utils.PrintGlobals(globals);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(TR["executionTime"], sw.Elapsed.TotalMilliseconds.ToString());
                Console.ResetColor();
#endif
                Utils.PrintStack(UserStack);
                labels.Clear();
                CallStack.Clear();
            }
        }

        void SetupInterpreter(string code, ref Diagnostic d)
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
                    d = new Diagnostic(TR["TP0006"], "TP0006", DiagnosticType.Error, i.Value, i.Index);
                    return;
                }
            }
        }
    }
}