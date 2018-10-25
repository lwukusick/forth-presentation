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

\ Part 3: Given the following code for making an aray, make a word to access a location in the array

: array create allot does> + ;
