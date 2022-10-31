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

```txt
0 20000000 16 7 0 1 2 9 18 4 25
```

As labels, with program code locations:

```fs
[CSTI 20_000_000;
GOTO 7; 
CSTI 1;       // PC=4
SUB;
DUP;
IFNZERO 4;    // PC=7
STOP]
```


Immediately from the bytecode, we see that there is a difference in size.
Furthermore, the bytecode for `ex8.out` has a lot of overhead in terms of keeping track of the variable `i`. Between each iteration it needs to find its location in memory and read its value, which costs a lot of time, since its location stays the same, in memory, during the whole execution.

The abstraction with using a while loop is also costly, since overhead is added to check the predicate for the while loop, as opposed to a single bytecode instruction (`IFNZERO 4`) for the program `prog1`.

The `ex8.out` program also does a lot of redundant computation such as `CSTI 0; ADD`.

#### `ex13.c`

The symbolic bytecode from `ex13.c` looks like this:

```fs
[LDARGS; CALL (1, "L1"); STOP; 
Label "L1"; INCSP 1; GETBP; CSTI 1; ADD; CSTI 1889; STI; INCSP -1; GOTO "L3"; // main(n)
Label "L2"; GETBP; CSTI 1; ADD; GETBP;
   CSTI 1; ADD; LDI; CSTI 1; ADD; STI; INCSP -1; // y = y + 1;
   GETBP; CSTI 1; ADD; LDI; // loads y from memory
   CSTI 4; MOD; CSTI 0; EQ; IFZERO "L7"; // left side of `&&`: `y % 4 == 0`
   GETBP; CSTI 1; ADD; LDI; CSTI 100;
   MOD; CSTI 0; EQ; NOT; IFNZRO "L9"; GETBP; CSTI 1; ADD; LDI; CSTI 400; MOD;
   CSTI 0; EQ; GOTO "L8"; 
Label "L9"; CSTI 1; 
Label "L8"; GOTO "L6";
Label "L7"; CSTI 0; 
Label "L6"; IFZERO "L4"; GETBP; CSTI 1; ADD; LDI;
   PRINTI; INCSP -1; GOTO "L5"; 
Label "L4"; INCSP 0; 
Label "L5"; INCSP 0;
Label "L3"; GETBP; CSTI 1; ADD; LDI; GETBP; CSTI 0; ADD; LDI; LT;
   IFNZRO "L2"; INCSP -1; RET 0]
```

With this interaction, we take note of:

- Conditionals use labels to go through the program, since execution can be split up into many possible paths.
- Conditionals can leverage labels and a "top-to-botttom" execution to either skip an intermediate label (L8, skipping L7), or make sure that constants are put unto the stack such that `if` statements execute as intended.


## Exercise 8.5

We made the following changes to the abstract syntax, lexer, parser and compiler:

```diff
diff --git a/assignment7/MicroC/Absyn.fs b/assignment7/MicroC/Absyn.fs
index fd965be..00286eb 100644
--- a/assignment7/MicroC/Absyn.fs
+++ b/assignment7/MicroC/Absyn.fs
@@ -25,6 +25,7 @@ and expr =
   | Andalso of expr * expr           (* Sequential and              *)
   | Orelse of expr * expr            (* Sequential or               *)
   | Call of string * expr list       (* Function call f(...)        *)
+  | TernaryOp of expr * expr * expr  (* e1 ? e2 : e3                *)
                                                                    
 and access =                                                       
   | AccVar of string                 (* Variable access        x    *) 
diff --git a/assignment7/MicroC/CLex.fsl b/assignment7/MicroC/CLex.fsl
index 3a1eff0..7427390 100644
--- a/assignment7/MicroC/CLex.fsl
+++ b/assignment7/MicroC/CLex.fsl
@@ -68,6 +68,8 @@ rule Token = parse
   | "&&"            { SEQAND }                     
   | "&"             { AMP }                     
   | "!"             { NOT }                     
+  | "?"             { QUESTIONMARK }
+  | ":"             { COLON }
   | '('             { LPAR }
   | ')'             { RPAR }
   | '{'             { LBRACE }
diff --git a/assignment7/MicroC/CPar.fsy b/assignment7/MicroC/CPar.fsy
index 953d778..a373495 100644
--- a/assignment7/MicroC/CPar.fsy
+++ b/assignment7/MicroC/CPar.fsy
@@ -18,11 +18,13 @@ let nl = CstI 10
 %token PLUS MINUS TIMES DIV MOD
 %token EQ NE GT LT GE LE
 %token NOT SEQOR SEQAND
+%token COLON QUESTIONMARK
 %token LPAR RPAR LBRACE RBRACE LBRACK RBRACK SEMI COMMA ASSIGN AMP INC DEC
 %token EOF
 
 %right ASSIGN             /* lowest precedence */
 %nonassoc PRINT
+%left QUESTIONMARK COLON
 %left SEQOR
 %left SEQAND
 %left EQ NE 
@@ -136,6 +138,7 @@ ExprNotAccess:
   | Expr LE    Expr                     { Prim2("<=", $1, $3) }
   | Expr SEQAND Expr                    { Andalso($1, $3)     }
   | Expr SEQOR  Expr                    { Orelse($1, $3)      }
+  | Expr QUESTIONMARK Expr COLON Expr   { TernaryOp($1, $3, $5) }
 ;
 
 AtExprNotAccess:
diff --git a/assignment7/MicroC/Comp.fs b/assignment7/MicroC/Comp.fs
index 0c172e1..b91eef5 100644
--- a/assignment7/MicroC/Comp.fs
+++ b/assignment7/MicroC/Comp.fs
@@ -208,6 +208,14 @@ and cExpr (e : expr) (varEnv : varEnv) (funEnv : funEnv) : instr list =
       @ cExpr e2 varEnv funEnv
       @ [GOTO labend; Label labtrue; CSTI 1; Label labend]
     | Call(f, es) -> callfun f es varEnv funEnv
+    | TernaryOp(e1, e2, e3) ->
+        let labelFalse  = newLabel()
+        let labelEnd = newLabel()
+
+        cExpr e1 varEnv funEnv    @ [IFZERO labelFalse]
+        @ cExpr e2 varEnv funEnv  @ [GOTO labelEnd]
+        @ [Label labelFalse]      @ cExpr e3 varEnv funEnv
+        @ [Label labelEnd]
```

We wrote a Micro-C program to test the ternary operator:

```c
void main() {
	int n;
	n = 1 < 0 ? 10 : 20;
	print n;
	println;
}
```

Which compiled to the following symbolic bytecode and terminated with the given output:

```fsi
> open ParseAndComp;;
> compile "ex8.5";;
val it : Machine.instr list =
  [LDARGS; CALL (0, "L1"); STOP; Label "L1"; INCSP 1; GETBP; CSTI 0; ADD;
   CSTI 1; CSTI 0; LT; IFZERO "L2"; CSTI 10; GOTO "L3"; Label "L2"; CSTI 20;
   Label "L3"; STI; INCSP -1; GETBP; CSTI 0; ADD; LDI; PRINTI; INCSP -1;
   CSTI 10; PRINTC; INCSP -1; INCSP -1; RET -1]
```

```sh
$ java Machine ex8.5.out
20

Ran 0.022 seconds
```

## Exercise 8.6

We added the following code:

```diff
diff --git a/assignment7/MicroC/Absyn.fs b/assignment7/MicroC/Absyn.fs
index 00286eb..8dc9da2 100644
--- a/assignment7/MicroC/Absyn.fs
+++ b/assignment7/MicroC/Absyn.fs
@@ -38,6 +38,7 @@ and stmt =
   | Expr of expr                     (* Expression statement   e;   *)
   | Return of expr option            (* Return from method          *)
   | Block of stmtordec list          (* Block: grouping and scope   *)
+  | Switch of expr * (int * stmt) list (* :) *)
                                                                    
 and stmtordec =                                                    
   | Dec of typ * string              (* Local variable declaration  *)
diff --git a/assignment7/MicroC/CLex.fsl b/assignment7/MicroC/CLex.fsl
index 7427390..8b0aaaf 100644
--- a/assignment7/MicroC/CLex.fsl
+++ b/assignment7/MicroC/CLex.fsl
@@ -28,6 +28,8 @@ let keyword s =
     | "true"    -> CSTBOOL 1
     | "void"    -> VOID 
     | "while"   -> WHILE         
+    | "switch"  -> SWITCH
+    | "case"    -> CASE
     | _         -> NAME s
  
 let cEscape s = 
diff --git a/assignment7/MicroC/CPar.fsy b/assignment7/MicroC/CPar.fsy
index a373495..e67b38e 100644
--- a/assignment7/MicroC/CPar.fsy
+++ b/assignment7/MicroC/CPar.fsy
@@ -19,6 +19,7 @@ let nl = CstI 10
 %token EQ NE GT LT GE LE
 %token NOT SEQOR SEQAND
 %token COLON QUESTIONMARK
+%token SWITCH CASE
 %token LPAR RPAR LBRACE RBRACE LBRACK RBRACK SEMI COMMA ASSIGN AMP INC DEC
 %token EOF
 
@@ -91,6 +92,9 @@ StmtOrDecSeq:
   | Vardec SEMI StmtOrDecSeq            { Dec (fst $1, snd $1) :: $3 }
 ;
 
+
+
+
 Stmt: 
     StmtM                               { $1 }
   | StmtU                               { $1 }
@@ -109,6 +113,12 @@ StmtU:
     IF LPAR Expr RPAR StmtM ELSE StmtU  { If($3, $5, $7)       }
   | IF LPAR Expr RPAR Stmt              { If($3, $5, Block []) }
   | WHILE LPAR Expr RPAR StmtU          { While($3, $5)        }
+  | SWITCH LPAR Expr RPAR LBRACE Cases RBRACE { Switch($3, $6) }
+;
+
+Cases:
+   /* empty */                          { [] } 
+  | CASE CSTINT COLON Block Cases       { ($2, $4) :: $5 }
 ;
 
 Expr: 
diff --git a/assignment7/MicroC/Comp.fs b/assignment7/MicroC/Comp.fs
index b91eef5..62726e9 100644
--- a/assignment7/MicroC/Comp.fs
+++ b/assignment7/MicroC/Comp.fs
@@ -144,6 +144,15 @@ let rec cStmt stmt (varEnv : varEnv) (funEnv : funEnv) : instr list =
       [RET (snd varEnv - 1)]
     | Return (Some e) -> 
       cExpr e varEnv funEnv @ [RET (snd varEnv)]
+    | Switch(e, cases) ->
+      let casesAndLabels = List.map (fun c -> (newLabel(), c) ) cases
+      let endLabel = newLabel()
+
+      cExpr e varEnv funEnv
+      @ List.fold (fun acc (label, (i, block)) -> acc @ [DUP; CSTI i; EQ; NOT; IFZERO label] ) [] casesAndLabels
+      @ List.fold (fun acc (label, (i, block)) -> acc @ [Label label] @ cStmt block varEnv funEnv @ [GOTO endLabel]) [] casesAndLabels
+      @ [Label endLabel; INCSP -1]
+      
```

We compile the program:

```fs
> open ParseAndComp;;
> compile "ex8.6";;
val it : Machine.instr list =
  [LDARGS; CALL (1, "L1"); STOP; Label "L1"; GETBP; CSTI 0; ADD; LDI; DUP;
   CSTI 3; EQ; NOT; IFZERO "L2"; DUP; CSTI 2; EQ; NOT; IFZERO "L3"; DUP;
   CSTI 1; EQ; NOT; IFZERO "L4"; Label "L2"; CSTI 30; PRINTI; INCSP -1;
   INCSP 0; GOTO "L5"; Label "L3"; CSTI 20; PRINTI; INCSP -1; INCSP 0;
   GOTO "L5"; Label "L4"; CSTI 10; PRINTI; INCSP -1; INCSP 0; GOTO "L5";
   Label "L5"; INCSP -1; CSTI 10; PRINTC; INCSP -1; INCSP 0; RET 0]

> #q;;
```

And run it:

```sh
$ java Machinetrace ex8.6.out 2
[ ]{0: LDARGS}
[ 2 ]{1: CALL 1 5}
[ 4 -999 2 ]{5: GETBP}
[ 4 -999 2 2 ]{6: CSTI 0}
[ 4 -999 2 2 0 ]{8: ADD}
[ 4 -999 2 2 ]{9: LDI}
[ 4 -999 2 2 ]{10: DUP}
[ 4 -999 2 2 2 ]{11: CSTI 3}
[ 4 -999 2 2 2 3 ]{13: EQ}
[ 4 -999 2 2 0 ]{14: NOT}
[ 4 -999 2 2 1 ]{15: IFZERO 31}
[ 4 -999 2 2 ]{17: DUP}
[ 4 -999 2 2 2 ]{18: CSTI 2}
[ 4 -999 2 2 2 2 ]{20: EQ}
[ 4 -999 2 2 1 ]{21: NOT}
[ 4 -999 2 2 0 ]{22: IFZERO 40}
[ 4 -999 2 2 ]{40: CSTI 20}
[ 4 -999 2 2 20 ]{42: PRINTI}
20 [ 4 -999 2 2 20 ]{43: INCSP -1}
[ 4 -999 2 2 ]{45: INCSP 0}
[ 4 -999 2 2 ]{47: GOTO 58}
[ 4 -999 2 2 ]{58: INCSP -1}        # <--- we refer to here
[ 4 -999 2 ]{60: CSTI 10}
[ 4 -999 2 10 ]{62: PRINTC}

[ 4 -999 2 10 ]{63: INCSP -1}
[ 4 -999 2 ]{65: INCSP 0}
[ 4 -999 2 ]{67: RET 0}
[ 2 ]{4: STOP}

Ran 0.065 seconds
$ java Machine ex8.6.out 2
20

Ran 0.019 seconds
```

One noteworthy issue we encountered is that we need to clean up after ourselves if we allocate things on the stack. Initially, we had our last instruction as `Label endLabel`, but since we copy the value of `n` onto the stack, it would remain on the stack after our switch case, completely breaking the return statement. Thus we had to add `INCSP -1` at the end to decrement the stack pointer and deallocate our copy of `n`. You can see this in action by the comment in the above code block.
