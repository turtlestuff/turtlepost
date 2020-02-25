namespace TurtlePost.Operations
{
    class JumpOperation : Operation
    {
        JumpOperation()
        {
        }

        public static JumpOperation Instance { get; } = new JumpOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopA<Label>(ref diagnostic, out var label)) return;
            interpreter.Enumerator.Position = label.Position - 1;
        }
    }
}
