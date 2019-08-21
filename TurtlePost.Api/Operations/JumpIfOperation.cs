namespace TurtlePost.Operations
{
    class JumpIfOperation : Operation
    {
        JumpIfOperation()
        {
        }

        public static JumpIfOperation Instance { get; } = new JumpIfOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopA<Label>(ref diagnostic, out var label)) return;
            if (!interpreter.TryPopA<bool>(ref diagnostic, out var cond)) return;
            
            if (cond) 
                interpreter.Enumerator.Position = label.Position - 1;
        }
    }
}
