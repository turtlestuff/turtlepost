# TurtlePost
TurtlePost is a simple, dynamic, stack-based postfix language which is currently in development.

## Operations
```add ret nop mul print jump exit println call dup sub write push mod div input help drop```

## The stack
The stack is dynamic, which means it accepts all types. To push absolute values to the stack, you just type the values, followed by a space. So if you type ``2 4``, your stack will now be ``2 | 4``. Strings can be pushed using quotes and ``null``, ``true`` and ``false`` are pushed using their names. 

## Postfix
Postfix means the operation come after it's parameters. To do 5 - 4, for exaample, you would do ``5 4 sub``. To print a stirng, you do ``"hello world" println``. This has the implication that parentheses are no longer needed. (3 + 4) * 2 would be done as ``3 4 add 2 mul``, and so on.

## Globals
Globals always are prefixed with &. When you type &global, a "pointer" to the global called "global" is pushed onto the stack. The operation ``value &global write`` is used to write a value to the variable. The operation ``&global push`` pushed the value onto the stack. 

## Program flow
TODO: Write this.

## Comments
Comments must start and end with a ``/`` 

