namespace TurtlePost
{
    public class MovableStringEnumerator
    {
        public MovableStringEnumerator(string code, int position = -1)
        {
            String = code;
            Position = position;
        }

        public int Position { get; private set; }
        public string String { get; }
        public char Current => String[Position];

        public bool MoveNext() => ++Position < String.Length;

        public bool TrySetPosition(int pos)
        {
            if (pos < -1 || pos >= String.Length)
                return false;

            Position = pos;
            return true;
        }
    }
}
