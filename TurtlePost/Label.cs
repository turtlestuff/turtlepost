using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost
{
    public class Label
    {
        public string Name { get; }
        public int SourcePosition { get; }

        public Label(string name, int position)
        {
            Name = name;
            SourcePosition = position;
        }
    }
    }
