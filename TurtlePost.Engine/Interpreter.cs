using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using TurtlePost.Operations;
using static TurtlePost.I18N;
using LabelBag = System.Collections.Generic.Dictionary<string, TurtlePost.Label>;
using System.Globalization;

namespace TurtlePost
{
    /// <summary>
    /// The TurtlePost interpreter. 
    /// </summary>
    public partial class Interpreter
    {
        /// <summary>
        /// Creates a new interpreter.
        /// </summary>
        public Interpreter(Dictionary<string, Operation>? operations = null, List? list = null)
        {
            // Do the regex compilation early to minimize perf hit on first evaluation
            LabelRegex.Match("");

            UserStack = list ?? new List();

            Operations = operations ?? new Dictionary<string, Operation>
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
        }

        /// <summary>
        /// An enumerator.
        /// </summary>
        public ref MovableStringEnumerator Enumerator => ref _enumerator;

        MovableStringEnumerator _enumerator;
        
        /// <summary>
        /// The code being currently interpreted.
        /// </summary>
        public string Code { get; private set; } = "";
        
        /// <summary>
        /// Gets a collection that represents locations in the source where a call was performed.
        /// </summary>
        public Stack<int> CallStack { get; } = new Stack<int>();
        
        /// <summary>
        /// Gets a collection that represents the objects that are on the stack.
        /// </summary>
        public List UserStack { get; }
        
        /// <summary>
        /// Gets the globals that have been defined.
        /// </summary>
        public GlobalBag Globals { get; } = new GlobalBag();

        /// <summary>
        /// Gets the labels that have been defined.
        /// </summary>
        public LabelBag Labels { get; } = new LabelBag();

        /// <summary>
        /// Gets the operations that this interpreter can use.
        /// </summary>
        public Dictionary<string, Operation> Operations { get; }

        static readonly Regex LabelRegex = new Regex(@"@(\w)*:", RegexOptions.Compiled);

        bool CheckDiagnostic(in Diagnostic d, InterpretationOptions options)
        {
            switch (d.Type)
            {
                case DiagnosticType.Error:
                    if (!options.HideDiagnostics)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"{TR["error"]} {d.Id}: {d.Message} ['");
                        Console.Out.Write(d.Span);
                        Console.WriteLine($"', {TR["pos"]} {d.SourceLocation ?? Enumerator.Position - d.Span.Length}]");
                        Console.ResetColor();
                    }
                    UserStack.Clear();
                    return true;
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// Interprets the specified code.
        /// </summary>
        /// <param name="code">The code to interpret.</param>
        /// <param name="diagnostic">A diagnostic that has information about errors that occur while interpreting.</param>
        /// <param name="options">The options to give to the interpreter</param>
        public void Interpret(string code, ref Diagnostic diagnostic, InterpretationOptions options = default)
        {
            if (code is null)
                throw new ArgumentNullException(nameof(code));
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
                Utils.PrintLabels(Labels);
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
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
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
                Utils.PrintGlobals(Globals);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(TR["executionTime"], sw.Elapsed.TotalMilliseconds.ToString(CultureInfo.CurrentCulture));
                Console.ResetColor();
#endif
                Utils.PrintStack(UserStack);
                Labels.Clear();
                CallStack.Clear();
            }

        }
        
        void SetupInterpreter(string code, ref Diagnostic d)
        {
            Enumerator = new MovableStringEnumerator(code);
            Code = code;
            Labels.Clear();
            Labels.Add("end", new Label("end", code.Length));
            
            // Search for labels in code
            var matches = LabelRegex.Matches(code);
            foreach (var i in (IEnumerable<Match>) matches)
            {
                var s = i.Value[1..^1];
                if (!Labels.TryAdd(s, new Label(s, i.Index)))
                {
                    d = Diagnostic.Translate("TP0006", DiagnosticType.Error, i.Value, i.Index);
                    return;
                }
            }
        }
    }
}