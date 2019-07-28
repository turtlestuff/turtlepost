using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost
{
    public struct Label
    {
        public string Name { get; }

        public Label(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return "Label(" + Name + ")";
        }

    }
}
