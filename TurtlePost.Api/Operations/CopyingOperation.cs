using System;
using static TurtlePost.I18N;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost.Operations
{
    class CopyingOperation : Operation
    {
        CopyingOperation()
        {
        }

        public static CopyingOperation Instance { get; } = new CopyingOperation();

        public override void Operate(Interpreter interpreter)
        {
            Console.WriteLine(TR["copyingNotice"]);
            Console.WriteLine();
            Console.WriteLine(TR["warrantyNotice"]);
            Console.WriteLine();
            Console.WriteLine(TR["licenseNotice"]);
        }
    }
}