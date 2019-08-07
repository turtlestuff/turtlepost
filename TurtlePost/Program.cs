using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static TurtlePost.I18n;

namespace TurtlePost
{
    static class Program
    {
        struct KeyPressed {
            ConsoleKey key;
            ConsoleModifiers modifiers;
            
            public KeyPressed(ConsoleKey key, ConsoleModifiers modifiers) {
                this.key = key;
                this.modifiers = modifiers;
            }
        }
        
        private static Interpreter DirectInterpreter;
        private static bool InputComplete = false;
        private static string CurrentInput = "";
        private static Dictionary<KeyPressed, Action> Keys = new Dictionary<KeyPressed, Action> {
            {new KeyPressed(ConsoleKey.D, ConsoleModifiers.Control), () => {
                if (CurrentInput == "") {
                    DirectInterpreter.Interpret("exit", true);
                    InputComplete = true;
                } else {
                    //There is currently input
                    Console.Beep();
                }
            }},
            {new KeyPressed(ConsoleKey.Enter, 0), () => {
                //Interpret the current line
                Console.WriteLine();
                DirectInterpreter.Interpret(CurrentInput, true);
                InputComplete = true;
            }},
            {new KeyPressed(ConsoleKey.Backspace, 0), () => {
                if (CurrentInput == "") {
                    //Nothing has been typed
                    Console.Beep();
                } else {
                    //Back up the caret and replace it with a space
                    Console.Write("\b \b");
                    CurrentInput = CurrentInput[..^1].ToString();
                }
            }},
            {new KeyPressed(ConsoleKey.C, ConsoleModifiers.Control), () => {
                //Reset the input buffer
                Console.WriteLine();
                InputComplete = true;
            }}
        };
        
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(string.Format(_("versionString"), Assembly.GetExecutingAssembly().GetName().Version));
                Console.WriteLine("By Vrabbers and Reflectronic, (c) 2019");
                Console.WriteLine(_("helpPrompt"));
                
                // Set up the console
                Console.TreatControlCAsInput = true;
                
                DirectInterpreter = new Interpreter();

                while (true)
                {
                    Console.Write("> ");
                    do {
                        var key = Console.ReadKey(true);
                        var keyPressed = new KeyPressed(key.Key, key.Modifiers);
                        if (Keys.ContainsKey(keyPressed)) {
                            Keys[keyPressed]();
                        } else if (key.Modifiers == 0) {
                            Console.Write(key.KeyChar);
                            CurrentInput += key.KeyChar;
                        }
                    } while (!InputComplete);
                    
                    CurrentInput = "";
                    InputComplete = false;
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