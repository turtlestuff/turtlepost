using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost
{
    public class CodeEnumerator
    {
        int position;
        string code;
        public CodeEnumerator(string gcode, int gpos = -1)
        {
            code = gcode;
            position = gpos;
        }

        public char Current { get => code[position]; }

        public bool MoveNext()
        {
            if(++position >= code.Length)
            {
                return false;
            }   

            return true;
        }

        public bool SetPosition(int pos)
        {
            if(pos < 0 | pos >= code.Length)
            {
                return false;
            }

            position = pos;
            return true;

        }

    }

}
