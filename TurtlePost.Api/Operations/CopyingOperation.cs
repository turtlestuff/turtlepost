using System;
using static TurtlePost.I18N;

namespace TurtlePost.Operations
{
    class CopyingOperation : Operation
    {
        CopyingOperation()
        {
        }

        public static CopyingOperation Instance { get; } = new CopyingOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            Console.WriteLine(TR["copyingNotice"]);
            Console.WriteLine();
            Console.WriteLine(TR["warrantyNotice"]);
            Console.WriteLine();
            Console.WriteLine(TR["licenseNotice"]);
        }
    }
}