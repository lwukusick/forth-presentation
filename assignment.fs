\ To load a source file into your repl, run: s" <file path>" included
\ To run all the tests run: runAllTests


: runAllTests 
  testSquare 
  test^4 ;

\ Here are some stack manipulations for your reference
\ The signatures are in the form ( <stack in> -- <stack out> )
\ dup   ( a -- a a )
\ drop  ( a -- )
\ swap  ( a b -- b a )
\ over  ( a b -- a b a )
\ nip   ( a b -- b )
\ tuck  ( a b -- b a b )
\ rot   ( a b c -- b c a )
\ -rot  ( a b c -- c a b )

\ The first two parts are a few trivial problems to get you used to the stack

\ Part 1: Define a word that squares a value on the stack called `square` ( n -- n^2 )





: testSquare 3 square 
  dup 9 = if 
    ." Passed"
    drop
  else
    ." Square Failed: Expected 9 got "
    .
  then space emit ;


\ Part 2: Define a word that takes a value to the 4th power called `^4` ( n -- n^4 )





: test^4 2 ^4 
  dup 16 = if
    ." Passed"
    drop
  else 
    ." ^4 Failed: Expected 16 got "
    .
  then space emit ;

\ Part 3: Here is code which creates an array which checks bounds
          Make a function which creates a 2d array (you don't have to worry about checking bounds)

: safearray CREATE DUP , ALLOT
            DOES> 2DUP @ U< 0= ABORT" ARRAY OUT OF BOUNDS "
                  + CELL+ ;

(EXPLANATION:
  Example - 10 safearray x
            12 x -- ABORT ARRAY OUT OF BOUNDS
            5 2 x !
            2 x @ -- 5
  DOES> initial stack ( index address)
  2DUP ( index address index address )
  @ ( index address index #elems )
  U< ( index address (index < #elems) [Unsigned compare]
  0= ( index address (index < # elems == 0) ) [ 0 is false, 0= negates a boolean]
  ABORT" [ If top of stack is not 0, abort with message, else continue
  + ( index+address )
  CELL+ ( index+address+8 ) [ cell+ adds the size of a cell to the top of the stack, need to add 1 to get beyond the length which is stored at the first cell]
  


