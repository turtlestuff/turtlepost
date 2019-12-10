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

        static Interpreter DirectInterpreter = default!;
        static bool InputComplete;
        static StringBuilder CurrentInput = new StringBuilder(256); 
        static int CaretPosition = 0;

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
                    CaretPosition = 0;
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
                        Console.Write('\b');
                        CaretPosition--;
                        CurrentInput = CurrentInput.Remove(CaretPosition, 1);
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
            },
            {
                new KeyPressed(ConsoleKey.LeftArrow, 0), (ref Diagnostic _) =>
                {
                    if (CurrentInput.Length == 0)
                    {
                        //Nothing has been typed
                        Console.Beep();
                    }
                    else
                    {
                        //Back up the caret
                        Console.Write('\b');
                        CaretPosition--;
                    }
                } 
            },
            {
                new KeyPressed(ConsoleKey.RightArrow, 0), (ref Diagnostic _) =>
                {
                    if (CaretPosition == CurrentInput.Length)
                    {
                        //We're already at the end!
                        Console.Beep();
                    }
                    else
                    {
                        //Advance the caret(Doesn't matter we're erasing a character, we'll write it all again anyway.)
                        Console.Write(' ');
                        CaretPosition++;
                    }
                } 
            }
        }.ToImmutableDictionary();
    }
}