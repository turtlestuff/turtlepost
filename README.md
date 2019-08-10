# TurtlePost
TurtlePost is a simple, interpreted, dynamic, stack-based postfix language.

#### [Download TurtlePost 1.0.0](https://github.com/turtlestuff/turtlepost/releases)

## Running TurtlePost
TurtlePost releases are posted on the releases page with the .NET runtime embedded. You can also build TurtlePost from source using the standard `dotnet` CLI commands or a .NET IDE. To start the REPL, run the executable without any arguments. To run a `.tpost` script, pass the path as the first argument to the executable.

## TurtlePost Language
TurtlePost is designed strictly around stack-based execution. Expressions push and pop values from the main 'user stack' (or simply, 'the stack').

### Syntax
TurtlePost has very simple syntax. All TurtlePost scripts are comprised of one or more value or operation expressions, each separated by spaces. 

TurtlePost supports basic types of values:
- Double-precision floating point numbers (`2`, `.5`)
- Boolean values (`true` and `false`)
- Strings (`"Hello"`, `"Alien \U0001F47D"`)
- Globals (`&var`)
- Labels (`@label`)
- Null (`null`)

A list of all operations and their functions is described [below](#operations-reference).

Comments are started and ended with a forward slash (`/sample comment/`).

### The Stack
The stack is simply a 'last in, first out' buffer of objects. Typing a value expression into TurtlePost pushes that value onto the top of the stack. Operations can pop/consume values off of the top of the stack, perform operations on those values, and push new values onto the stack. The stack stores values of any type (even if operations do not work on all types). TurtlePost represents the topmost value on the stack as the one furthest to the right.

```
> 2 /pushes 2 onto the stack/
2
> 3 /pushes 3 onto the stack, on top of the 2/
2 | 3
```

### Postfix
Due to the stack-based nature of TurtlePost, operations are said to be *postfix*, or following their operands. To add two numbers, you would write something like this:
```
> 2 3 add
5
```
As we learned, writing `2 3` pushes both 2 and 3 onto the stack. <kbd>add</kbd> is an *operation* that, as may be apparent to you, adds numbers. Specifically, it pops two numbers from the top of the stack, adds the top one to the bottom, and pushes the resulting sum. There are many operations; just to demonstrate some easy ones:
```
> "hello" /pushes the string 'hello' to the stack/
"hello"
> print /pops the top value on the stack and prints it to the console/
hello
```

```
> 5 3 /pushes 5 and 3 to the stack/
5 | 3
> gt /pops the top 2 values, checks if the bottom is greater than the top, and pushes the result/
true
```

```
> help /prints a list of all operations/
add sub mul div mod... (remaining omitted for brevity)

> exit /exits the interpreter/
```
It may be helpful to think of some of these operations in terms of 'ordinary' mathematical notation. For example, `5 3 gt` corresponds to `5 > 3`.

### Globals
Globals are values that represent references to other values. A global has a name, which always starts with &. When you top use a global, it is declared in memory. Once they are created, they cannot be removed (although, they can be easily reused). Typing in the name of a global pushes the global itself to the stack.
```
> &var &var2 &var3
/Globals: &var = null, &var2 = null, &var3 = null/
&var | &var2 | &var3
```

Globals can be written to using the <kbd>write</kbd> operation. It pops a global and a value, writing the value into the global. Their value can be retrieved using the <kbd>push</kbd> operation, which pops a global and pushes its value. Even if there are no remaining references to a global on the stack, it is still there and its value can be referred to at a later time.
```
> 5 &var write
/Globals: &var = 5/

> &var push
/Globals: &var = 5/
5
```

### Labels
Labels are values that represent their source location. A label has a name, which always begins with @. Labels are declared with a trailing colon (`@label:`) and subsequently referred to without the colon (`@label`). Before input is interpreted, a quick pass of the source is done to scan for all label declarations. The interpreter also autogenerates a lebal called `@end` at the end of each input string.

Like globals, referring to a label pushes it onto the stack. The `jump` operation pops a label and unconditionally transfers control of the program to the code starting from the label. Unlike globals, labels are cleared every time an expression is evaluated in the REPL.
```
> "Hello" println
> 2 3 add println
> @label jump
> "I am skipped" println
> 
> @label:
>     "Jumped" println
Hello
5
Jumped
```
### Subroutines
Labels can also be used to denote *subroutines*. A subroutine is similar to a jump, but expected to use the `ret` operation to return to where it was called from. The `call` operation is used to jump to a subroutine.
```
4 @powerof2 call
print /prints 16/
@end jump

@powerof2:
    dup mul
    ret
```
The <kbd>call</kbd> operation pops a label, saves the current source location to the *call stack*, and unconditionally transfers control of the program to the code starting from the label. When the code hits a <kbd>ret</kbd> operation, it will pop the previous source location from the call stack and unconditionally start executing from there. 

Note that subroutines will still be executed as normal code if the interpreter reaches the subroutine's code. Since the call stack would be empty at this point, the `ret` operation would cause an error. Thus, it is advised to place all subroutines after an `@end jump` at the end of the file, so that the interpreter does not attempt to run subroutines with an empty call stack.

## Operations Reference
`add sub mul div mod ceil round floor sin cos tan write push concat print println input cls width height cursor dup drop swap over not and or xor eq gt lt gte lte string parse jump call jumpif callif ret exit nop help copying`

### Arithmetic
| Name             | Operation                                                                                 |
|:----------------:|-------------------------------------------------------------------------------------------|
| <kbd>add</kbd>   | Pops two numbers, adds the top to the bottom, and pushes the resulting sum.               |
| <kbd>sub</kbd>   | Pops two numbers, subtracts the top from the bottom, and pushes the resulting difference. |
| <kbd>mul</kbd>   | Pops two numbers, multiplies the top by the bottom, and pushes the resulting product.     |
| <kbd>div</kbd>   | Pops two numbers, divides the top by the bottom, and pushes the resulting quotient.       |
| <kbd>mod</kbd>   | Pops two numbers, divides the top by the bottom, and pushes the resulting remainder.      |

### Rounding
| Name             | Operation                                   |
|:----------------:|---------------------------------------------|
| <kbd>ceil</kbd>  | Pops a number and pushes it, rounded up.    |
| <kbd>round</kbd> | Pops a number and pushes it after rounding. |
| <kbd>floor</kbd> | Pops a numbers and pushes it, rounded down. |

### Trigonometric
| Name             | Operation                             |
|:----------------:|---------------------------------------|
| <kbd>sin</kbd>   | Pops a number and pushes its sine.    |
| <kbd>cos</kbd>   | Pops a number and pushes its cosine.  |
| <kbd>tan</kbd>   | Pops a number and pushes its tangent. |

### Globals
| Name             | Operation                                                          |
|:----------------:|--------------------------------------------------------------------|
| <kbd>write</kbd> | Pops a global and any value, and writes the value into the global. |
| <kbd>push</kbd>  | Pops a global and pushes the value of the global.                  |

### Strings
| Name              | Operation                                                                             |
|:-----------------:|---------------------------------------------------------------------------------------|
| <kbd>concat</kbd> | Pops two strings, concatenates the bottom with the top, and pushes the concatenation. |

### I/O
| Name               | Operation                                                                                                            |
|:------------------:|----------------------------------------------------------------------------------------------------------------------|
| <kbd>print</kbd>   | Pops any value and writes it to the standard output.                                                                 |
| <kbd>println</kbd> | Pops any value, writes it to the standard output, and writes an additional newline.                                  |
| <kbd>input</kbd>   | Reads a line of text from the standard input and pushes the string.                                                  |
| <kbd>cls</kbd>     | Clears the terminal.                                                                                                 |
| <kbd>width</kbd>   | Pushes the amount of characters the current terminal can show horizontally.                                          |
| <kbd>height</kbd>  | Pushes the amount of characters the current terminal can show vertically.                                            |
| <kbd>cursor</kbd>  | Pops two numbers, truncates them, and sets the them as the Y and X coordinates of the terminal cursor respectively.  |

### Stack Manipulation
| Name            | Operation                                                                                                                            |
|:---------------:|--------------------------------------------------------------------------------------------------------------------------------------|
| <kbd>dup</kbd>  | Pops a value and pushes it twice.                                                                                                    |
| <kbd>drop</kbd> | Pops a value, discarding it.                                                                                                         |
| <kbd>swap</kbd> | Pops two values and pushes them back in reverse order.                                                                               |
| <kbd>over</kbd> | Pops two values, and pushes the bottom, top, and bottom value again. This effectively pushes the second topmost object on the stack. |

### Boolean Logic
| Name            | Operation                                                              |
|:--------------:|------------------------------------------------------------------------|
| <kbd>not</kbd> | Pops a boolean, computes its logical NOT, and pushes it.               |
| <kbd>and</kbd> | Pops two booleans, computes their logical AND, and pushes it.          |
| <kbd>or</kbd>  | Pops two booleans, computes their logical OR, and pushes it.           |
| <kbd>xor</kbd> | Pops two booleans, computes their logical exclusive-OR, and pushes it. |

### Comparisons
| Name           | Operation                                                                                             |
|:--------------:|-------------------------------------------------------------------------------------------------------|
| <kbd>eq</kbd>  | Pops two values, checks if the bottom one equals the top, and pushes the result.                      |
| <kbd>gt</kbd>  | Pops two values, checks if the bottom one is greater than the top, and pushes the result.             |
| <kbd>lt</kbd>  | Pops two values, checks if the bottom one is less than the top, and pushes the result.                |
| <kbd>gte</kbd> | Pops two values, checks if the bottom one is greater than or equal to the top, and pushes the result. |
| <kbd>lte</kbd> | Pops two values, checks if the bottom one is less than or equal to the top, and pushes the result.    |

### Conversions
| Name              | Operation                                                      |
|:-----------------:|----------------------------------------------------------------|
| <kbd>string</kbd> | Pops a value, converts it to a string, and pushes the result.  |
| <kbd>parse</kbd>  | Pops a string, parses a number from it, and pushes the result. |

### Control Flow
| Name              | Operation                                                                                                                                         |
|:-----------------:|---------------------------------------------------------------------------------------------------------------------------------------------------|
| <kbd>jump</kbd>   | Pops a label and unconditionally transfers control to code starting from the label.                                                               |
| <kbd>call</kbd>   | Pops a label, pushes the current source location on the call stack, and unconditionally transfers control to the source code at the label.        |
| <kbd>jumpif</kbd> | Pops a label and a boolean and and transfers control to the source code at the label if the boolean is true.                                      |
| <kbd>callif</kbd> | Pops a label, pushes the current source location on the call stack, and transfers control to the source code at the label if the boolean is true. |
| <kbd>ret</kbd>    | Pops the previous source location from the call stack and unconditionally transfers control to the source code at that location.                  |

### Miscellaneous
| Name               | Operation                                                                           |
|:------------------:|-------------------------------------------------------------------------------------|
| <kbd>exit</kbd>    | Exits the process the interpreter is running in with exit code 0.                   |
| <kbd>nop</kbd>     | Performs no operation.                                                              |
| <kbd>help</kbd>    | Prints out a list of all operations the interpreter is configured to use.           |
| <kbd>copying</kbd> | Prints out information about redistribution, warranty, and licensing of TurtlePost. |

## Localizations
A special thanks for these folks for translating TurtlePost:
- **Esperanto** - Astro
- **Dutch** - JelleTheWhale
- **Portuguese** - [Vrabbers](https://github.com/Vrabbers)
- **Ukranian** - [John Tur](https://github.com/reflectronic)
- **Vietnamese** - [Victor Tran](https://github.com/vicr123)
