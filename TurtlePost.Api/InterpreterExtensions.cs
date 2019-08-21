using static TurtlePost.I18N;

namespace TurtlePost.Operations
{
    /// <summary>
    /// A set of extensions to make developing operations easier.
    /// </summary>
    public static class StackExtensions
    {
        /// <summary>
        /// Attempts to retrieve an item from the index. 
        /// </summary>
        /// <param name="stack">The stack to search.</param>
        /// <param name="index">The index to retrieve.</param>
        /// <param name="d">A diagnostic which will contain information about any errors.</param>
        /// <param name="value">The value returned from the specified index, if any.</param>
        /// <returns>A value indicating if the value was returned successfully.</returns>
        public static bool GetAt(this List stack, int index, ref Diagnostic d, out object? value)
        {
            if (!stack.TryGetElement(index, out value))
            {
                d = new Diagnostic(TR["TP0013", index], "TP0013", DiagnosticType.Error, d.Span);
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Attempts to retrieve set the item at an index. 
        /// </summary>
        /// <param name="stack">The stack to search.</param>
        /// <param name="index">The index to set.</param>
        /// <param name="d">A diagnostic which will contain information about any errors.</param>
        /// <param name="value">The value to set at the index.</param>
        /// <returns>A value indicating if the value was set successfully.</returns>
        public static bool SetAt(this List stack, int index, ref Diagnostic d, object? value)
        {
            if (!stack.TrySetElement(index, value))
            {
                d = new Diagnostic(TR["TP0013", index], "TP0013", DiagnosticType.Error, d.Span);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Attempts to remove an item at an index. 
        /// </summary>
        /// <param name="stack">The stack to search.</param>
        /// <param name="index">The index to remove.</param>
        /// <param name="d">A diagnostic which will contain information about any errors.</param>
        /// <returns>A value indicating if the value was removed successfully.</returns>
        public static bool RemoveAt(this List stack, int index, ref Diagnostic d)
        {
            if (!stack.TryRemoveElement(index))
            {
                d = new Diagnostic(TR["TP0013", index], "TP0013", DiagnosticType.Error, d.Span);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Attempts to pop an item of the specified type <typeparamref name="T"/>. If <typeparam name="T" /> is <see cref="object"/>, it will pop any object, including null. 
        /// </summary>
        /// <param name="interpreter">The interpreter whose user stack to pop.</param>
        /// <param name="d">A diagnostic which will contain information about any errors.</param>
        /// <param name="value">The popped value, if any.</param>
        /// <returns>A value indicating whether the object was popped successfully.</returns>
        public static bool TryPopA<T>(this Interpreter interpreter, ref Diagnostic d, out T value) => interpreter.UserStack.TryPopA(ref d, out value);
        
        /// <summary>
        /// Attempts to pop an item of the specified type <typeparamref name="T"/>. If <typeparam name="T" /> is <see cref="object"/>, it will pop any object, including null. 
        /// </summary>
        /// <param name="stack">The stack to search.</param>
        /// <param name="d">A diagnostic which will contain information about any errors.</param>
        /// <param name="value">The popped value, if any.</param>
        /// <returns>A value indicating whether the object was popped successfully.</returns>
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
        
        /// <summary>
        /// Attempts to pop the top location from the call stack.
        /// </summary>
        /// <param name="interpreter">The interpreter whose call stack to pop.</param>
        /// <param name="d">A diagnostic which will contain information about any errors.</param>
        /// <param name="position">The returned position, if any.</param>
        /// <returns>A value indicating whether the stack frame was popped successfully.</returns>
        public static bool TryPopStackFrame(this Interpreter interpreter, ref Diagnostic d, out int position)
        {
            if (interpreter.CallStack.TryPop(out position)) return true;
            
            d = Diagnostic.Translate("TP0002", DiagnosticType.Error, d.Span);
            return false;
        }
    }
}