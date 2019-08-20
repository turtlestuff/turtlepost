namespace TurtlePost.Operations
{
    public class TypeOfOperation : UnaryOperation<object?, string>
    {
        TypeOfOperation()
        {
        }
        
        public static TypeOfOperation Instance { get; } = new TypeOfOperation();

        protected override string Operate(object? value, Interpreter interpreter, ref Diagnostic diagnostic)
            => Utils.GetTypeName(value?.GetType());

    }
}