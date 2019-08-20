using static TurtlePost.I18N;

namespace TurtlePost.Operations
{
    public static class InterpreterExtensions
    {
        public static bool TryPopA<T>(this Interpreter interpreter, ref Diagnostic d, out T value) // TODO: MaybeNullAttribute
        {
            if (!interpreter.UserStack.TryPop(out var obj))
            {
                d = Diagnostic.Translate("TP0001", DiagnosticType.Error, d.Span);
                value = default!;
                return false;
            }

            if (typeof(T) == typeof(object) || obj is T)
            {
                value = (T) obj!;
                return true;
            }

            var expectedType = Utils.GetTypeName(typeof(T));
            d = new Diagnostic(TR["TP0003", expectedType, Utils.GetTypeName(obj?.GetType())],
                "TP0003", DiagnosticType.Error, d.Span);
            
            value = default!;
            return false;
        }
        
        public static bool TryPopStackFrame(this Interpreter interpreter, ref Diagnostic d, out int position)
        {
            if (interpreter.CallStack.TryPop(out position)) return true;
            
            d = Diagnostic.Translate("TP0002", DiagnosticType.Error, d.Span);
            return false;
        }
    }
}