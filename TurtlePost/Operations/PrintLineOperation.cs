﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class PrintLineOperation : Operation
    {
        PrintLineOperation()
        {
        }

        public static PrintLineOperation Instance { get; } = new PrintLineOperation();


        public override void Operate(Stack<object?> stack)
        {
            Console.WriteLine(stack.Pop());
        }
    }
}