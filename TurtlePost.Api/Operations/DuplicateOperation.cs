namespace TurtlePost.Operations
{
    class DuplicateOperation : Operation
    {
        DuplicateOperation()
        {
        }

        public static DuplicateOperation Instance { get; } = new DuplicateOperation();

        public override void Operate(Interpreter interpreter, ref Diagnostic diagnostic)
        {
            if (!interpreter.TryPopAny(ref diagnostic, out var value)) 
                return;
            
            interpreter.UserStack.Push(value);
            interpreter.UserStack.Push(value);
        }
    }
}
