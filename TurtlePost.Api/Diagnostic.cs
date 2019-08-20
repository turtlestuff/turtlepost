using System;

namespace TurtlePost
{
    public ref struct Diagnostic
    {
        public static Diagnostic Translate(string id, DiagnosticType type, ReadOnlySpan<char> span, int? sourceLocation = null) =>
            new Diagnostic
            {
                Message = I18N.TR[id],
                Id = id,
                Type = type,
                Span = span,
                SourceLocation = sourceLocation
            };

        public Diagnostic(string message, string id, DiagnosticType type, ReadOnlySpan<char> span, int? sourceLocation = null)
        {
            Message = message;
            Id = id;
            Type = type;
            Span = span;
            SourceLocation = sourceLocation;
        }
        
        public string Message;
        public string Id;
        public DiagnosticType Type;
        public ReadOnlySpan<char> Span;
        public int? SourceLocation;
    }
    
    public enum DiagnosticType
    {
        None,
        Error,
    }
}