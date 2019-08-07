using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static TurtlePost.I18n;

namespace TurtlePost
{
    static class Program
    {
        struct KeyPressed
        {
            ConsoleKey key;
            ConsoleModifiers modifiers;

            public KeyPressed(ConsoleKey key, ConsoleModifiers modifiers)
            {
                this.key = key;
                this.modifiers = modifiers;
            }
        }

        static Interpreter DirectInterpreter;
        static bool InputComplete = false;
        static StringBuilder CurrentInput = new StringBuilder(256);

        static Dictionary<KeyPressed, Action> Keys = new Dictionary<KeyPressed, Action>
        {
            {
                new KeyPressed(ConsoleKey.D, ConsoleModifiers.Control), () =>
                {
                    if (CurrentInput.Length == 0)
                    {
                        Console.WriteLine("exit");
                        DirectInterpreter.Interpret("exit", true);
                        InputComplete = true;
                    }
                    else
                    {
                        //There is currently input
                        Console.Beep();
                    }
                }
            },
            {
                new KeyPressed(ConsoleKey.Enter, 0), () =>
                {
                    //Interpret the current line
                    Console.WriteLine();
                    DirectInterpreter.Interpret(CurrentInput.ToString(), true);
                    InputComplete = true;
                }
            },
            {
                new KeyPressed(ConsoleKey.Backspace, 0), () =>
                {
                    if (CurrentInput.Length == 0)
                    {
                        //Nothing has been typed
                        Console.Beep();
                    }
                    else
                    {
                        //Back up the caret and replace it with a space
                        Console.Write("\b \b");
                        CurrentInput = CurrentInput.Remove(CurrentInput.Length - 1, 1);
                    }
                }
            },
            {
                new KeyPressed(ConsoleKey.C, ConsoleModifiers.Control), () =>
                {
                    //Reset the input buffer
                    Console.WriteLine();
                    if (CurrentInput.Length == 0)
                    {
                        //Print a message stating how to exit
                        Console.WriteLine(_("exitExplanation"));
                    }

                    InputComplete = true;
                }
            }
        };

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(_("versionString"), Assembly.GetExecutingAssembly().GetName().Version);
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
                        else if (key.Modifiers == 0)
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