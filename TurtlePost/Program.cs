using System;
using System.Diagnostics;
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

                directInterpreter = new Interpreter();

                while (true)
                {
                    Console.Write("> ");
                    (int initCX, int initCY) = (Console.CursorLeft, Console.CursorTop);
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
                            if (caretPosition == currentInput.Length)
                            {
                                currentInput.Append(key.KeyChar);
                                caretPosition++;
                            }
                            else
                            {
                                if (!insertKey) currentInput.Insert(caretPosition, key.KeyChar);
                                else currentInput[caretPosition] = key.KeyChar;
                                caretPosition++;
                            }
                        }
                        (int oldCX, int oldCY) = (Console.CursorLeft, Console.CursorTop);
                        Console.SetCursorPosition(initCX, initCY);
                        Console.Write(currentInput+" ");
                        Console.SetCursorPosition(oldCX, oldCY);
                    } while (!inputComplete);

                    currentInput.Clear();
                    inputComplete = false;

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