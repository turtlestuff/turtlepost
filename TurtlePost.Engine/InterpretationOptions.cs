using System.Diagnostics.CodeAnalysis;

namespace TurtlePost
{
    /// <summary>
    /// A structure that represents options to pass to the interpreter.
    /// </summary>
    [SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Not an expected scenario")]
    public struct InterpretationOptions
    {
        /// <summary>
        /// Gets or sets whether the <see cref="Interpreter.UserStack"/> should be printed to the console.
        /// </summary>
        public bool HideStack { get; set; }
        /// <summary>
        /// Gets or sets whether operations should be allowed to be parsed.
        /// </summary>
        public bool DisallowOperations { get; set; }
        /// <summary>
        /// Gets or sets whether diagnostics should be printed to the console.
        /// </summary>
        public bool HideDiagnostics { get; set; }
    }
}