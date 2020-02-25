namespace TurtlePost
{
    /// <summary>
    /// Represents a named location in the source code.
    /// </summary>
    /// <remarks>
    /// Labels are created using the <code>@label</code> syntax.
    /// </remarks>
    public class Label
    {
        /// <summary>
        /// The name of the label.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The position of the label in the source code.
        /// </summary>
        public int Position { get; }
        
        /// <summary>
        /// Creates a new label.
        /// </summary>
        /// <param name="name">The name of the new label.</param>
        /// <param name="position">The position in the source code of the new label.</param>

        public Label(string name, int position)
        {
            Name = name;
            Position = position;
        }
    }
}
