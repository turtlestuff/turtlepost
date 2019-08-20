using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using static TurtlePost.I18N;

namespace TurtlePost
{
    delegate void KeyResponse(ref Diagnostic diagnostic);
    
    static partial class Program
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
        static bool InputComplete;
        static StringBuilder CurrentInput = new StringBuilder(256);

        static ImmutableDictionary<KeyPressed, KeyResponse> Keys = new Dictionary<KeyPressed, KeyResponse>
        {
            {
                new KeyPressed(ConsoleKey.D, ConsoleModifiers.Control), (ref Diagnostic d) =>
                {
                    if (CurrentInput.Length == 0)
                    {
                        Console.WriteLine("exit");
                        DirectInterpreter.Interpret("exit", ref d);
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
                new KeyPressed(ConsoleKey.Enter, 0), (ref Diagnostic d) =>
                {
                    //Interpret the current line
                    Console.WriteLine();
                    if (CurrentInput.Length != 0) 
                        DirectInterpreter.Interpret(CurrentInput.ToString(), ref d);
                    
                    InputComplete = true;
                }
            },
            {
                new KeyPressed(ConsoleKey.Backspace, 0), (ref Diagnostic _) =>
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
                new KeyPressed(ConsoleKey.C, ConsoleModifiers.Control), (ref Diagnostic _) =>
                {
                    //Reset the input buffer
                    Console.WriteLine();
                    if (CurrentInput.Length == 0)
                    {
                        //Print a message stating how to exit
                        Console.WriteLine(TR["exitExplanation"]);
                    }

                    InputComplete = true;
                }
            }
        }.ToImmutableDictionary();
    }
}