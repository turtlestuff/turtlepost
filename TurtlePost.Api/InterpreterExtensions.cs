using System.Collections.Generic;
using System.Linq;
using static TurtlePost.I18N;

namespace TurtlePost
{
    public static class StackExtensions
    {
        public static bool GetAt(this List stack, int index, ref Diagnostic d, out object? value)
        {
            if (!stack.TryGetElement(index, out value))
            {
                d = new Diagnostic(TR["TP0013", index], "TP0013", DiagnosticType.Error, d.Span);
                return false;
            }

            return true;
        }
        
        public static bool SetAt(this List stack, int index, ref Diagnostic d, object? value)
        {
            if (!stack.TrySetElement(index, value))
            {
                d = new Diagnostic(TR["TP0013", index], "TP0013", DiagnosticType.Error, d.Span);
                return false;
            }

            return true;
        }

        public static bool RemoveAt(this List stack, int index, ref Diagnostic d)
        {
            if (!stack.TryRemoveElement(index))
            {
                d = new Diagnostic(TR["TP0013", index], "TP0013", DiagnosticType.Error, d.Span);
                return false;
            }

            return true;
        }

        
        public static bool TryPopA<T>(this Interpreter interpreter, ref Diagnostic d, out T value) => interpreter.UserStack.TryPopA(ref d, out value);
        
        public static bool TryPopA<T>(this List stack, ref Diagnostic d, out T value) // TODO: MaybeNullAttribute
        {
            if (!stack.TryPop(out var obj))
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