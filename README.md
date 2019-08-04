# TurtlePost
TurtlePost is a simple, dynamic, stack-based postfix language which is currently in development.

## Operations
```print input mul ret call nop callif println jumpif jump div write push dup help exit drop sub mod add```

## The stack
The stack is dynamic, which means it accepts all types. To push absolute values to the stack, you just type the values, followed by a space. So if you type ``2 4``, your stack will now be ``2 | 4``. Strings can be pushed using quotes and ``null``, ``true`` and ``false`` are pushed using their names. You can use ``dup`` to duplicate the top value on the stack, and ``drop`` to delete it.

## Postfix
Postfix means the operation come after it's parameters. To do 5 - 4, for exaample, you would do ``5 4 sub``. To print a stirng, you do ``"hello world" println``. This has the implication that parentheses are no longer needed. (3 + 4) * 2 would be done as ``3 4 add 2 mul``, and so on.

## Globals
Globals always are prefixed with &. When you type &global, a "pointer" to the global called "global" is pushed onto the stack. The operation ``value &global write`` is used to write a value to the variable. The operation ``&global push`` pushed the value onto the stack. 

## Program flow
Labels are created using ``@label:``. They are scanned for before any execution is done. There is also a hidden label called ``@end:`` which points to the end of the program. To put a label into the stack, use ``@label``. By using ``@label jump``, you can simply jump execution to that label. By using ``call``, you can make subroutines. ``call`` will also jump to the label specified, but also push the current position into the program stack (which is separate from the user stack). In the end of the subroutine, a ``ret`` instruction is expected, which will take the position from the program stack and go back. There are also conditional variants, ``condition @label jumpif`` and ``condition @label callif``, which only jump if the ``condition`` is ``true``.
Example: 
```2 @powerof2 call
@end jump

@powerof2
    dup mul
    ret
```
    
## Comments
Comments must start and end with a ``/`` 

