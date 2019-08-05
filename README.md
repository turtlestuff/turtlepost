# TurtlePost
TurtlePost is a simple, dynamic, stack-based postfix language which is currently in development.

## The stack
The stack is dynamic, which means it accepts all types. To push absolute values to the stack, you just type the values, followed by a space. So if you type ``2 4``, your stack will now be ``2 | 4``. Strings can be pushed using quotes and ``null``, ``true`` and ``false`` are pushed using their names. You can use ``dup`` to duplicate the top value on the stack, and ``drop`` to delete it.

## Postfix
Postfix means the operation come after it's parameters. To do 5 - 4, for exaample, you would do ``5 4 sub``. To print a stirng, you do ``"hello world" println``. This has the implication that parentheses are no longer needed. (3 + 4) * 2 would be done as ``3 4 add 2 mul``, and so on.

## Globals
Globals always are prefixed with &. When you type &global, a "pointer" to the global called "global" is pushed onto the stack. The operation ``value &global write`` is used to write a value to the variable. The operation ``&global push`` pushed the value onto the stack. 

## Program flow
Labels are created using ``@label:``. They are scanned for before any execution is done. There is also a hidden label called ``@end:`` which points to the end of the program. To put a label into the stack, use ``@label``. By using ``@label jump``, you can simply jump execution to that label. By using ``call``, you can make subroutines. ``call`` will also jump to the label specified, but also push the current position into the program stack (which is separate from the user stack). In the end of the subroutine, a ``ret`` instruction is expected, which will take the position from the program stack and go back. There are also conditional variants, ``condition @label jumpif`` and ``condition @label callif``, which only jump if the ``condition`` is ``true``.
Example: 
```
4 @powerof2 call
print /prints 16/
@end jump

@powerof2:
    dup mul
    ret
```
    
## Comments
Comments must start and end with a ``/`` 

## Operations Reference
`add sub mul div mod write push print println input dup drop not and or xor eq gt lt gte lte string parse jump call jumpif callif ret exit nop help`
### Math
| Name  | Operation                                                                                          |
|-------|----------------------------------------------------------------------------------------------------|
| `add` | Pops two values, adds the first to the second, and pushes the sum.                                 |
| `sub` | Pops two values, subtracts the first from the second, and pushes the difference.                   |
| `mul` | Pops two values, multiplies the first by the second, and pushes the product.                       |
| `div` | Pops two values, divides the first by the second, and pushes the quotient.                         |
| `mod` | Pops two values, divides the first by the second, and pushes the remainder.                        |

### Globals
| Name    | Operation                                                          |
|---------|--------------------------------------------------------------------|
| `write` | Pops a global and any value, and writes the value into the global. |
| `push`  | Pops a global and pushes the value of the global.                  |

### I/O
| Name      | Operation                                                                           |
|-----------|-------------------------------------------------------------------------------------|
| `print`   | Pops any value and writes it to the standard output.                                |
| `println` | Pops any value, writes it to the standard output, and writes an additional newline. |
| `input`   | Reads a line of text from the standard input and pushes the string.                 |

### Stack Manipulation
| Name   | Operation                         |
|--------|-----------------------------------|
| `dup`  | Pops a value and pushes it twice. |
| `drop` | Pops a value, discarding it.      |


### Boolean Logic
| Name  | Operation                                                              |
|-------|------------------------------------------------------------------------|
| `not` | Pops a boolean, computes its logical NOT, and pushes it.               |
| `and` | Pops two booleans, computes their logical AND, and pushes it.          |
| `or`  | Pops two booleans, computes their logical OR, and pushes it.           |
| `xor` | Pops two booleans, computes their logical exclusive-OR, and pushes it. |

### Comparisons
| Name  | Operation                                                                                                 |
|-------|-----------------------------------------------------------------------------------------------------------|
| `eq`  | Pops two values, checks if the first one equals the second, and pushes the result.                        |
| `gt`  | Pops two booleans, checks if the first one is greater than the second, and pushes the result.             |
| `lt`  | Pops two booleans, checks if the first one is less than the second, and pushes the result.                |
| `gte` | Pops two booleans, checks if the first one is greater than or equal to the second, and pushes the result. |
| `lte` | Pops two booleans, checks if the first one is less than or equal to the second, and pushes the result.    |

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
| Name   | Operation                                                                 |
|--------|---------------------------------------------------------------------------|
| `exit` | Exits the process the interpreter is running in with exit code 0.         |
| `nop`  | Performs no operation.                                                    |
| `help` | Prints out a list of all operations the interpreter is configured to use. |
