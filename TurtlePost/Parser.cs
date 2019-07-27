
using System;
using System.Collections.Generic;
using System.Text;
using TurtlePost.Operations;

namespace TurtlePost
{
    class Parser
    {
        readonly CharEnumerator enumerator;
        internal static Dictionary<string, Operation> Operations { get; } = new Dictionary<string, Operation>
        {
            { "add", OperationAdd.Instance }
        };
        readonly StringBuilder buffer = new StringBuilder();

        public Parser(string code)
        {
            enumerator = code.GetEnumerator();
        }

       

        public IEnumerable<Operation?> Do()
        {
            // Break on EOF
            if (!enumerator.MoveNext()) yield break;

            switch (enumerator.Current)
            {
                case ' ':
                case '\n':
                case '\r':
                    yield return OperationNone.Instance;
                    break;
                case '"':
                    yield return ParseString();
                    break;
                case '/': 
                    yield return SkipComment();
                    break;
                default:
                    yield return ParseOperation();
                    break;
            }
            
            buffer.Clear();
        }

        Operation ParseOperation()
        {
            do
            {
                buffer.Append(enumerator.Current);
                enumerator.MoveNext();
            } while (enumerator.Current != ' ');

            string s = buffer.ToString();

            if (!double.TryParse(s, out var num))
            {
                return Operations.TryGetValue(s, out var op) ?
                    op : throw new InvalidOperationException("Operation does not fucking exist.");
            }
            else
            {
                return new OperationPush(num);
            }
        }

        OperationPush ParseString()
        {
            enumerator.MoveNext();
            do
            {
                buffer.Append(enumerator.Current);
                enumerator.MoveNext();
            } while (enumerator.Current != '"');

            return new OperationPush(buffer.ToString());
        }

        OperationNone SkipComment()
        {
            enumerator.MoveNext();
            do 
            { 
                enumerator.MoveNext(); 
            } while (enumerator.Current != '/');

            return OperationNone.Instance;
        }
    }
}        
    
