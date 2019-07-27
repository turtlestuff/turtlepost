
using System;

namespace TurtlePost
{
    class Parser
    {
        Int32 location = 0;

        public Parser(String code)
        {
            Code = code;
        }

        public String Code { get; }

        public Operation NextExpression()
        {

        }
    }
}