using System;
using System.Collections.Generic;
using System.Linq;

namespace TurtlePost
{
    public static class Utils
    {
        public static void PrintStack(Stack<object?> stack, bool colors)
        {
            //#pragma warning disable CS8619 no longer needed because we have a Normal for loop now
            
            for(int i = 0; i < stack.Count; i++)
            {
                object? o = stack.ElementAt (i);
                switch (o)
                {
                    case string s:
                        if (colors) Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('"');
                        Console.Write(s);
                        Console.Write('"');
                        break;
                    case double d:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(d);
                        break;
                    case Global g:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(g);
                        break;
                    case null:
                        if (colors) Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write("null");
                        break;
                    default:
                        Console.Write(o);
                        break;
                }

                Console.ResetColor();
                if (i != stack.Count - 1)
                {
                    // Do not write separator if this is the last item
                    Console.Write(" | ");
                }
            }

            Console.WriteLine();
        }
    }
}