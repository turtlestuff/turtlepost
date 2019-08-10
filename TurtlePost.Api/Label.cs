namespace TurtlePost
{
    public class Label
    {
        public string Name { get; }
        public int Position { get; }

        public Label(string name, int position)
        {
            Name = name;
            Position = position;
        }
    }
}
