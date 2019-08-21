using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace TurtlePost
{
    public class List : IEnumerable<object?>
    {
        Stack<object?> stack;
        
        static readonly Func<Stack<object?>, object?[]> GetStackArray =
            Utils.GetFieldDelegate<Stack<object?>, object?[]>(typeof(Stack<object?>)
                .GetField("_array", BindingFlags.Instance | BindingFlags.NonPublic));
        
        static readonly Func<Stack<object?>, int> GetStackSize =
            Utils.GetFieldDelegate<Stack<object?>, int>(typeof(Stack<object?>)
                .GetField("_size", BindingFlags.Instance | BindingFlags.NonPublic));
        
        static readonly Action<Stack<object?>, int> SetStackSize =
            Utils.SetFieldDelegate<Stack<object?>, int>(typeof(Stack<object?>)
                .GetField("_size", BindingFlags.Instance | BindingFlags.NonPublic));


        public List()
        {
            stack = new Stack<object?>();
        }

        public List(Stack<object?> stack)
        {
            this.stack = stack;
        }

        object?[] Array => GetStackArray(stack);


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
        
        public bool TrySetElement(int index, object? var)
        {
            if (index >= Count)
                return false;

            Array[index] = var;
            return true;
        }

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
        
        public int Count => stack.Count;
        public void Push(object? obj) => stack.Push(obj);
        public void Clear() => stack.Clear();
        public bool TryPop(out object? obj) => stack.TryPop(out obj);

        public struct Enumerator : IEnumerator<object?>
        {
            readonly List stack;
            int position;

            public Enumerator(List stack)
            {
                this.stack = stack;
                position = -1;
            }

            public bool MoveNext() => ++position < stack.Count;

            public void Reset() => position = -1;

            public object? Current => stack[position];

            object? IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<object> IEnumerable<object?>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
}