using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Reflection;
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
        
        public List UserStack { get; private set; } = new List();
        
        public Dictionary<string, Operation> Operations { get; } = new Dictionary<string, Operation>
        {
            // Arithmetic
            { "add",     AddOperation.Instance },
            { "sub",     SubOperation.Instance },
            { "mul",     MultiplyOperation.Instance },
            { "div",     DivideOperation.Instance },
            { "mod",     ModuloOperation.Instance },
            { "sqrt",    SquareRootOperation.Instance },
            { "pow",     PowerOperation.Instance }, 
            
            // Rounding
            { "ceil",    CeilingOperation.Instance },
            { "round",   RoundOperation.Instance },
            { "floor",   FloorOperation.Instance },
            
            // Trigonometric
            { "sin",     SineOperation.Instance },
            { "cos",     CosineOperation.Instance },
            { "tan",     TangentOperation.Instance },

            // Globals
            { "read",    ReadOperation.Instance },
            { "write",   WriteOperation.Instance },

            // Strings
            { "concat",  ConcatOperation.Instance },
            
            // Lists
            { "pop",     PopOperation.Instance },
            { "push",    PushOperation.Instance },
            { "get",     GetOperation.Instance },
            { "set",     SetOperation.Instance },
            { "del",     DeleteOperation.Instance },

            
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
            { "typeof",  TypeOfOperation.Instance }, 
            { "exit",    ExitOperation.Instance },
            { "nop",     NoOperation.Instance },
            { "help",    HelpOperation.Instance },
            { "copying", CopyingOperation.Instance },
        };

        static readonly Regex LabelRegex = new Regex(@"@(\w)*:", RegexOptions.Compiled);
        
        LabelBag labels = new LabelBag();
        
        readonly GlobalBag globals = new GlobalBag();

        bool CheckDiagnostic(in Diagnostic d, InterpretationOptions options)
        {
            switch (d.Type)
            {
                case DiagnosticType.Error:
                    if (!options.HideDiagnostics)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("{0} {1}: {2} ['", TR["error"], d.Id, d.Message);
                        Console.Out.Write(d.Span);
                        Console.WriteLine("', {0} {1}]", TR["pos"],
                            (d.SourceLocation ?? Enumerator.Position - d.Span.Length).ToString());
                        Console.ResetColor();
                    }
                    UserStack.Clear();
                    return true;
                default:
                    return false;
            }
        }

        public struct InterpretationOptions
        {
            public bool HideStack { get; set; }
            public bool DisallowOperations { get; set; }
            public bool HideDiagnostics { get; set; }
        }

        public void Interpret(string code, ref Diagnostic diagnostic, InterpretationOptions options = default)
        {
#if DEBUG
            Stopwatch sw = default!;
            if (!options.HideStack) 
                sw = Stopwatch.StartNew();
#endif
            SetupInterpreter(code, ref diagnostic);
            
            if (CheckDiagnostic(diagnostic, options))
                return;
#if DEBUG
            if (!options.HideStack)
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
                            LexGlobal();
                            continue;
                        case '@':
                            LexLabel(ref diagnostic);
                            continue;
                        case '{':
                            LexList(ref diagnostic);
                            continue;
                        case '"':
                            LexString(ref diagnostic);
                            continue;
                        case '/':
                            SkipComment();
                            continue;
                        case '.':
                        case char c when char.IsDigit(c):
                            LexNumber(ref diagnostic);
                            continue;
                        case char c when char.IsWhiteSpace(c):
                            continue;
                        default:
                            LexOperation(ref diagnostic, !options.DisallowOperations)?.Operate(this, ref diagnostic);
                            continue;
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

            CheckDiagnostic(diagnostic, options);

            if (!options.HideStack)
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
                    d = Diagnostic.Translate("TP0006", DiagnosticType.Error, i.Value, i.Index);
                    return;
                }
            }
        }
    }
}