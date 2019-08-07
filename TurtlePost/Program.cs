using System;
using System.IO;
using System.Reflection;
using static TurtlePost.I18n;

namespace TurtlePost
{
    static partial class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(_("versionString"), Assembly.GetExecutingAssembly().GetName().Version + " 🐢");
                Console.WriteLine("By Vrabbers and Reflectronic, (c) 2019");
                Console.WriteLine(_("helpPrompt"));

                // Set up the console
                Console.TreatControlCAsInput = true;

                DirectInterpreter = new Interpreter();

                while (true)
                {
                    Console.Write("> ");
                    do
                    {
                        var key = Console.ReadKey(true);
                        var keyPressed = new KeyPressed(key.Key, key.Modifiers);
                        if (Keys.TryGetValue(keyPressed, out var func))
                        {
                            func();
                        }
                        else if (key.Modifiers == 0 && !char.IsControl(key.KeyChar))
                        {
                            Console.Write(key.KeyChar);
                            CurrentInput.Append(key.KeyChar);
                        }
                    } while (!InputComplete);

                    CurrentInput.Clear();
                    InputComplete = false;
                }
            }
            else
            {
                // Load file
                var code = File.ReadAllText(args[0]);
                new Interpreter().Interpret(code, false);
                Console.WriteLine();
                Console.WriteLine(_("exitPrompt"));
                Console.ReadKey();
            }
        }
    }
}