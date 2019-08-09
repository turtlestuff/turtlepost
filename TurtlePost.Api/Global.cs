namespace TurtlePost
{
    // TODO: use readonly struct
    public class Global
    {
        public string Name { get; }
        public object? Value { get; set; }

        public Global(string name, object? value = null)
        {
            Name = name;
            Value = value;
        }
    }
}