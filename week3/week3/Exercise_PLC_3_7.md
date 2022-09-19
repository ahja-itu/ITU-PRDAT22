# PLC 3.7

## Solution

Note, we chose to aim for the following precedence:

```sh
if x then y else z + if g then h else j
# should yield
if x then y else (z + if g then h else j)
# and not
(if x then y else z) + (if g then h else j)
```

You can find our code by searching for `Exercise 3.7` comments, which should reveal changes in `Absyn.fs`, `ExprLex.fsl`, and `ExprPar.fsy`.

Alternatively, see the git diff here:

```diff
diff --git a/week3/week3/Expr/Absyn.fs b/week3/week3/Expr/Absyn.fs
index d734237..7f55508 100644
--- a/week3/week3/Expr/Absyn.fs
+++ b/week3/week3/Expr/Absyn.fs
@@ -9,3 +9,6 @@ type expr =
   | Var of string
   | Let of string * expr * expr
   | Prim of string * expr * expr
+  (* Exercise 3.7 - start block *)
+  | If of expr * expr * expr
+  (* Exercise 3.7 - end block *)
diff --git a/week3/week3/Expr/ExprLex.fsl b/week3/week3/Expr/ExprLex.fsl
index 5d62327..2d0f197 100644
--- a/week3/week3/Expr/ExprLex.fsl
+++ b/week3/week3/Expr/ExprLex.fsl
@@ -19,6 +19,11 @@ let keyword s =
     | "let" -> LET
     | "in"  -> IN
     | "end" -> END
+    (* Exercise 3.7 - start block *)
+    | "if"  -> IF
+    | "then"  -> THEN
+    | "else"  -> ELSE
+    (* Exercise 3.7 - end block *)
     | _     -> NAME s
 }
 
diff --git a/week3/week3/Expr/ExprPar.fsy b/week3/week3/Expr/ExprPar.fsy
index cea96c5..c2ba827 100644
--- a/week3/week3/Expr/ExprPar.fsy
+++ b/week3/week3/Expr/ExprPar.fsy
@@ -9,10 +9,16 @@
 %token <int> CSTINT
 %token <string> NAME
 %token PLUS MINUS TIMES DIVIDE EQ
+/* Exercise 3.7 - start block */
+%token ELSE THEN IF
+/* Exercise 3.7 - end block */
 %token END IN LET
 %token LPAR RPAR
 %token EOF
 
+/* Exercise 3.7 - start block */
+%right IF THEN ELSE
+/* Exercise 3.7 - end block */
 %left MINUS PLUS        /* lowest precedence  */
 %left TIMES             /* highest precedence */
 
@@ -30,6 +36,9 @@ Expr:
   | CSTINT                              { CstI $1           }
   | MINUS CSTINT                        { CstI (- $2)       }
   | LPAR Expr RPAR                      { $2                }
+  /* Exercise 3.7 - start block */
+  | IF Expr THEN Expr ELSE Expr         { If($2, $4, $6)    }
+  /* Exercise 3.7 - end block */
   | LET NAME EQ Expr IN Expr END        { Let($2, $4, $6)   }
   | Expr TIMES Expr                     { Prim("*", $1, $3) }
   | Expr PLUS  Expr                     { Prim("+", $1, $3) }  
```

## Outputs and examples

```sh
$ ~/bin/fsharp/fsyacc --module ExprPar ExprPar.fsy
$ dotnet fsi -r FsLexYacc.Runtime.dll Absyn.fs ExprPar.fs ExprLex.fs Parse.fs 

# tons of output.. and then you're in interactive F# mode
```

Below are some example runs. In our last example, you can see that we achieved our goal precedence.

```fs
> open Parse;;
> fromString "if x then y else z + if g then h else j";;
val it: Absyn.expr =
  If (Var "x", Var "y", Prim ("+", Var "z", If (Var "g", Var "h", Var "j")))

> fromString "if 2 then 4 else 5";;
val it: Absyn.expr = If (CstI 2, CstI 4, CstI 5)

> fromString "let x = if 2 then 4 else 5 in 10 + x end";;
val it: Absyn.expr =
  Let ("x", If (CstI 2, CstI 4, CstI 5), Prim ("+", CstI 10, Var "x"))

> fromString "if 1 then 2 else 3 if 4 then 5 else 6";;
System.Exception: parse error near line 1, column 21

   at Microsoft.FSharp.Core.PrintfModule.PrintFormatToStringThenFail@1439.Invoke(String message) in /private/tmp/d20220425-7054-1ivq06p/src/fsharp.6d626ff0752a
77d339f609b4d361787dc9ca93a5/artifacts/source-build/self/src/src/fsharp/FSharp.Core/printf.fs:line 1439
   at FSI_0002.Parse.fromString(String str)
   at <StartupCode$FSI_0007>.$FSI_0007.main@()
Stopped due to error
> fromString "if 1 then 2 else if 3 then 4 else 5";;
val it: Absyn.expr = If (CstI 1, CstI 2, If (CstI 3, CstI 4, CstI 5))

> fromString "if x then y else z + if g then h else j";;
val it: Absyn.expr =
  If (Var "x", Var "y", Prim ("+", Var "z", If (Var "g", Var "h", Var "j")))
```
