﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost
{
    public abstract class Operation
    {
        public abstract void Operate(Stack<Object> stack, Dictionary<Global,Object> globals);
    }
}
