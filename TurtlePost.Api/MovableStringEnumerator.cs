using System.Collections;
using System.Collections.Generic;

namespace TurtlePost
{
    /// <summary>
    /// An enumerator that 
    /// </summary>
    public class MovableStringEnumerator : IEnumerator<char>
    {
        /// <summary>
        /// Initializes a new enumerator.
        /// </summary>
        /// <param name="str">The string to enumerate.</param>
        /// <param name="position">The initial position to set the enumerator to.</param>
        public MovableStringEnumerator(string str, int position = -1)
        {
            String = str;
            Position = position;
        }

        int position;

        /// <summary>
        /// Gets or sets the current position of the enumerator.
        /// </summary>
        public int Position
        {
            get => position;
            set
            {
                if (value < -1 || value > String.Length) return;
                position = value;
            }
        }
        
        /// <summary>
        /// Gets the string that is currently being enumerated.
        /// </summary>
        public string String { get; }

        /// <inheritdoc /> 
        public char Current => String[Position];
        
        object IEnumerator.Current => Current;
        
        /// <inheritdoc /> 
        public bool MoveNext() => ++Position < String.Length;
        
        /// <inheritdoc /> 
        public void Reset()
        {
        }
        
        /// <inheritdoc /> 
        public void Dispose()
        {
        }
    }
}
