using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.IO;
using TurtlePost.Localization;

namespace TurtlePost
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(string.Format(Resources.Version, Assembly.GetExecutingAssembly().GetName().Version));
                Console.WriteLine(Resources.Copyright);
                Console.WriteLine(Resources.Help);
                var directInterpreter = new Interpreter();

                while (true)
                {
                    Console.Write("> ");
                    var toInterpret = Console.ReadLine();
                    
                    if (toInterpret == null)
                    {
                        toInterpret = "exit";
                        Console.Write(toInterpret);
                    }
                    
                    directInterpreter.Interpret(toInterpret,true);
                }
            } 
            else
            {
                // Load file
                var code = File.ReadAllText(args[0]);
                new Interpreter().Interpret(code,false);
                Console.WriteLine();
                Console.WriteLine("Press a key to exit...");
                Console.ReadKey();
            }
        }
    }
}