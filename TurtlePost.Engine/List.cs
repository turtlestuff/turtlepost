using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace TurtlePost
{
    /// <summary>
    /// A list-stack hybrid. This collection can be indexed and pushed to/popped from.
    /// </summary>
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "Not a collection")]
    public class List : IEnumerable<object?>
    {
        Stack<object?> stack;
        
        // These methods are used to get the internal state of the stack that is used as the backing collection.
        
        static readonly Func<Stack<object?>, object?[]> GetStackArray =
            Utils.GetFieldDelegate<Stack<object?>, object?[]>(typeof(Stack<object?>)
                .GetField("_array", BindingFlags.Instance | BindingFlags.NonPublic)!);
        
        static readonly Func<Stack<object?>, int> GetStackSize =
            Utils.GetFieldDelegate<Stack<object?>, int>(typeof(Stack<object?>)
                .GetField("_size", BindingFlags.Instance | BindingFlags.NonPublic)!);
        
        static readonly Action<Stack<object?>, int> SetStackSize =
            Utils.SetFieldDelegate<Stack<object?>, int>(typeof(Stack<object?>)
                .GetField("_size", BindingFlags.Instance | BindingFlags.NonPublic)!);

        
        /// <summary>
        /// Initializes a new list.
        /// </summary>
        public List()
        {
            stack = new Stack<object?>();
        }

        /// <summary>
        /// Gets the backing array of the stack.
        /// </summary>
        object?[] Array => GetStackArray(stack);

        /// <summary>
        /// Gets or sets the value at a specified index.
        /// </summary>
        /// <param name="index">The index to get or set.</param>
        /// <exception cref="ArgumentOutOfRangeException">The index was out of range of the collection.</exception>
        public object? this[int index]
        {
            get
            {
                if (index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                
                return Array[index];
            }
            set
            {
                if (index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                
                Array[index] = value;
            }
        }

        /// <summary>
        /// Attempts to get the value at the specified index.
        /// </summary>
        /// <param name="index">The index to get the value of.</param>
        /// <param name="var">The resulting value from the index.</param>
        /// <returns>A value indicating whether the object was retrieved successfully.</returns>
        public bool TryGetElement(int index, out object? var)
        {
            if (index >= Count)
            {
                var = default;
                return false;
            }

            var = Array[index];
            return true;
        }
        
        /// <summary>
        /// Attempts to set the specified index of the collection to the specified value.
        /// </summary>
        /// <param name="index">The index to set to the value.</param>
        /// <param name="var">The value to set at the index.</param>
        /// <returns>A value indicating whether the index was set successfully.</returns>
        public bool TrySetElement(int index, object? var)
        {
            if (index >= Count)
                return false;

            Array[index] = var;
            return true;
        }

        /// <summary>
        /// Attempts to remove the object at the specified index from the collection.
        /// </summary>
        /// <param name="index">The index to remove the value from.</param>
        /// <returns>A value indicating whether the object was removed successfully.</returns>
        public bool TryRemoveElement(int index)
        {
            if (index >= Count)
                return false;

            SetStackSize(stack, GetStackSize(stack) - 1);
            var size = GetStackSize(stack);
            if (index < size)
            {
                System.Array.Copy(Array, index + 1, Array, index, size - index);
            }
            
            Array[size] = default!;
            return true;
        }
        
        /// <summary>
        /// Gets the number of stored elements.
        /// </summary>
        public int Count => stack.Count;
        /// <summary>
        /// Inserts an object at the top of the <see cref="List"/>.
        /// </summary>
        /// <param name="value">The object to insert.</param>
        public void Push(object? value) => stack.Push(value);
        /// <summary>
        /// Clears the collection.
        /// </summary>
        public void Clear() => stack.Clear();
        /// <summary>
        /// Attempts to remove and return the object at the top of the <see cref="List"/>.
        /// </summary>
        /// <param name="value">The resulting object.</param>
        /// <returns>A value indicating whether the object was popped successfully.</returns>
        public bool TryPop(out object? value) => stack.TryPop(out value);

        /// <summary>
        /// Enumerates the elements of a <see cref="List"/> object.
        /// </summary>
        public struct Enumerator : IEnumerator<object?>
        {
            readonly List stack;
            int position;

            internal Enumerator(List stack)
            {
                this.stack = stack;
                position = -1;
            }

            /// <inheritdoc /> 
            public bool MoveNext() => ++position < stack.Count;

            /// <inheritdoc /> 
            public void Reset() => position = -1;

            /// <inheritdoc /> 
            public object? Current => stack[position];

            object? IEnumerator.Current => Current;

            /// <inheritdoc /> 
            public void Dispose()
            {
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<object> IEnumerable<object?>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc /> 
        public override String ToString() =>
            $"{{ {string.Join(" ", Array.Select(a => a is string ? $@"""{a}""" : a?.ToString() ?? "null").ToArray(), 0, Count)} }}";
    }
    
}