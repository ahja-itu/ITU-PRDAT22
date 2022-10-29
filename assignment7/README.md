# Assignment 5

Authors: adbo, aljb and ahja

This code can also be found online at <https://github.com/andreaswachs/ITU-PRDAT22>.

We recommend reading this Markdown page on [GitHub](https://github.com/andreaswachs/ITU-PRDAT22/blob/main/assignment7/README.md) or VSCode because it makes the diffs easier to read.

## Solutions

### Exercise 8.1

#### I

```sh
$ fslex --unicode CLex.fsl
(...)

$ fsyacc --module CPar CPar.fsy
(...)

$ fsharpi -r ~/fsharp/FsLexYacc.Runtime.dll Absyn.fs CPar.fs CLex.fs Parse.fs Machine.fs Comp.fs ParseAndComp.fs   
(...)

> open ParseAndComp;;
> compileToFile (fromFile "ex11.c") "ex11.out";;
> compile "ex11";;
> #q;;
$ javac Machine.java
(...)
$ java Machine ex11.out 8
1 5 8 6 3 7 2 4
1 6 8 3 7 4 2 5
1 7 4 6 8 2 5 3
(...)
8 2 5 3 1 7 4 6
8 3 1 6 2 5 7 4
8 4 1 3 6 2 7 5

Ran 0.025 seconds
```


#### II


Output for compiling and running `ex3.c`

```sh
> open ParseAndComp;;
> compileToFile (fromFile "ex3.c") "ex3.out";;
val it : Machine.instr list =
  [LDARGS; CALL (1, "L1"); STOP; Label "L1"; INCSP 1; GETBP; CSTI 1; ADD;
   CSTI 0; STI; INCSP -1; GOTO "L3"; Label "L2"; GETBP; CSTI 1; ADD; LDI;
   PRINTI; INCSP -1; GETBP; CSTI 1; ADD; GETBP; CSTI 1; ADD; LDI; CSTI 1; ADD;
   STI; INCSP -1; INCSP 0; Label "L3"; GETBP; CSTI 1; ADD; LDI; GETBP; CSTI 0;
   ADD; LDI; LT; IFNZRO "L2"; INCSP -1; RET 0]

> compile "ex3";;
val it : Machine.instr list =
  [LDARGS; CALL (1, "L1"); STOP; Label "L1"; INCSP 1; GETBP; CSTI 1; ADD;
   CSTI 0; STI; INCSP -1; GOTO "L3"; Label "L2"; GETBP; CSTI 1; ADD; LDI;
   PRINTI; INCSP -1; GETBP; CSTI 1; ADD; GETBP; CSTI 1; ADD; LDI; CSTI 1; ADD;
   STI; INCSP -1; INCSP 0; Label "L3"; GETBP; CSTI 1; ADD; LDI; GETBP; CSTI 0;
   ADD; LDI; LT; IFNZRO "L2"; INCSP -1; RET 0]

> #q;;
$ java Machine ex3.out 8
0 1 2 3 4 5 6 7
Ran 0.02 seconds
```

Output from running `ex5.c`:

```sh
> open ParseAndComp;;
> compileToFile (fromFile "ex5.c") "ex5.out";;
val it : Machine.instr list =
  [LDARGS; CALL (1, "L1"); STOP; Label "L1"; INCSP 1; GETBP; CSTI 1; ADD;
   GETBP; CSTI 0; ADD; LDI; STI; INCSP -1; INCSP 1; GETBP; CSTI 0; ADD; LDI;
   GETBP; CSTI 2; ADD; CALL (2, "L2"); INCSP -1; GETBP; CSTI 2; ADD; LDI;
   PRINTI; INCSP -1; INCSP -1; GETBP; CSTI 1; ADD; LDI; PRINTI; INCSP -1;
   INCSP -1; RET 0; Label "L2"; GETBP; CSTI 1; ADD; LDI; GETBP; CSTI 0; ADD;
   LDI; GETBP; CSTI 0; ADD; LDI; MUL; STI; INCSP -1; INCSP 0; RET 1]

> compile "ex5";;
val it : Machine.instr list =
  [LDARGS; CALL (1, "L1"); STOP; Label "L1"; INCSP 1; GETBP; CSTI 1; ADD;
   GETBP; CSTI 0; ADD; LDI; STI; INCSP -1; INCSP 1; GETBP; CSTI 0; ADD; LDI;
   GETBP; CSTI 2; ADD; CALL (2, "L2"); INCSP -1; GETBP; CSTI 2; ADD; LDI;
   PRINTI; INCSP -1; INCSP -1; GETBP; CSTI 1; ADD; LDI; PRINTI; INCSP -1;
   INCSP -1; RET 0; Label "L2"; GETBP; CSTI 1; ADD; LDI; GETBP; CSTI 0; ADD;
   LDI; GETBP; CSTI 0; ADD; LDI; MUL; STI; INCSP -1; INCSP 0; RET 1]

> #q;;
$ java Machine ex5.out 8
64 8
Ran 0.02 seconds
```

#### Symbolic bytecode for `ex3.c`

```fs
[LDARGS; CALL (1, "L1"); STOP;                // main(args[1])
Label "L1"; INCSP 1; GETBP; CSTI 1; ADD;      // int i;
            CSTI 0; STI;                      // i=0;
            INCSP -1; GOTO "L3";              // while (i < n)
Label "L2"; GETBP; CSTI 1; ADD; LDI; PRINTI;  // print i;
            INCSP -1; GETBP; CSTI 1; ADD;     // *Gets address of i in* i=i+1;
            GETBP; CSTI 1; ADD; LDI;          // *Reads value of i in* i=i+1;
            CSTI 1; ADD; STI;                 // *Writes new value of i in* i=i+1;
            INCSP -1;                         // Throws value away due to semicolon in `i=i+1`
            INCSP 0;                          // Exits block
Label "L3"; GETBP; CSTI 1; ADD; LDI;          // i
            GETBP; CSTI 0; ADD; LDI;          // n
            LT;                               // i < n
            IFNZRO "L2";                      // { while-body }
            INCSP -1; RET 0]                  // *function cleanup*
```


#### Symbolic bytecode for `ex5.c`


```fs
[LDARGS; CALL (1, "L1"); STOP;                          // main(args[1])
Label "L1"; INCSP 1; GETBP; CSTI 1; ADD;                // int r;
   GETBP; CSTI 0; ADD; LDI; STI; INCSP -1;              // r = n;
           INCSP 1; GETBP; CSTI 0; ADD; LDI;                    // int r;
           GETBP; CSTI 2; ADD; CALL (2, "L2"); INCSP -1;        // square(n, &r);
           GETBP; CSTI 2; ADD; LDI; PRINTI; INCSP -1; INCSP -1; // print r;
   GETBP; CSTI 1; ADD; LDI; PRINTI; INCSP -1;           // print r;
   INCSP -1; RET 0;                                     // *Program termination*
Label "L2"; GETBP; CSTI 1; ADD; LDI;                    // int *rp
            GETBP; CSTI 0; ADD; LDI;                    // i
            GETBP; CSTI 0; ADD; LDI;                    // i
            MUL; STI; INCSP -1;                         // *rp = i * i;
            INCSP 0; RET 1]                             // *function cleanup*
```



#### Tracing the execution of `ex3.c`

We've done this with the symbolic bytecode above and we felt that was sufficient. We will skip this part of the exercise.

### Exercise 8.3

We added the following case in the compiler:

```diff
diff --git a/assignment7/MicroC/Comp.fs b/assignment7/MicroC/Comp.fs
index 92bff0d..0c172e1 100644
--- a/assignment7/MicroC/Comp.fs
+++ b/assignment7/MicroC/Comp.fs
@@ -168,6 +168,8 @@ and cExpr (e : expr) (varEnv : varEnv) (funEnv : funEnv) : instr list =
     | Assign(acc, e) -> cAccess acc varEnv funEnv @ cExpr e varEnv funEnv @ [STI]
     | CstI i         -> [CSTI i]
     | Addr acc       -> cAccess acc varEnv funEnv
+    | PreInc acc     -> cAccess acc varEnv funEnv @ [DUP; LDI; CSTI 1; ADD; STI]
+    | PreDec acc     -> cAccess acc varEnv funEnv @ [DUP; LDI; CSTI 1; SUB; STI]
     | Prim1(ope, e1) ->
       cExpr e1 varEnv funEnv
       @ (match ope with
```

To test our changes we wrote the following program:


```c
void main(int n) {
  print n;
  ++n;
  print n;
  print ++n;
  println;
}
```

then compiled and ran it:


```fsi
> open ParseAndComp;;
> compileToFile (fromFile "ex8.3.c") "ex8.3.out";;
val it : Machine.instr list =
  [LDARGS; CALL (1, "L1"); STOP; Label "L1"; GETBP; CSTI 0; ADD; LDI; PRINTI;
   INCSP -1; GETBP; CSTI 0; ADD; DUP; LDI; CSTI 1; ADD; STI; INCSP -1; GETBP;
   CSTI 0; ADD; LDI; PRINTI; INCSP -1; GETBP; CSTI 0; ADD; DUP; LDI; CSTI 1;
   ADD; STI; PRINTI; INCSP -1; CSTI 10; PRINTC; INCSP -1; INCSP 0; RET 0]

> compile "ex8.3";;
val it : Machine.instr list =
  [LDARGS; CALL (1, "L1"); STOP; Label "L1"; GETBP; CSTI 0; ADD; LDI; PRINTI;
   INCSP -1; GETBP; CSTI 0; ADD; DUP; LDI; CSTI 1; ADD; STI; INCSP -1; GETBP;
   CSTI 0; ADD; LDI; PRINTI; INCSP -1; GETBP; CSTI 0; ADD; DUP; LDI; CSTI 1;
   ADD; STI; PRINTI; INCSP -1; CSTI 10; PRINTC; INCSP -1; INCSP 0; RET 0]

> #q;;
root@361dfdd0c167:/work# java Machine ex8.3.out 0
0 1 2

Ran 0.021 seconds
```


### Exercise 8.4


#### `ex8.c`

Comparison of `prog1` and `ex8.out`


Symbolic bytecode:
```fs
[LDARGS; CALL (0, "L1"); STOP; 
Label "L1";                               // main()
INCSP 1; GETBP; CSTI 0; ADD;              // int i;
   CSTI 20000000; STI; INCSP -1;          // i = 20_000_000
   GOTO "L3"; 
Label "L2"; 
   GETBP; CSTI 0; ADD;                    // loads address of i
   GETBP; CSTI 0; ADD; LDI;               // loads value of i
   CSTI 1; SUB; STI; INCSP -1;            // i = i - 1;
   INCSP 0;                               // block ends
Label "L3"; 
   GETBP; CSTI 0; ADD; LDI; IFNZRO "L2";  // while(i)
   INCSP -1; RET -1]                      // program ends
```


Bytecode for `prog1`:
0 20000000 16 7 0 1 2 9 18 4 25

As labels, with program code locations:

[CSTI 20_000_000;
GOTO 7; 
CSTI 1; // PC=4
SUB;
DUP;
IFNZERO 4; // PC=7
STOP]


Immediately from the bytecode, we see that there is a difference in size.
Furthermore, the bytecode for `ex8.out` has a lot of overhead in terms of keeping track of the variable `i`. Between each iteration it needs to find its location in memory and read its value, which costs a lot of time, since its location stays the same, in memory, during the whole execution.

The abstraction with using a while loop is also costly, since overhead is added to check the predicate for the while loop, as opposed to a single bytecode instruction (`IFNZERO 4`) for the program `prog1`.

The `ex8.out` program also does a lot of redundant computation such as `CSTI 0; ADD`.

#### `ex13.c`
