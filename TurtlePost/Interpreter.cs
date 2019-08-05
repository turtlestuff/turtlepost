using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TurtlePost.Operations;
using LabelBag = System.Collections.Generic.Dictionary<string, TurtlePost.Label>;

namespace TurtlePost
{
    public class Interpreter
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
            { "mul",     MulOperation.Instance },
            { "div",     DivOperation.Instance },
            { "mod",     ModOperation.Instance },
            
            // Globals
            { "write",   WriteOperation.Instance },
            { "push",    PushOperation.Instance },

            // I/O
            { "println", PrintLineOperation.Instance },
            { "print",   PrintOperation.Instance },
            { "input",   InputOperation.Instance },

            // Stack
            { "dup",     DuplicateOperation.Instance },
            { "drop",    DropOperation.Instance  },

            // Flow
            { "jump",    JumpOperation.Instance },
            { "call",    CallOperation.Instance },
            { "jumpif",  JumpIfOperation.Instance },
            { "callif",  CallIfOperation.Instance },
            { "ret",     ReturnOperation.Instance },

            // Misc
            { "exit",    ExitOperation.Instance },
            { "nop",     NopOperation.Instance },
            { "help",    HelpOperation.Instance }
        };

        static readonly Regex LabelRegex = new Regex(@"@(\w)*:", RegexOptions.Compiled);
        
        LabelBag labels = new LabelBag();
        
        readonly GlobalBag globals = new GlobalBag();

        public void Interpret(string code)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            SetupInterpreter(code);
#if DEBUG
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
                            ParseGlobal();
                            continue;
                        case '"':
                            ParseString();
                            continue;
                        case '@':
                            ParseLabel();
                            continue;
                        case '/':
                            SkipComment();
                            continue;
                        case '.':
                        case char c when char.IsDigit(c):
                            ParseNumber();
                            continue;
                        case char c when char.IsWhiteSpace(c):
                            continue;
                        default:
                            ParseOperation()?.Operate(this);
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

#if DEBUG
            sw.Stop();
            Utils.PrintGlobals(globals);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Execution time: {0}ms", sw.ElapsedMilliseconds);
            Console.ResetColor();
#endif

            Utils.PrintStack(UserStack);
            labels.Clear();
            CallStack.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        ReadOnlySpan<char> ReadToNextDelimiter(char c = ' ')
        {
            var start = Enumerator.Position;
            do
            {
                if (!Enumerator.MoveNext()) break;
            } while (Enumerator.Current != c);

            return Code[start..Enumerator.Position];
        }

        void ParseNumber()
        {
            var buffer = ReadToNextDelimiter();
            UserStack.Push(double.Parse(buffer, provider: CultureInfo.InvariantCulture));
        }
        
        Operation? ParseOperation()
        {
            var buffer = ReadToNextDelimiter();
            switch (buffer)
            {
                case var _ when buffer.Equals("true", StringComparison.Ordinal):
                    UserStack.Push(true);
                    return null;
                case var _ when buffer.Equals("false", StringComparison.Ordinal):
                    UserStack.Push(false);
                    return null;
                case var _ when buffer.Equals("null", StringComparison.Ordinal):
                    UserStack.Push(null);
                    return null;
                default:
                    return Operations[buffer.ToString()];
            }
        }

        void ParseString()
        {
            Enumerator.MoveNext(); // Skip " character
            UserStack.Push(ReadToNextDelimiter('"').ToString());
        }

        void SkipComment()
        {
            Enumerator.MoveNext();  // Skip / character
            ReadToNextDelimiter('/');
        }

        void ParseGlobal()
        {
            Enumerator.MoveNext(); // Skip & character
            var buffer = ReadToNextDelimiter();
            UserStack.Push(globals[buffer.ToString()]);
        }

        void ParseLabel()
        {
            Enumerator.MoveNext(); // Skip @ character
            var buffer = ReadToNextDelimiter();
            if (buffer[^1] == ':')
            {
                // Label declaration; do not do anything
                return;
            }

            UserStack.Push(labels[buffer.ToString()]);
        }
    }
}