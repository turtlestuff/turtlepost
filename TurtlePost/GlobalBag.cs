using System;
using System.Collections.Generic;

namespace TurtlePost
{
    public class GlobalBag
    {
        private Dictionary<Global, object?> globals = new Dictionary<Global, object?>();

        public object? this[Global global]
        {
            get
            {
                if (globals.TryGetValue(global, out var val))
                {
                    //the global already exists. 
                    return val;
                }

                // the global doesn't exist, so we create it
                globals.Add(global, null);
                return null;
            }
            set
            {
                if (globals.TryGetValue(global, out _))
                {
                    // the global already exists. now we just write to it
                    globals[global] = value;
                }
                else
                {
                    // the global doesn't exist, so we create it
                    globals.Add(global, value);
                }
            }
        }

        public void Delete(Global global)
        {
            globals.Remove(global);
        }
    }
}