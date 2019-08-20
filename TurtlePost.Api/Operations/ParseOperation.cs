using System.Globalization;
using static TurtlePost.I18N;

namespace TurtlePost.Operations
{
    class ParseOperation : Operation
    {
        ParseOperation()
        {
        }

        public static ParseOperation Instance { get; } = new ParseOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopA<string>(ref diagnostic, out var str)) return;
            if (!double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
            {
                diagnostic = Diagnostic.Translate("TP0010", DiagnosticType.Error, diagnostic.Span);
                return;
            }
            
            interpreter.UserStack.Push(number);
        }
    }
}