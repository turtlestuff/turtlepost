using System.Diagnostics.CodeAnalysis;

namespace TurtlePost
{
    /// <summary>
    /// Represents a named value of any type.
    /// </summary>
    /// <remarks>
    /// Globals are referenced using the <code>&amp;var</code> syntax. The <see cref="GlobalBag"/> stores and creates globals as they are used.
    /// </remarks>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "No")]
    public class Global
    {
        /// <summary>
        /// The name of the global.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The global's value.
        /// </summary>
        public object? Value { get; set; }

        /// <summary>
        /// Initializes a new <see cref="Global"/>.
        /// </summary>
        /// <param name="name">The name of the new global.</param>
        /// <param name="value">The value of the new global.</param>
        public Global(string name, object? value = null)
        {
            Name = name;
            Value = value;
        }
    }
}