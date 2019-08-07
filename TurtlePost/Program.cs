using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.IO;

namespace TurtlePost
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine($"TurtlePost Blueprint {Assembly.GetExecutingAssembly().GetName().Version}");
                Console.WriteLine("By Vrabbers and Reflectronic, (c) 2019");
                Console.WriteLine("Type help for a list of operations");
                var directInterpreter = new Interpreter();

                while (true)
                {
                    Console.Write("> ");
                    var toInterpret = Console.ReadLine();
                    Resources.
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