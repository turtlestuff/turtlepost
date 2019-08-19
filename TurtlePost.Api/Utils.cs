using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using LabelBag = System.Collections.Generic.Dictionary<string, TurtlePost.Label>;
using static TurtlePost.I18N;

namespace TurtlePost
{
    static class Utils
    {
        public static void PrintLabels(LabelBag labels)
        {
            if (labels.Any())
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(TR["labels"]);
                Console.Write(" ");
                Console.WriteLine(string.Join(" | ", 
                    labels.Select(p => $"@{p.Key} -> {p.Value.Position.ToString()}")));
                Console.ResetColor();
            }
        }

        public static void PrintGlobals(GlobalBag globals)
        {
            if (globals.GlobalDictionary.Any())
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(TR["globals"]);
                Console.Write(" ");
                Console.WriteLine(string.Join(" | ",
                    globals.GlobalDictionary.Select(p => $"&{p.Key} = {p.Value.Value ?? "null"}")));
                Console.ResetColor();
            }
        }

        public static void WriteFormatted<T>(T o)
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
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(d);
                    break;
                case Global g:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("&{0}", g.Name);                   
                    break;
                case Label l:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("@{0}", l.Name);
                    break;
                case bool b:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write(b.ToString().ToLower());
                    break;
                case null:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("null");
                    break;
                default:                        
                    Console.Write(o.ToString());
                    break;
            }
            Console.ResetColor();
        }

        public static void PrintStack(Stack<object?> stack)
        {            
            for (var i = stack.Count-1; i >= 0; i--)
            {
                var o = stack.ElementAt(i);

                WriteFormatted(o);

                if (i != 0)
                    // Do not write separator if this is the last item
                    Console.Write(" | ");
                else
                    Console.WriteLine();
            }
        }

        public static string TypeName(Type? type)
        {
            return type == typeof(double) ? "number" : type?.Name.ToLower() ?? "null";
        }
    }
}