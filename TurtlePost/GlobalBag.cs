using System;
using System.Collections;
using System.Collections.Generic;

namespace TurtlePost
{
    public class GlobalBag
    {
        internal Dictionary<string, Global> GlobalDictionary { get; } = new Dictionary<string, Global>();

        public Global this[string name]
        {
            get
            {
                if (GlobalDictionary.TryGetValue(name, out var global))
                {
                    // Global already exists; return existing global
                    return global;
                }

                // Create new global
                global = new Global(name);
                GlobalDictionary.Add(name, global);
                return global;
            }
        }
    }
}