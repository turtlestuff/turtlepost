using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost
{
    public class CodeEnumerator
    {
        public int Position;
        string code;
        public CodeEnumerator(string gcode, int gpos = -1)
        {
            code = gcode;
            Position = gpos;
        }

        public char Current { get => code[Position]; }

        public bool MoveNext()
        {
            if(++Position >= code.Length)
            {
                return false;
            }   

            return true;
        }

        public bool SetPosition(int pos)
        {
            if(pos < -1 | pos >= code.Length)
            {
                return false;
            }

            Position = pos;
            return true;

        }

    }

}
