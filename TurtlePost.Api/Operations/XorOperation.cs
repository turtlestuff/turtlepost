namespace TurtlePost.Operations
{
    class XorOperation : BinaryOperation<bool, bool, bool>
    {
        XorOperation()
        {
        }

        public static XorOperation Instance { get; } = new XorOperation();

        protected override bool Operate(bool top, bool bottom, Interpreter interpreter,
            ref Diagnostic diagnostic) => bottom ^ top;
    }
}