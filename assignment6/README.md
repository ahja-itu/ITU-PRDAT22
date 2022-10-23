# Assignment 5

Authors: adbo, aljb and ahja

This code can also be found online at <https://github.com/andreaswachs/ITU-PRDAT22>.

We recommend reading this Markdown page on [GitHub](https://github.com/andreaswachs/ITU-PRDAT22/blob/main/assignment6/README.md) or VSCode because it makes the diffs easier to read.

## Solutions

### PLC 7.1

The programs `ex1.c` and  `ex11.c` have been parsed and run as follows:

```fsi
$ fsharpi -r FsLexYacc.Runtime.dll Absyn.fs CPar.fs CLex.fs Parse.fs Interp.fs ParseAndRun.fs

> open ParseAndRun;;

> fromFile "ex1.c";;
val it : Absyn.program =
  Prog
    [Fundec
       (None, "main", [(TypI, "n")],
        Block
          [Stmt
             (While
                (Prim2 (">", Access (AccVar "n"), CstI 0),
                 Block
                   [Stmt (Expr (Prim1 ("printi", Access (AccVar "n"))));
                    Stmt
                      (Expr
                         (Assign
                            (AccVar "n",
                             Prim2 ("-", Access (AccVar "n"), CstI 1))))]));
           Stmt (Expr (Prim1 ("printc", CstI 10)))])]

> run (fromFile "ex1.c") [10];;
10 9 8 7 6 5 4 3 2 1
val it : Interp.store = map [(0, 0)]

> run (fromFile "ex11.c") [5];;
1 3 5 2 4
1 4 2 5 3
2 4 1 3 5
2 5 3 1 4
3 1 4 2 5
3 5 2 4 1
4 1 3 5 2
4 2 5 3 1
5 2 4 1 3
5 3 1 4 2
val it : Interp.store =
  map
    [(0, 5); (1, 0); (2, 6); (3, -999); (4, 0); (5, 0); (6, 0); (7, 0); (8, 0);
     ...]
```

### PLC 7.2

### i

The Micro-C program itself:

```c
void arrsum(int n, int arr[], int *sump) {
  int i;
  i = 0;
  int sum;
  sum = 0;

  while (i < n) {
    sum = sum + arr[i];
    i = i + 1;
  }

  *sump = sum;
}

void main(int n) {
  int arr[4];
  arr[0] = 7;
  arr[1] = 13;
  arr[2] = 9;
  arr[3] = 8;

  int *sump;

  arrsum(n, arr, sump);

  print *sump;
  println;
}
```

Executing the program:

```fsi
> run (fromFile "../ex_7_2_i.c") [2];;
20 val it : Interp.store =
  map
    [(-1, 20); (0, 2); (1, 7); (2, 13); (3, 9); (4, 8); (5, 1); (6, -1);
     (7, 2); ...]

> run (fromFile "../ex_7_2_i.c") [5];;
38
val it : Interp.store =
  map
    [(-1, 38); (0, 5); (1, 7); (2, 13); (3, 9); (4, 8); (5, 1); (6, -1);
     (7, 5); ...]

> run (fromFile "../ex_7_2_i.c") [4];;
37
val it : Interp.store =
  map
    [(-1, 37); (0, 4); (1, 7); (2, 13); (3, 9); (4, 8); (5, 1); (6, -1);
     (7, 4); ...]
```

The middle execution with the input `5` shows how wildly unsafe Micro-C is since we can easily just read the memory outside array bounds. In this case it also reads the address of the array.

#### ii

The Micro-C program:

```c
void squares(int n, int arr[]) {
  int i;
  i = 0;

  while (i < n) {
    arr[i] = i * i;
    print arr[i];
    i = i + 1;
  }
}

void main(int n) {
  int arr[20];

  squares(n, arr);

  println;
}
```

And the result of running it:

```fsi
> run (fromFile "../ex_7_2_ii.cnotc") [4];;
0 1 4 9
val it : Interp.store =
  map
    [(0, 4); (1, 0); (2, 1); (3, 4); (4, 9); (5, -999); (6, -999); (7, -999);
     (8, -999); ...]

> run (fromFile "../ex_7_2_ii.cnotc") [21];;
0 1 4 9 16 25 36 49 64 81 100 121 144 169 196 225 256 289 324 361 400
val it : Interp.store =
  map
    [(0, 21); (1, 0); (2, 1); (3, 4); (4, 9); (5, 16); (6, 25); (7, 36);
     (8, 49); ...]

> run (fromFile "../ex_7_2_ii.cnotc") [25];;
0 1 4 9 16 25 36 49 64 81 100 121 144 169 196 225 256 289 324 361 400 441 System.Collections.Generic.KeyNotFoundException: The given key was not present in the dictionary.
```

The first run shows a perfectly fine result. The second run shows that we can write outside the array. The third run shows the same but with a boom boom as a result of our bad behaviour.

#### iii

The program:

```c
void histogram(int n, int ns[], int max, int freq[]) {
  int i;
  i = 0;

  while (i < max) {
    freq[i] = 0;
    i = i + 1;
  }

  i = 0;
  while (i < n) {
    freq[ns[i]] = freq[ns[i]] + 1;
    i = i + 1;
  }
}

void main(int n) {
  int ns[7];
  ns[0] = 7;
  ns[1] = 13;
  ns[2] = 9;
  ns[3] = 8;
  ns[4] = 7;
  ns[5] = 9;
  ns[6] = 9;

  int freq[20];
  histogram(n, ns, 20, freq);

  int i;
  i = 0;
  while (i < 20) {
    print freq[i];
    i = i + 1;
  }

  println;
}
```

And here we run the program:

```fsi
> run (fromFile "../ex_7_2_iii.cnotc") [7];;
0 0 0 0 0 0 0 2 1 3 0 0 0 1 0 0 0 0 0 0
val it : Interp.store =
  map
    [(0, 7); (1, 7); (2, 13); (3, 9); (4, 8); (5, 7); (6, 9); (7, 9); (8, 1);
     ...]

> run (fromFile "../ex_7_2_iii.cnotc") [10];;
1 2 0 0 0 0 0 2 1 3 0 0 0 1 0 0 0 0 0 0
val it : Interp.store =
  map
    [(0, 10); (1, 7); (2, 13); (3, 9); (4, 8); (5, 7); (6, 9); (7, 9); (8, 1);
     ...]
```

The first run is perfectly normal and counts correctly. The second run, however, reads outside of array bounds and counts those numbers too - some very undefined behaviour / buffer overflow.

### PLC 7.3

We have changed the lexer and parser as follows ("aliasing" a for loop as a while loop):

```diff
diff --git a/assignment6/MicroC/CLex.fsl b/assignment6/MicroC/CLex.fsl
index 13d2450..69d24db 100644
--- a/assignment6/MicroC/CLex.fsl
+++ b/assignment6/MicroC/CLex.fsl
@@ -28,6 +28,7 @@ let keyword s =
     | "true"    -> CSTBOOL 1
     | "void"    -> VOID 
     | "while"   -> WHILE         
+    | "for"     -> FOR
     | _         -> NAME s
  
 let cEscape s = 
diff --git a/assignment6/MicroC/CPar.fsy b/assignment6/MicroC/CPar.fsy
index a1b2075..05c8660 100644
--- a/assignment6/MicroC/CPar.fsy
+++ b/assignment6/MicroC/CPar.fsy
@@ -14,7 +14,7 @@ let nl = CstI 10
 %token <int> CSTINT CSTBOOL
 %token <string> CSTSTRING NAME
 
-%token CHAR ELSE IF INT NULL PRINT PRINTLN RETURN VOID WHILE
+%token CHAR ELSE IF INT NULL PRINT PRINTLN RETURN VOID WHILE FOR
 %token PLUS MINUS TIMES DIV MOD
 %token EQ NE GT LT GE LE
 %token NOT SEQOR SEQAND
@@ -100,12 +100,30 @@ StmtM:  /* No unbalanced if-else */
   | Block                               { $1                   }
   | IF LPAR Expr RPAR StmtM ELSE StmtM  { If($3, $5, $7)       }
   | WHILE LPAR Expr RPAR StmtM          { While($3, $5)        }
+  | FOR LPAR Expr SEMI Expr SEMI Expr RPAR StmtM {
+    Block [
+      Stmt (Expr $3);
+      Stmt (While($5, Block [
+        Stmt $9;
+        Stmt (Expr $7)
+      ]))
+    ]
+  }
 ;
 
 StmtU:
     IF LPAR Expr RPAR StmtM ELSE StmtU  { If($3, $5, $7)       }
   | IF LPAR Expr RPAR Stmt              { If($3, $5, Block []) }
   | WHILE LPAR Expr RPAR StmtU          { While($3, $5)        }
+  | FOR LPAR Expr SEMI Expr SEMI Expr RPAR StmtU {
+    Block [
+      Stmt (Expr $3);
+      Stmt (While($5, Block [
+        Stmt $9;
+        Stmt (Expr $7)
+      ]))
+    ]
+  }
 ;
 
 Expr: 
```

One test program:

```c
void main(int n) {
  int i;
  for (i = 0; i < n; i = i + 1)
    print i;
  println;
}
```

Running the above:

```fsi
$ make run
...

> run (fromFile "../ex_7_3_test1.cnotc") [10];;
0 1 2 3 4 5 6 7 8 9
```

Another test program:
```c
void main(int n) {
  int i;
  for (i = 1; i < n; i) {
    i = i + i;
    print i;
  }
  println;
}
```

With the output:

```fsi
> run (fromFile "../ex_7_3_test2.cnotc") [50];;
2 4 8 16 32 64
```

#### Rewrites of 7.2 assignments

We tested and ensured that these programs calculate the same results as their original counterpart, but have omitted submitting the output from the terminal.

##### i

```c
void arrsum(int n, int arr[], int *sump) {
  int i;
  int sum;
  sum = 0;

  for (i = 0; i < n; i = i + 1) {
    sum = sum + arr[i];
  }

  *sump = sum;
}

void main(int n) {
  int arr[4];
  arr[0] = 7;
  arr[1] = 13;
  arr[2] = 9;
  arr[3] = 8;

  int *sump;

  arrsum(n, arr, sump);

  print *sump;
  println;
}
```


##### ii

```c
void squares(int n, int arr[]) {
  int i;

  for (i = 0; i < n; i = i + 1) {
    arr[i] = i * i;
    print arr[i];
  }
}

void main(int n) {
  int arr[20];

  squares(n, arr);

  println;
}
```

##### iii

```c
void histogram(int n, int ns[], int max, int freq[]) {
  int i;

  for (i = 0; i < max; i = i + 1) {
    freq[i] = 0;
  }
  for (i = 0; i < n; i = i + 1) {
    freq[ns[i]] = freq[ns[i]] + 1;
  }
}

void main(int n) {
  int ns[7];
  ns[0] = 7;
  ns[1] = 13;
  ns[2] = 9;
  ns[3] = 8;
  ns[4] = 7;
  ns[5] = 9;
  ns[6] = 9;

  int freq[20];
  histogram(n, ns, 20, freq);

  int i;
  
  for (i = 0; i < 20; i = i + 1) {
    print freq[i];
  }

  println;
}
```

### PLC 7.4

Here are the changes we have made to support the inner workings of the two new operators:

```diff
diff --git a/assignment6/MicroC/Absyn.fs b/assignment6/MicroC/Absyn.fs
index da8f60c..fd965be 100644
--- a/assignment6/MicroC/Absyn.fs
+++ b/assignment6/MicroC/Absyn.fs
@@ -16,6 +16,8 @@ type typ =
 and expr =                                                         
   | Access of access                 (* x    or  *p    or  a[e]     *)
   | Assign of access * expr          (* x=e  or  *p=e  or  a[e]=e   *)
+  | PreInc of access                 (* ++x                         *)
+  | PreDec of access                 (* --x                         *)
   | Addr of access                   (* &x   or  &*p   or  &a[e]    *)
   | CstI of int                      (* Constant                    *)
   | Prim1 of string * expr           (* Unary primitive operator    *)
diff --git a/assignment6/MicroC/Interp.fs b/assignment6/MicroC/Interp.fs
index 940bb9a..6d28e58 100644
--- a/assignment6/MicroC/Interp.fs
+++ b/assignment6/MicroC/Interp.fs
@@ -158,6 +158,12 @@ and eval e locEnv gloEnv store : int * store =
     | Assign(acc, e) -> let (loc, store1) = access acc locEnv gloEnv store
                         let (res, store2) = eval e locEnv gloEnv store1
                         (res, setSto store2 loc res) 
+    | PreInc acc     -> let (loc, store1) = access acc locEnv gloEnv store
+                        let res = (getSto store1 loc) + 1
+                        (res, setSto store1 loc res)
+    | PreDec acc     -> let (loc, store1) = access acc locEnv gloEnv store
+                        let res = (getSto store1 loc) - 1
+                        (res, setSto store1 loc res)
     | CstI i         -> (i, store)
     | Addr acc       -> access acc locEnv gloEnv store
     | Prim1(ope, e1) ->
```

We will test that this works when we have extended the parser in the next section.

### PLC 7.5

Here are the changes we have made to the lexer and parser to support the two new operators:

```diff
diff --git a/assignment6/MicroC/CLex.fsl b/assignment6/MicroC/CLex.fsl
index 69d24db..2b30cc4 100644
--- a/assignment6/MicroC/CLex.fsl
+++ b/assignment6/MicroC/CLex.fsl
@@ -53,6 +53,8 @@ rule Token = parse
                     { keyword (lexemeAsString lexbuf) }
   | '+'             { PLUS } 
   | '-'             { MINUS } 
+  | "++"            { INC }
+  | "--"            { DEC }
   | '*'             { TIMES } 
   | '/'             { DIV } 
   | '%'             { MOD }                     
diff --git a/assignment6/MicroC/CPar.fsy b/assignment6/MicroC/CPar.fsy
index 05c8660..3e7da48 100644
--- a/assignment6/MicroC/CPar.fsy
+++ b/assignment6/MicroC/CPar.fsy
@@ -18,7 +18,7 @@ let nl = CstI 10
 %token PLUS MINUS TIMES DIV MOD
 %token EQ NE GT LT GE LE
 %token NOT SEQOR SEQAND
-%token LPAR RPAR LBRACE RBRACE LBRACK RBRACK SEMI COMMA ASSIGN AMP
+%token LPAR RPAR LBRACE RBRACE LBRACK RBRACK SEMI COMMA ASSIGN AMP INC DEC
 %token EOF
 
 %right ASSIGN             /* lowest precedence */
@@ -29,6 +29,7 @@ let nl = CstI 10
 %left GT LT GE LE
 %left PLUS MINUS
 %left TIMES DIV MOD 
+%right INC DEC
 %nonassoc NOT AMP 
 %nonassoc LBRACK          /* highest precedence  */
 
@@ -134,6 +135,8 @@ Expr:
 ExprNotAccess:
     AtExprNotAccess                     { $1                  }
   | Access ASSIGN Expr                  { Assign($1, $3)      }
+  | INC Access                  				{ PreInc($2)          }
+  | DEC Access                  				{ PreDec($2)          }
   | NAME LPAR Exprs RPAR                { Call($1, $3)        }  
   | NOT Expr                            { Prim1("!", $2)      }
   | PRINT Expr                          { Prim1("printi", $2) }

```

Here are some tests to demonstrate the the parser and operators work:

```c
void main(int n) {
  // Simple loop with ++
  int i;
  for (i = 0; i < n; ++i)
    print i;
  println;

  // Simple loop with --
  i = 0;
  for (i = n; i >= 0; --i)
    print i;
  println;

  // Test that ++ and -- modify and return the result
  int x;
  int y;
  x = 0;
  y = 0;
  while (x < n || y > (0 - n)) {
    print ++x;
    print --y;
  }
  println;

  // Test that it doesn't mess with + and -
  int v;
  v = 3;
  print (5 + ++v);
  print v;
  println;
  v = 3;
  print (5 - --v);
  print v;
  println;
}
```

```fsi
> run (fromFile "../ex_7_4_test1.cnotc") [3];;
0 1 2
3 2 1 0
1 -1 2 -2 3 -3
9 4
3 2
val it : Interp.store = map [(0, 3); (1, -1); (2, 3); (3, -3); (4, 2)]
```

