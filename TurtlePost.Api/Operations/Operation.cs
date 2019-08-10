namespace TurtlePost.Operations
{
    public abstract class Operation
    {
        public abstract void Operate(Interpreter interpreter, ref Diagnostic diagnostic);
    }
}
