using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
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

        static Interpreter directInterpreter = default!;
        static bool inputComplete;
        static StringBuilder currentInput = new StringBuilder(256); 
        static int caretPosition = 0;
        static bool insertKey = false;

        static ImmutableDictionary<KeyPressed, KeyResponse> Keys = new Dictionary<KeyPressed, KeyResponse>
        {
            {
                new KeyPressed(ConsoleKey.D, ConsoleModifiers.Control), (ref Diagnostic d) =>
                {
                    if (currentInput.Length == 0)
                    {
                        Console.WriteLine("exit");
                        directInterpreter.Interpret("exit", ref d);
                        inputComplete = true;
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
                    if (currentInput.Length != 0) 
                        directInterpreter.Interpret(currentInput.ToString(), ref d);
                    caretPosition = 0;
                    inputComplete = true;
                }
            },
            {
                new KeyPressed(ConsoleKey.Backspace, 0), (ref Diagnostic _) =>
                {
                    if (currentInput.Length == 0)
                    {
                        //Nothing has been typed
                        Console.Beep();
                    }
                    else
                    {
                        //Back up the caret and replace it with a space
                        Console.Write('\b');
                        caretPosition--;
                        currentInput = currentInput.Remove(caretPosition, 1);
                    }
                }
            },
            {
                new KeyPressed(ConsoleKey.C, ConsoleModifiers.Control), (ref Diagnostic _) =>
                {
                    //Reset the input buffer
                    Console.WriteLine();
                    if (currentInput.Length == 0)
                    {
                        //Print a message stating how to exit
                        Console.WriteLine(TR["exitExplanation"]);
                    }

                    inputComplete = true;
                }
            },
            {
                new KeyPressed(ConsoleKey.LeftArrow, 0), (ref Diagnostic _) =>
                {
                    if (currentInput.Length == 0)
                    {
                        //Nothing has been typed
                        Console.Beep();
                    }
                    else
                    {
                        //Back up the caret
                        Console.Write('\b');
                        caretPosition--;
                    }
                } 
            },
            {
                new KeyPressed(ConsoleKey.RightArrow, 0), (ref Diagnostic _) =>
                {
                    if (caretPosition == currentInput.Length)
                    {
                        //We're already at the end!
                        Console.Beep();
                    }
                    else
                    {
                        //Advance the caret(Doesn't matter we're erasing a character, we'll write it all again anyway.)
                        Console.Write(' ');
                        caretPosition++;
                    }
                } 
            },
            {
                new KeyPressed(ConsoleKey.Delete, 0), (ref Diagnostic _) =>
                {
                    if (caretPosition == currentInput.Length)
                    {
                        //We're at the end already!
                        Console.Beep();
                    }
                    else
                    {
                        currentInput.Remove(caretPosition, 1);
                    }
                } 
            },
            {
                new KeyPressed(ConsoleKey.Insert, 0), (ref Diagnostic _) =>
                {
                    insertKey = !insertKey;
                    Console.CursorSize = insertKey ? 100 : 25;
                }
            }
        }.ToImmutableDictionary();
    }
}