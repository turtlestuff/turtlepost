
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TurtlePost.Operations;

namespace TurtlePost
{
    class Parser
    {
        readonly CharEnumerator enumerator;
        internal static Dictionary<string, Operation> Operations { get; } = new Dictionary<string, Operation>
        {
            { "add", OperationAdd.Instance },
            { "sub", OperationSub.Instance }
        };
        readonly StringBuilder buffer = new StringBuilder();

        public Parser(string code)
        {
            enumerator = code.GetEnumerator();
        }

       

        public Operation? Do()
        {
            buffer.Clear();
            // Break on EOF
            if (!enumerator.MoveNext()) return null;

            switch (enumerator.Current)
            {
                case ' ':
                case '\n':
                case '\r':
                    return OperationNone.Instance;
                case '"':
                    return ParseString();
                case '/': 
                    return SkipComment();
                default:
                    return ParseOperation();
            }
            
            
        }

        Operation? ParseOperation()
        {
            do
            {
                buffer.Append(enumerator.Current);
                if (!enumerator.MoveNext()) break;
            } while (enumerator.Current != ' ');

            string s = buffer.ToString();

            if (!double.TryParse(s,NumberStyles.Float,CultureInfo.InvariantCulture,out var num))
            {
                return Operations.TryGetValue(s, out var op) ?
                    op : throw new InvalidOperationException("Operation does not exist.");
            }
            else
            {
                return new OperationPush(num);
            }
        }

        OperationPush? ParseString()
        {
            enumerator.MoveNext();
            do
            {
                buffer.Append(enumerator.Current);
                if (!enumerator.MoveNext()) break;
            } while (enumerator.Current != '"');

            return new OperationPush(buffer.ToString());
        }

        OperationNone SkipComment()
        {
            enumerator.MoveNext();
            do 
            {
                if (!enumerator.MoveNext()) break;
            } while (enumerator.Current != '/');

            return OperationNone.Instance;
        }
    }
}        
    
