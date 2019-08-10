namespace TurtlePost
{
    public class MovableStringEnumerator
    {
        public MovableStringEnumerator(string code, int position = -1)
        {
            String = code;
            Position = position;
        }

        int position;

        public int Position
        {
            get => position;
            set
            {
                if (value < -1 || value > String.Length) return;
                position = value;
            }
        }
        public string String { get; }
        public char Current => String[Position];
        public bool MoveNext() => ++Position < String.Length;
    }
}
