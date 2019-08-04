﻿using System;
using System.Collections.Generic;
using System.Linq;
using LabelBag = System.Collections.Generic.Dictionary<string, TurtlePost.Label>;

namespace TurtlePost
{
    public static class Utils
    {
        public static void PrintLabels(LabelBag labels)
        {
            if (labels.Any())
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Labels: ");
                Console.WriteLine(string.Join(" | ", labels.Select(p => $"@{p.Key} -> {p.Value.SourcePosition}")));
                Console.ResetColor();
            }
        }

        public static void PrintGlobals(GlobalBag globals)
        {
            if (globals.GlobalDictionary.Any())
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Globals: ");
                Console.WriteLine(string.Join(" | ", globals.GlobalDictionary.Select(p => $"&{p.Key} = {p.Value.Value ?? "null"}")));
                Console.ResetColor();
            }
        }

        public static void WriteFormatted(object? o)
        {
            switch (o)
            {
                case string s:
                    Console.ForegroundColor = ConsoleColor.Green;
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
                    Console.Write("&{0}", g.Name);                   
                    break;
                case null:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write("null");
                    break;
                default:                        
                    Console.Write(o);
                    break;
            }
            Console.ResetColor();
        }

        public static void PrintStack(Stack<object?> stack)
        {            
            for (int i = stack.Count-1; i >= 0; i--)
            {
                object? o = stack.ElementAt(i);

                WriteFormatted(o);

                if (i != 0)
                {
                    // Do not write separator if this is the last item
                    Console.Write(" | ");
                }
            }

            Console.WriteLine();
        }
    }
}