# Assignment 9

Authors: adbo, aljb and ahja

This code can also be found online at <https://github.com/andreaswachs/ITU-PRDAT22>.

We recommend reading this Markdown page on [GitHub](https://github.com/andreaswachs/ITU-PRDAT22/blob/main/assignment9/README.md) or VSCode because it makes the diffs easier to read.

## Solutions

### PLC 10.1

Answers with descriptions:

#### I

* `ADD`: When we store numbers in the program memory, it is done by tagging integers, making us able to distinguish between references and integer values. This makes it so that the rightmost bit is 0 for references and 1 for integer values. When adding, we untag the two topmost numbers on the stack. We add the numbers together and tag the sum, leaving that sum on top of the stack. The stack pointer is decreased by one.
* `CSTI i`: We wish to store the constant `i`. We tag the integer `i` and then push it atop of the stack, while increasing the program and stack pointer.
* `NIL`: NIL is a reference type, since it has a `0` as its least siginficant bit, while a value type has a 1 in its least significant bit.
* `IFZERO`: We make a check to see if the top most item on the stack is an integer. If yes, the then we untag the value and check whether it is zero or not. Should that check be true, we make the jump. If there is a reference value on top of the stack, then we only make a jump if the value is not a NIL value. It should be noted that the ternary operator is actually unnecessary here since the least significant bit is ignored either way. You could replace the ternary with a simple `UnTag(v) == 0`.
* `CONS`: We allocate a new cons cell on the heap. The cons cell is marked with the object type CONS and contains the two values on top of the stack. We put the new cons cell on the penultimate location in the stack and decrease the stack pointer by one, making the cons cell the topmost item on the stack.
* `CAR`: Given a reference to a cons cell and we make a NIL check on the reference located at the top of the stack. Should it not be a NIL reference, we take the first component out of the cell. We pop the cons cell reference and push the car onto the stack.
* `SETCAR`: We pop the two topmost items on the stack: a value and a reference to a cons cell located on the heap. The reference is used to set the first component of the car to the value.

#### II

Answers:

* Applying `Length` macro: Determines the number of blocks associated with the header.
* Applying `Colour` macro: Gets the colour of the block header
* Applying `Paint` macro: Sets the colour of the block header

#### III

Answers:

The abstract machine calls `allocate()` only when interpreting the CONS instruction. The cons list is the only heap allocated data structure in this abstract machine.

There are no other interactions between the mutator and the garbage collector.

#### IIII

`collect()` is only called within the function `allocate()` in the case where there are no more heap space available to allocate.


### PLC 10.2


### PLC 10.3



