namespace TurtlePost.Operations
{
    class EqualsOperation : BinaryOperation<object, object, bool>
    {
        EqualsOperation()
        {
        }

        public static EqualsOperation Instance { get; } = new EqualsOperation();

        protected override bool Operate(object top, object bottom, Interpreter interpreter,
            ref Diagnostic diagnostic) => bottom.Equals(top);
    }
}