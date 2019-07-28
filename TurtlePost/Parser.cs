using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using TurtlePost.Operations;

namespace TurtlePost
{
    ref struct Parser
    {
        readonly string code;
        ReadOnlySpan<char>.Enumerator enumerator;
        string range;
        int location;
        internal static ImmutableDictionary<string, Operation> Operations { get; } = new Dictionary<string, Operation>
        {
            // maths
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
        
        public Parser(string code)
        {
            range = code;
            location = -1;
            this.code = code;
            enumerator = code.AsSpan().GetEnumerator();

        }

        bool NextChar()
        {
            var moved = enumerator.MoveNext();
            location++;

            return moved;
        }

        public Operation? NextOperation()
        {
            // Break on EOF
            if (!NextChar()) return null;

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

        void ReadUntil(char c)
        {
            var start = location;
            do
            {
                if (!NextChar()) break;
            } while (enumerator.Current != c);

            range = code[start..location];
        }

        private Operation ParseNumber()
        {
            ReadUntil(' ');
            return new PushObjectOperation(double.Parse(range, CultureInfo.InvariantCulture));
        }

        Operation ParseOperation()
        {
            ReadUntil(' ');

            return Operations[range];
        }

        PushObjectOperation ParseString()
        {
            NextChar();
            ReadUntil('"');
            
            return new PushObjectOperation(range);
        }

        NopOperation SkipComment()
        {
            NextChar();
            ReadUntil('/');
            return NopOperation.Instance;
        }

        PushObjectOperation ParseGlobal()
        {
            NextChar();
            ReadUntil(' ');
            return new PushObjectOperation(new Global(range));
        }

        Operation ParseLabel()
        {
            var start = location;
            do
            {
                if (!NextChar()) break;
            } while (enumerator.Current != ' ' || enumerator.Current != ':');
            if (enumerator.Current == ':')
            {
                return NopOperation.Instance;

            } else
            {
                return new PushObjectOperation(new Label(code[start..location].Substring(1)));
            }
            
        }

    }
}        
    