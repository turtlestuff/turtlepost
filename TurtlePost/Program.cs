using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.IO;

namespace TurtlePost
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("TurtlePost");
                Console.WriteLine("By Vrabbers and Reflectronic, 2019");
                Console.WriteLine("To get the operation dictionary, type help");
                var directInterpreter = new Interpreter();

                while (true)
                {
                    Console.Write(">");
                    var toInterpret = Console.ReadLine();

                    directInterpreter.Interpret(toInterpret,true);
                }
            } else
            {
                //load Felis
                string code = File.ReadAllText(args[0]);
                new Interpreter().Interpret(code,false);
                Console.WriteLine();
                Console.WriteLine("Press a key to exit...");
                Console.ReadKey();
            }
        }
    }
}