# Forth

## An Overview of Forth

Forth is a low-level, stack based language. It is not particularly well defined and there isn't really any standard for what a language must have to be Forth. There are no enforced types. Everything is just data that must be interpereted correctly.

## Stack-Based Language

Push things onto the stack:

`1 2 3`

Print (but not clear) the stack with `.s`:

`.s \prints 1 2 3`

Use words to operate on the stack values:

`1 2 + \ consumes 1 and 2 and pushes the sum onto the stack`

Because of this stack structure all of the operators are postfix. Words are defined with `:` and `;`. This defines a square function:

`: square dup * ;`

`:` is the command to enter compiler mode and `;` exits compiler mode. `square` is the defined word. `dup` duplicates the item on the top of the stack and `*` multiplies this duplicate by the original.

(Also of note, Forth is case-insensitive)

The existance of the stack not mean the stack is the only way to store data; memory can be allocated, etc. Instead think of it as a way to pass arguments to functions. 

## Words

Words in Forth can't have defined parameters. In order to make words more readable, the convention is to follow a definition with a comment describing the state of the stack before and after the word is called, e.g.:

`: square ( n -- n^2 ) 
    dup * ;`
    
Conditionals and loops of course make an appearance.

`: abs ( n -- m )
    dup 0 < if 
        negate
    then ;`

`: fac ( n -- n! )
    1 swap 1+ 1 do
        i *
    loop ;`

(`do` consumes two values from the stack, a limit and a initial index. The current index can be accessed in the loop body with `i`)

There are a few loop variations but we won't go throught them all here.

Some words can only be used in the definitions of other words. For example, `if` cannot be used in the repl. These words are called immediate words. This means in compiler mode these words are executed immediately. For example:

`: compileShow5 5 . ; immediate
: example compileShow5 ; \ prints 5 **when its compiled**`

This is how a lot of branching statements are implemented. Immediate values are used to save the memory locations **of the instructions** in order to return to them.

## Bootstrapping Your Forth

Forth is a language that is meant to be built on. This is an implementation of comments in Forth:

`: ( 41 word drop ;`

There are a few things to unpack here. As we said, `:` starts the word definition and `(` is the word being defined. `word` reads the input stream until it recieves a value with the proceeding ASCII code (41 is ')'). `drop` then drops everything `word` collected from the stream. In this way everything in between parentheses is dropped. 

## Memory in Forth

Memory in Forth is yet another stack. This stack has dictionary labels, and so it is referred to as dictionary memory. For example a portion of the memory might look like this:

|      MYITEM       |
|-------------------|
|      <Some>       |
|-------------------|
|      <meta>       |
|-------------------|
|      <stuff>      |
|-------------------|
|         0         |<
|-------------------|
|         0         |
|-------------------|
|         0         |

Where < is the dictionary pointer (points at the first free address on the memory stack). Memory locations can be accessed with `@` ( adr -- val ) and stored with `!` (val adr -- ). Value size is not handled for you; instead the word `cell` ( -- size ) gives the size of a block in memory in order to properly access adjacent cells on the dictionary. For example, on my machine all values in Forth take 8 bytes, so `cell` just pushes 8. `here` ( -- adr ) puts the address of the top of the user dictionary onto the stack. `allot` moves this pointer to allocate more space. `,` is a common word that might be defined as:

`: ,  here ! cell allot ;`

This is all well and good for manipulating things in already existant dictionary entries, but how do you add new entries?

## CREATE

MYITEM is the dictionary heading. These headings are created by the `create` word. Lets take a look at the `variable` word. A definition for this word might be:

`: variable create 0 , ;`

`variable myItem`

`1 myItem !`

`myItem @ . \ prints 1`

`create` reads the next word from stdin and creates a dictionary entry with it. It then leaves the address of the next open memory location on the stack. Notice that it does not use the stack to get the name of the entry.

0 is then pushed onto the stack. `,` is a word that takes the value before it and puts it into the next memory slot. `!` ( a b -- ) stores a at memory location b and `@` ( a -- ) retreives the value at memory location a. So after `1 myItem !` is run, the same location as above might look like:

|      MYITEM       |
|-------------------|
|      <Some>       |
|-------------------|
|      <meta>       |
|-------------------|
|      <stuff>      |
|-------------------|
|         1         |
|-------------------|
|         0         |<
|-------------------|
|         0         |

This is actually how all words in Forth are stored. Without going into it too much, words that execute other words simply store the executed words' memory pointers under its dictionary entry ("meta stuff" in the diagram).

This creation happens when `variable` is executed, but it is important to note that it occurs during `myItem`'s **compile time**. There is another block of code that is executed during `myItems`'s **runtime**, but in this example it is the default behavior.

This block can be designated by `does>`. `does>` provides, at runtime, the address following the word's code. In the example case, it pushes the location of the 1 onto the stack. If we didn't want the address and instead wanted, say, a constant value we could define it as:

`: const create , does> @ ;`

This would store the proceeding value in memory with , under the following dictionary header at compile time. At run time it would access the memory address with @ and return the value stored there.
