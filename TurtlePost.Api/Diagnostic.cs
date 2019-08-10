using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using TurtlePost.Operations;

namespace TurtlePost
{
    public ref struct Diagnostic
    {
        public Diagnostic(string message, string id, DiagnosticType type, ReadOnlySpan<char> span, int? sourceLocation = null)
        {
            Message = message;
            Id = id;
            DiagnosticType = type;
            Span = span;
            SourceLocation = sourceLocation;
        }
        
        public string Message;
        public string Id;
        public DiagnosticType DiagnosticType;
        public ReadOnlySpan<char> Span;
        public int? SourceLocation;
    }
    
    public enum DiagnosticType
    {
        None,
        Error,
    }
}