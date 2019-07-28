using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using TurtlePost.Operations;
using LabelBag = System.Collections.Generic.Dictionary<string, TurtlePost.Label>;

namespace TurtlePost
{
    ref struct Parser
    {
        readonly ReadOnlySpan<char> code;
        private readonly GlobalBag globals;
        private readonly LabelBag labels;
        ReadOnlySpan<char>.Enumerator enumerator;
        ReadOnlySpan<char> buffer;
        int location;
    
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
        
        public Parser(ReadOnlySpan<char> code, GlobalBag globals, LabelBag labels)
        {
            this.code = code;
            this.globals = globals;
            this.labels = labels;
            buffer = code;
            location = -1;
            enumerator = this.code.GetEnumerator();
        }

        bool MoveNext()
        {
            location++;
            return enumerator.MoveNext();
        }

        public Operation? NextOperation()
        {
            // Break on EOF
            if (!MoveNext()) return null;

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
            var start = location;
            do
            {
                if (!MoveNext()) break;
            } while (enumerator.Current != c);

            buffer = code[start..location];
        }

        private Operation ParseNumber()
        {
            ReadToNextDelimiter();
            return new PushObjectOperation(double.Parse(buffer, provider: CultureInfo.InvariantCulture));
        }

        Operation ParseOperation()
        {
            ReadToNextDelimiter();

            // TODO: Figure out allocation neutral way to do this
            return Operations[buffer.ToString()];
        }

        PushObjectOperation ParseString()
        {
            MoveNext(); // Skip " character
            ReadToNextDelimiter('"');
            
            // We don't want to hold onto the old code string just to store a string variable
            // Thus we copy the new string object out of it so we can save memory
            return new PushObjectOperation(buffer.ToString()); 
        }

        NopOperation SkipComment()
        {
            MoveNext();  // Skip / character
            ReadToNextDelimiter('/');
            return NopOperation.Instance;
        }

        PushObjectOperation ParseGlobal()
        {
            MoveNext(); // Skip & character
            ReadToNextDelimiter();
            return new PushObjectOperation(globals[buffer.ToString()]);
        }

        Operation ParseLabel()
        {
            MoveNext(); // Skip @ character
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
    