using System.Collections.Generic;

namespace TurtlePost
{
    internal class GlobalBag
    {
        public Dictionary<string, Global> GlobalDictionary { get; } = new Dictionary<string, Global>();

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
                var nameStr = name;
                global = new Global(nameStr);
                GlobalDictionary.Add(nameStr, global);
                return global;
            }
        }
    }
}