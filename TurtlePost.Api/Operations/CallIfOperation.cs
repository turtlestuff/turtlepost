namespace TurtlePost.Operations
{
    class CallIfOperation : Operation
    {
        CallIfOperation()
        {
        }

        public static CallIfOperation Instance { get; } = new CallIfOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopA<Label>(ref diagnostic, out var label)) return;
            if (!interpreter.TryPopA<bool>(ref diagnostic, out var cond)) return;

            if (cond)
            {
                interpreter.CallStack.Push(interpreter.Enumerator.Position);
                interpreter.Enumerator.Position = label.Position - 1;
            }
        }
    }
}
