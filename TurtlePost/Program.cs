using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using static TurtlePost.I18N;

namespace TurtlePost
{
    static partial class Program
    {
        static void Main(string[] args)
        {
            Diagnostic d = default;
            if (args.Length == 0)
            {
                Console.WriteLine(TR["version"],
                    typeof(Interpreter).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
                        .InformationalVersion);
                
                Console.WriteLine("(C) 2019 Vrabbers, Reflectronic");
                Console.WriteLine(TR["copying"]);
                Console.WriteLine(TR["help"]);

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
                            func(ref d);
                        }
                        else if (!char.IsControl(key.KeyChar))
                        {
                            Console.Write(key.KeyChar);
                            CurrentInput.Append(key.KeyChar);
                        }
                    } while (!InputComplete);

                    CurrentInput.Clear();
                    InputComplete = false;

                    if (d.Type != DiagnosticType.None)
                        d = default;
                }
            }
            else
            {
                // Load file
                var code = File.ReadAllText(args[0]);
                new Interpreter().Interpret(code, ref d, new Interpreter.InterpretationOptions { HideStack = true });
                Console.WriteLine();
                Console.WriteLine(TR["exit"]);
                Console.ReadKey();
            }
        }
    }
}