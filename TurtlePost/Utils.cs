using System;
using System.Collections.Generic;
using System.Text;

namespace TurtlePost
{
    public static class Utils
    {
        public static void PrintStack(Stack<Object> stack, bool colors)
        {
            Console.Write("| ");

            foreach (var i in stack)
            {
                if (i is string)
                {
                    if (colors) Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write('"');
                    Console.Write(i);
                    Console.Write('"');
                }
                else if(i == null)
                {
                    if (colors) Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write("null");
                }
                else
                {
                    if (colors) {
                        if (i is double) Console.ForegroundColor = ConsoleColor.Cyan;
                        if (i is Global) Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.Write(i);
                }
                Console.ResetColor();
                Console.Write(" | ");

            }

            Console.WriteLine();
        }
    }
}
