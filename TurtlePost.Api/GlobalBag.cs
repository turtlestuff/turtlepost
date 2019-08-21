using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TurtlePost
{
    /// <summary>
    /// Represents a bag of <see cref="Global"/> values. 
    /// </summary>
    /// <seealso cref="Global"/>
    public class GlobalBag
    {
        /// <summary>
        /// Initializes a new <see cref="GlobalBag"/>.
        /// </summary>
        public GlobalBag()
        {
            GlobalDictionary = new ReadOnlyDictionary<String, Global>(globals);
        }
        
        /// <summary>
        /// A read-only dictionary that contains the globals that have been created. Use <see cref="this[string]"/> to retrieve or add items.
        /// </summary>
        public ReadOnlyDictionary<string, Global> GlobalDictionary { get; } 
        
        Dictionary<string, Global> globals = new Dictionary<string, Global>();

        /// <summary>
        /// Gets a global with the provided name. If such a global, it will be created with a default value.
        /// </summary>
        /// <param name="name">The name of the global to get.</param>
        public Global this[string name]
        {
            get
            {
                if (globals.TryGetValue(name, out var global))
                {
                    // Global already exists; return existing global
                    return global;
                }

                // Create new global
                var nameStr = name;
                global = new Global(nameStr);
                globals.Add(nameStr, global);
                return global;
            }
        }
    }
}