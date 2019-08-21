using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LabelBag = System.Collections.Generic.Dictionary<string, TurtlePost.Label>;
using static TurtlePost.I18N;

namespace TurtlePost
{
    static class Utils
    {
        
        // For List, we access private members of Stack<T> so that we can get the items in the correct order. We use expression trees to make that invocation
        // fast and only perform the reflection lookup once. Then, we get a delegate which can perform the invocation directly. 
        
        public static Func<TTarget, TField> GetFieldDelegate<TTarget, TField>(FieldInfo info)
        {
            var target = Expression.Parameter(typeof(TTarget), "target");
            var field = Expression.Field(target, info);

            return Expression.Lambda<Func<TTarget, TField>>(field, target).Compile();
        }
        
        public static Action<TTarget, TField> SetFieldDelegate<TTarget, TField>(FieldInfo info)
        {
            var target = Expression.Parameter(typeof(TTarget), "target");
            var value = Expression.Parameter(typeof(TField), "value");

            var field = Expression.Field(target, info);
            var assign = Expression.Assign(field, value);

            return Expression.Lambda<Action<TTarget, TField>>(assign, target, value).Compile();
        }
        
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
                case string str:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write('"');
                    Console.Write(str);
                    Console.Write('"');
                    break;
                case double num:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(num);
                    break;
                case Global global:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("&{0}", global.Name);                   
                    break;
                case Label label:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("@{0}", label.Name);
                    break;
                case List list:
                    Console.ResetColor();
                    Console.Write("{");
                    foreach (var obj in list)
                    {
                        Console.Write(" ");
                        WriteFormatted(obj);
                    }
                    Console.Write(" }");
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

        public static void PrintStack(List stack)
        {            
            for (var i = 0; i < stack.Count; i++)
            {
                var o = stack[i];

                WriteFormatted(o);

                if (i == stack.Count - 1)
                    Console.WriteLine();
                else
                    Console.Write(" | ");
            }
        }

        public static string GetTypeName(Type? type) => type == typeof(double) ? "number" : type?.Name.ToLower() ?? "null";
    }
}