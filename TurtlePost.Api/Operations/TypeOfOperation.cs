namespace TurtlePost.Operations
{
    public class TypeOfOperation : UnaryOperation<object?, string>
    {
        private TypeOfOperation(){}
        
        public static TypeOfOperation Instance { get; } = new TypeOfOperation();

        protected override string Operate(object? value, Interpreter interpreter, ref Diagnostic diagnostic)
            => Utils.TypeName(value?.GetType());

    }
}