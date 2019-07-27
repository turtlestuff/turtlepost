namespace TurtlePost
{
    public struct Global
    {
        public string Name { get; }

        public Global(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return "Global (" + Name + ")";
        }
    }
}