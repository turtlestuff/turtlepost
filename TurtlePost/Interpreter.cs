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
            { "mul",     MultiplyOperation.Instance },
            { "div",     DivideOperation.Instance },
            { "mod",     ModuloOperation.Instance },
            
            // Globals
            { "write",   WriteOperation.Instance },
            { "push",    PushOperation.Instance },

            // I/O
            { "print",   PrintOperation.Instance },
            { "println", PrintLineOperation.Instance },
            { "input",   InputOperation.Instance },

            // Stack manipulation
            { "dup",     DuplicateOperation.Instance },
            { "drop",    DropOperation.Instance  },
            { "swap",    SwapOperation.Instance },

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

        public void Interpret(string code, bool printStack)
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

#if DEBUG
            sw.Stop();
            Utils.PrintGlobals(globals);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Execution time: {0}ms", sw.Elapsed.TotalMilliseconds);
            Console.ResetColor();
#endif
            if (printStack)
            {
                Utils.PrintStack(UserStack);
                labels.Clear();
                CallStack.Clear();
            }
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

        ReadOnlySpan<char> ReadToNextDelimiter(char? c = null)
        {
            var start = Enumerator.Position;
            do
            {
                if (!Enumerator.MoveNext()) break;
            } while (c == null ? !char.IsWhiteSpace(Enumerator.Current) : c != Enumerator.Current);

            return Code[start..Enumerator.Position];
        }

        void ReadNumber()
        {
            var buffer = ReadToNextDelimiter();
            UserStack.Push(double.Parse(buffer, provider: CultureInfo.InvariantCulture));
        }
        
        Operation? ReadOperation()
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

        void ReadString()
        {
            // This function shifts a sub-slice of the buffer to the left a specified amount of times.
            // The sub-slice starts at the given start index and continues to the end (we don't need anything more).
            // The goal is to overwrite some leftover data after we collapse the escape sequence into 1 (or 2) chars.
            // It does this in a few steps:
            // - It takes a slice of the buffer from the given starting index to the end
            // - It subtracts our shift amount from the start index, giving us the index we intend to start writing to
            // - It uses CopyTo to actually write the sub-slice into the original buffer
            // - It slices off leftover data from the end; since we shifted over 'amount' times, there will be 'amount'
            // indices of "garbage data" left over on the end of the buffer that we need to remove.
            static void ShiftBufferSegment(ref Span<char> buffer, int startIndex, int amount)
            {
                buffer[startIndex..].CopyTo(buffer[(startIndex - amount)..]);
                buffer = buffer[..^amount];
            }
            
            Enumerator.MoveNext(); // Skip " character
            var sourceString = ReadToNextDelimiter('"');

            // Before we push the input string to the stack, we need to parse escape sequences. Since the buffer we 
            // get from the source is read-only (strings are immutable), we will need to make our own temporary mutable
            // buffer. It needs to be as long as the source string, but when we collapse the escape sequences,
            // the string becomes shorter, so we will have to trim off the leftover space as we go.

            // Allocate our buffer. If the string is too big for the stack, use the array pool.
            char[]? rentedArr = default;
            var buffer = sourceString.Length > 256 ? 
                rentedArr = ArrayPool<char>.Shared.Rent(sourceString.Length) : 
                stackalloc char[sourceString.Length];
            
            // Now we can copy from the source input to our mutable buffer
            sourceString.CopyTo(buffer);

            // Loop through our buffer to find any escape sequences
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != '\\')
                    continue;

                // An escape sequence has started; look ahead to see what is actually being escaped
                switch (buffer[i + 1])
                {
                    case '\'':
                        buffer[i] = '\'';
                        break;
                    case '"':
                        buffer[i] = '"';
                        break;
                    case '\\':
                        buffer[i] = '\\';
                        break;
                    case '0':
                        buffer[i] = '\0';
                        break;
                    case 'a':
                        buffer[i] = '\a';
                        break;
                    case 'b':
                        buffer[i] = '\b';
                        break;
                    case 'f':
                        buffer[i] = '\f';
                        break;
                    case 'n':
                        buffer[i] = '\n';
                        break;
                    case 'r':
                        buffer[i] = '\r';
                        break;
                    case 't':
                        buffer[i] = '\t';
                        break;
                    case 'v':
                        buffer[i] = '\v';
                        break;
                    case 'u':
                        // 'u' denotes an escape of the form \uXXXX, where XXXX is a UTF-16 code unit in hex. We need
                        // to capture those 4 characters that make up the hex digit, parse it, and reinterpret it as 
                        // a character. 
                        var utf16CodeUnit = ushort.Parse(buffer[(i + 2)..(i + 6)], NumberStyles.HexNumber,
                            CultureInfo.InvariantCulture);
                        buffer[i] = (char) utf16CodeUnit;
                        
                        // Shift the buffer left 5 to cover up the 'u' and the hex digit. Since we overwrote the
                        // backslash with the code unit, we shouldn't discard it.
                        ShiftBufferSegment(ref buffer, i + 6, 5);
                        continue;
                    case 'U':
                        // 'U' is similar in form to 'u', but parses a UTF-32 code unit instead (with an 8 digit long
                        // hex string to match). Because a UTF-32 code unit may or may not correspond to a UTF-16
                        // surrogate pair, we will have to shift either 8 or 9 characters over, depending on how many
                        // UTF-16 code units we end up writing.
                        var utf32CodeUnit = uint.Parse(buffer[(i + 2)..(i + 10)], NumberStyles.HexNumber,
                            CultureInfo.InvariantCulture);

                        // We can use Rune to convert a UTF-32 code unit to UTF-16 code units and write it to our buffer
                        // starting from the current index (which is the backslash). We need to store the amount of 
                        // characters written so that we trim off the correct number of garbage characters; if we write
                        // more than 1 character, we need to account for that (or else it gets discarded and the wrong
                        // character sequence is written).
                        var charsWritten = new Rune(utf32CodeUnit).EncodeToUtf16(buffer[i..]);
                        
                        // Since charsWritten is always 1 or 2, we can subtract it from 10 to see how many garbage
                        // characters we have to trim.
                        ShiftBufferSegment(ref buffer, i + 10, 10 - charsWritten);
                        continue;
                    default:
                        // TODO: Report an error
                        continue;
                }
                
                // We have already performed the character escape; however, the character we inserted only replaces
                // the first character of the escape sequence:
                // "H e l l \ n o" -> "H e l l \n n o"
                // Since we're only changing one character, and escape sequences are multiple characters long, we need
                // to clear away the rest of the characters in the escape sequence. We will do that by shifting the
                // remaining contents of the buffer *over* the leftover characters from the escape sequence.
                // Thus, the other characters in the escape sequence get overwritten and the buffer is trimmed to 
                // correctly fit the string. See ShiftBufferSegment for info on how it works.
                
                // The shifted sub-buffer should start 2 indices ahead of the current one to skip the (now replaced)
                // backslash and the character representing the escape. We are only going to shift one space to the left
                // since we only want to overwrite the character representing the escape, not the backslash (which got
                // replaced with the escaped character).
                ShiftBufferSegment(ref buffer, i + 2, 1);
            }

            // Make sure we return our array to the array pool if we used one
            if (rentedArr != null) 
                ArrayPool<char>.Shared.Return(rentedArr);

            // Finally push our escaped string to the stack.
            // And it was done allocation-free :)
            UserStack.Push(buffer.ToString());
        }

        void SkipComment()
        {
            Enumerator.MoveNext();  // Skip / character
            ReadToNextDelimiter('/');
        }

        void ReadGlobal()
        {
            Enumerator.MoveNext(); // Skip & character
            var buffer = ReadToNextDelimiter();
            UserStack.Push(globals[buffer.ToString()]);
        }

        void ReadLabel()
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