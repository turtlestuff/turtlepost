using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace TurtlePost
{
    /// <summary>
    /// An enumerator that can be moved.
    /// </summary>
    public struct MovableStringEnumerator : IEnumerator<char>
    {
        /// <summary>
        /// Initializes a new enumerator.
        /// </summary>
        /// <param name="source">The string to enumerate.</param>
        /// <param name="position">The initial position to set the enumerator to.</param>
        public MovableStringEnumerator(string source, int position = -1)
        {
            Source = source;
            this.position = position;
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
                if (value < -1 || value > Source.Length) return;
                position = value;
            }
        }
        
        /// <summary>
        /// Gets the string that is currently being enumerated.
        /// </summary>
        public string Source { get; }

        /// <inheritdoc /> 
        public char Current => Source[Position];
        
        object IEnumerator.Current => Current;
        
        /// <inheritdoc /> 
        public bool MoveNext() => ++Position < Source.Length;
        
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
