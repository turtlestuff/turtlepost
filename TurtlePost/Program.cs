using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.IO;
using static TurtlePost.I18n;

namespace TurtlePost
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(string.Format(_("versionString"), Assembly.GetExecutingAssembly().GetName().Version));
                Console.WriteLine("By Vrabbers and Reflectronic, (c) 2019");
                Console.WriteLine(_("helpPrompt"));
                var directInterpreter = new Interpreter();

                while (true)
                {
                    Console.Write("> ");
                    var toInterpret = Console.ReadLine();
                    
                    if (toInterpret == null)
                    {
                        toInterpret = "exit";
                        Console.WriteLine(toInterpret);
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
                Console.WriteLine(_("exitPrompt"));
                Console.ReadKey();
            }
        }
    }
}