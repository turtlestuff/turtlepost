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
The stack is simply a 'last in, first out' buffer of objects. Typing a value expression into TurtlePost pushes that value onto the top of the stack. Operations can pop/consume values off of the top of the stack, perform operations on those values, and push new values onto the stack. The stack stores values of any type (even if operations do not work on all types).

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
As we learned, writing `2 3` pushes both 2 and 3 onto the stack. `add` is an *operation* that, as may be apparent to you, adds numbers. Specifically, it pops two numbers from the top of the stack, adds the first one to the second, and pushes the resulting sum. There are many operations; just to demonstrate some easy ones:
```
> "hello" /pushes the string 'hello' to the stack/
"hello"
> print /pops the top value on the stack and prints it to the console/
hello
```

```
> 5 3 /pushes 5 and 3 to the stack/
5 | 3
> gt /pops the top 2 values, checks if the second is greater than the first, and pushes the result/
true
```

```
> help /prints a list of all operations/
add sub mul div mod... (remaining omitted for brevity)

> exit /exits the interpreter/
```
It may be helpful to think of some of these operations in terms of 'ordinary' mathematical notation. For example, `5 3 gt` corresponds to `5 > 3`.

### Globals
Globals are values that represent references to other values. A global has a name, which always starts with &. When you first use a global, it is declared in memory. Once they are created, they cannot be removed (although, they can be easily reused). Typing in the name of a global pushes the global itself to the stack.
```
> &var &var2 &var3
/Globals: &var = null, &var2 = null, &var3 = null/
&var | &var2 | &var3
```

Globals can be written to using the `write` operation. It pops a global and a value, writing the value into the global. Their value can be retrieved using the `push` operation, which pops a global and pushes its value. Even if there are no remaining references to a global on the stack, it is still there and its value can be referred to at a later time.
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
The `call` operation pops a label, saves the current source location to the *call stack*, and unconditionally transfers control of the program to the code starting from the label. When the code hits a `ret` operation, it will pop the previous source location from the call stack and unconditionally start executing from there. 

Note that subroutines will still be executed as normal code if the interpreter reaches the subroutine's code. Since the call stack would be empty at this point, the `ret` operation would cause an error. Thus, it is advised to place all subroutines after an `@end jump` at the end of the file, so that the interpreter does not attempt to run subroutines with an empty call stack.

## Operations Reference
`add sub mul div mod write push concat print println input dup drop swap over not and or xor eq gt lt gte lte string parse jump call jumpif callif ret exit nop help copying`

### Math
| Name  | Operation                                                                                                    |
|-------|--------------------------------------------------------------------------------------------------------------|
| `add` | Pops two values, adds the first to the second, and pushes the resulting sum.                                 |
| `sub` | Pops two values, subtracts the first from the second, and pushes the resulting difference.                   |
| `mul` | Pops two values, multiplies the first by the second, and pushes the resulting product.                       |
| `div` | Pops two values, divides the first by the second, and pushes the resulting quotient.                         |
| `mod` | Pops two values, divides the first by the second, and pushes the resulting remainder.                        |

### Globals
| Name    | Operation                                                          |
|---------|--------------------------------------------------------------------|
| `write` | Pops a global and any value, and writes the value into the global. |
| `push`  | Pops a global and pushes the value of the global.                  |

### Strings
| Name     | Operation                      |
|----------|--------------------------------|
| `concat` | Pops two strings, concats them |

### I/O
| Name      | Operation                                                                           |
|-----------|-------------------------------------------------------------------------------------|
| `print`   | Pops any value and writes it to the standard output.                                |
| `println` | Pops any value, writes it to the standard output, and writes an additional newline. |
| `input`   | Reads a line of text from the standard input and pushes the string.                 |

### Stack Manipulation
| Name   | Operation                                               | 
|--------|---------------------------------------------------------|
| `dup`  | Pops a value and pushes it twice.                       |
| `drop` | Pops a value, discarding it.                            |
| `swap` | Pops two values and pushes them back in reverse order.  |
| `over` | x \| y -> x \| y \| x (how do i explain this?)          |

### Boolean Logic
| Name  | Operation                                                              |
|-------|------------------------------------------------------------------------|
| `not` | Pops a boolean, computes its logical NOT, and pushes it.               |
| `and` | Pops two booleans, computes their logical AND, and pushes it.          |
| `or`  | Pops two booleans, computes their logical OR, and pushes it.           |
| `xor` | Pops two booleans, computes their logical exclusive-OR, and pushes it. |

### Comparisons
| Name  | Operation                                                                                               |
|-------|---------------------------------------------------------------------------------------------------------|
| `eq`  | Pops two values, checks if the econd one equals the first, and pushes the result.                       |
| `gt`  | Pops two values, checks if the second one is greater than the first, and pushes the result.             |
| `lt`  | Pops two values, checks if the second one is less than the first, and pushes the result.                |
| `gte` | Pops two values, checks if the second one is greater than or equal to the first, and pushes the result. |
| `lte` | Pops two values, checks if the second one is less than or equal to the first, and pushes the result.    |

### Conversions
| Name     | Operation                                                      |
|----------|----------------------------------------------------------------|
| `string` | Pops a value, converts it to a string, and pushes the result.  |
| `parse`  | Pops a string, parses a number from it, and pushes the result. |

### Control Flow
| Name     | Operation                                                                                                                                         |
|----------|---------------------------------------------------------------------------------------------------------------------------------------------------|
| `jump`   | Pops a label and unconditionally transfers control to code starting from the label.                                                               |
| `call`   | Pops a label, pushes the current source location on the call stack, and unconditionally transfers control to code starting from the label.        |
| `jumpif` | Pops a label and a boolean and transfers control to code starting from the label if the boolean is true.                                          |
| `callif` | Pops a label, pushes the current source location on the call stack, and transfers control to code starting from the label if the boolean is true. |
| `ret`    | Pops the previous source location from the call stack and unconditionally transfers control to source starting at that location.                  |

### Miscellaneous
| Name      | Operation                                                                           |
|-----------|-------------------------------------------------------------------------------------|
| `exit`    | Exits the process the interpreter is running in with exit code 0.                   |
| `nop`     | Performs no operation.                                                              |
| `help`    | Prints out a list of all operations the interpreter is configured to use.           |
| `copying` | Prints out information about redistribution, warranty, and licensing of TurtlePost. |

## Localizations
A special thanks for these folks for translating TurtlePost:\
Esperanto - Astro\
Dutch - JelleTheWhale\
Portuguese - Vrabbers\
Ukranian - Reflectronic\
Vietnamese - Victor Tran
