using System;
using System.Diagnostics.CodeAnalysis;

namespace TurtlePost
{
    /// <summary>
    /// Represents a diagnostic that was emitted by the interpreter.
    /// </summary>
    /// <remarks>
    /// Diagnostics are often passed by <see langword="ref"/> to other methods in order for them to 'bubble up' throughout the interpretation of a script.
    /// </remarks>
    [SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Not an expected scenario")]
    public ref struct Diagnostic
    {
        internal static Diagnostic Translate(string id, DiagnosticType type, ReadOnlySpan<char> span, int? sourceLocation = null) =>
            new Diagnostic
            {
                Message = I18N.TR[id],
                Id = id,
                Type = type,
                Span = span,
                SourceLocation = sourceLocation
            };

        /// <summary>
        /// Creates a diagnostic.
        /// </summary>
        /// <param name="message">The message to give to give this diagnostic that will be presented to the user.</param>
        /// <param name="id">The diagnostic ID to give this diagnostic. Used as the translation key to find the <see cref="Diagnostic.Message"/>.</param>
        /// <param name="type">The type of diagnostic.</param>
        /// <param name="span">The span of text which generated this diagnostic.</param>
        /// <param name="sourceLocation">The location in the original source from with the <paramref name="span"/> started. If null, </param>
        public Diagnostic(string message, string id, DiagnosticType type, ReadOnlySpan<char> span, int? sourceLocation = null)
        {
            Message = message;
            Id = id;
            Type = type;
            Span = span;
            SourceLocation = sourceLocation;
        }

        /// <summary>
        /// The user facing message of the emitted diagnostic.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The ID of the emitted diagnostic.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The type of the emitted diagnostic.
        /// </summary>
        public DiagnosticType Type { get; set; }

        /// <summary>
        /// The span of text which the emitted diagnostic was generated from.
        /// </summary>
        public ReadOnlySpan<char> Span { get; set; }

        /// <summary>
        /// The source location
        /// </summary>
        public int? SourceLocation { get; set; }
    }
    
    /// <summary>
    /// Represents the types of diagnostics that can be emitted.
    /// </summary>
    public enum DiagnosticType
    {
        /// <summary>
        /// No diagnostic. This is only used when a <see langword="default" /> instance of <see cref="Diagnostic"/> created.
        /// </summary>
        None,
        /// <summary>
        /// An error.
        /// </summary>
        Error,
    }
}