using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using static TurtlePost.I18N;

namespace TurtlePost
{
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

        static ImmutableDictionary<KeyPressed, Action> Keys = new Dictionary<KeyPressed, Action>
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
                    if (CurrentInput.Length != 0) 
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
                        Console.WriteLine(TR["exitExplanation"]);
                    }

                    InputComplete = true;
                }
            }
        }.ToImmutableDictionary();
    }
}