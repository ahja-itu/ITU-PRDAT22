# Solutions

## PLC 4.1

"*Get archive fun.zip from the homepage and unpack to directory Fun. It contains lexer and parser specifications and interpreter for a small first- order functional language. Generate and compile the lexer and parser as described in README.TXT; parse and run some example programs with ParseAndRun.fs"*

A:
```fsi
$ fsharpi Absyn.fs Fun.fs
(...)

-    open Absyn;;
>    open Fun;;
>    let res = run (Prim("+", CstI 5, CstI 7));;
val res : int = 12

>    #q;;
$ 
```

B:

```fsi
$ fsharpi -r ~/fsharp/FsLexYacc.Runtime.dll Absyn.fs FunPar.fs FunLex.fs Parse.fs

(...)

>
- open Parse;;
>    let e1 = fromString "5+7";;
val e1 : Absyn.expr = Prim ("+", CstI 5, CstI 7)

>    let e2 = fromString "let y = 7 in y + 2 end";;
val e2 : Absyn.expr = Let ("y", CstI 7, Prim ("+", Var "y", CstI 2))

>    let e3 = fromString "let f x = x + 7 in f 2 end";;
val e3 : Absyn.expr =
  Letfun ("f", "x", Prim ("+", Var "x", CstI 7), Call (Var "f", CstI 2))

```


C:
```fsi
(...)
> open ParseAndRun;;
> run (fromString "5+7");;
val it : int = 12

>
- run (fromString "let y = 7 in y + 2 end");;
val it : int = 9

>
- run (fromString "let f x = x + 7 in f 2 end");;
val it : int = 9
```


## PLC 4.2

- Compute sum program:

```fsi
> fromString "let sum n = if n < 0 then 0 else n + (sum (n - 1)) in sum 1000 end";;
val it : Absyn.expr =
  Letfun
    ("sum", "n",
     If
       (Prim ("<", Var "n", CstI 0), CstI 0,
        Prim ("+", Var "n", Call (Var "sum", Prim ("-", Var "n", CstI 1)))),
     Call (Var "sum", CstI 1000))

> run it;;
val it : int = 500500
```

- Compute 3ˆ8

```fsi
> fromString "let threePowE n = if n < 1 then 1 else 3 * (threePowE (n - 1)) in threePowE 8 end";;
val it : Absyn.expr =
  Letfun
    ("threePowE", "n",
     If
       (Prim ("<", Var "n", CstI 1), CstI 1,
        Prim
          ("*", CstI 3, Call (Var "threePowE", Prim ("-", Var "n", CstI 1)))),
     Call (Var "threePowE", CstI 8))

> run it;;
val it : int = 6561
```

- Compute 3ˆ0 + .. 3ˆ11



```fsi
// Program:
// let threePowE e = 
//   if e < 1 then 1 else 3 * (threePowE (e - 1)) 
// in 
//   let sum n = 
//     if n < 1 then 1 else (threePowE n) + (sum (n - 1)) 
//   in 
//     sum 11 
//   end 
// end

> fromString "let threePowE e = if e < 1 then 1 else 3 * (threePowE (e - 1)) in let sum n = if n < 1 then 1 else (threePowE n) + (sum (n - 1)) in sum 11 end end
- ";;
val it : Absyn.expr =
  Letfun
    ("threePowE", "e",
     If
       (Prim ("<", Var "e", CstI 1), CstI 1,
        Prim
          ("*", CstI 3, Call (Var "threePowE", Prim ("-", Var "e", CstI 1)))),
     Letfun
       ("sum", "n",
        If
          (Prim ("<", Var "n", CstI 1), CstI 1,
           Prim
             ("+", Call (Var "threePowE", Var "n"),
              Call (Var "sum", Prim ("-", Var "n", CstI 1)))),
        Call (Var "sum", CstI 11)))

> run it;;
val it : int = 265720
```

- Compute 1^8 + .. - 10^8

```fsi
> fromString "let sum n = if n < 1 then 0 else (n * n * n * n * n * n * n * n) + (sum (n - 1)) in sum 10 end" |> run;;
val it : int = 167731333
```

## PLC 4.3

Running `git show` we can show the changes done to solve exercise 4.3:

```diff
diff --git a/week4/Fun/Absyn.fs b/week4/Fun/Absyn.fs
index b0dfc3f..c28518e 100644
--- a/week4/Fun/Absyn.fs
+++ b/week4/Fun/Absyn.fs
@@ -9,5 +9,5 @@ type expr =
   | Let of string * expr * expr
   | Prim of string * expr * expr
   | If of expr * expr * expr
-  | Letfun of string * string * expr * expr    (* (f, x, fBody, letBody) *)
-  | Call of expr * expr
+  | Letfun of string * string list * expr * expr    (* (f, x, fBody, letBody) *)
+  | Call of expr * expr list
diff --git a/week4/Fun/Fun.fs b/week4/Fun/Fun.fs
index 7847151..8b19b97 100644
--- a/week4/Fun/Fun.fs
+++ b/week4/Fun/Fun.fs
@@ -24,7 +24,7 @@ let rec lookup env x =
 
 type value = 
   | Int of int
-  | Closure of string * string * expr * value env       (* (f, x, fBody, fDeclEnv) *)
+  | Closure of string * string list * expr * value env       (* (f, xs, fBody, fDeclEnv) *)
 
 let rec eval (e : expr) (env : value env) : int =
     match e with 
@@ -52,15 +52,19 @@ let rec eval (e : expr) (env : value env) : int =
       let b = eval e1 env
       if b<>0 then eval e2 env
       else eval e3 env
-    | Letfun(f, x, fBody, letBody) -> 
-      let bodyEnv = (f, Closure(f, x, fBody, env)) :: env 
+
+    | Letfun(f, xs, fBody, letBody) ->  // x -> xs
+      let bodyEnv = (f, Closure(f, xs, fBody, env)) :: env 
       eval letBody bodyEnv
-    | Call(Var f, eArg) -> 
+    | Call(Var f, eArgs) ->  // eArg -> eArgs
       let fClosure = lookup env f
       match fClosure with
-      | Closure (f, x, fBody, fDeclEnv) ->
-        let xVal = Int(eval eArg env)
-        let fBodyEnv = (x, xVal) :: (f, fClosure) :: fDeclEnv
+      | Closure (f, xs, fBody, fDeclEnv) -> // x -> xs
+        let xVals = List.map (fun e -> Int(eval e env)) eArgs
+  
+        let fBodyEnv = (List.zip xs xVals) @ ((f, fClosure) :: fDeclEnv)
+
         eval fBody fBodyEnv
       | _ -> failwith "eval Call: not a function"
     | Call _ -> failwith "eval Call: not first-order function"
@@ -71,28 +75,29 @@ let run e = eval e [];;
 
```

Apart from the above, we've also changed the example code as seen below:

```diff
 (* Examples in abstract syntax *)
 
-let ex1 = Letfun("f1", "x", Prim("+", Var "x", CstI 1), 
-                 Call(Var "f1", CstI 12));;
+let ex1 = Letfun("f1", ["x"], Prim("+", Var "x", CstI 1), 
+                 Call(Var "f1", [CstI 12]));;
+
 
 (* Example: factorial *)
 
-let ex2 = Letfun("fac", "x",
+let ex2 = Letfun("fac", ["x"],
                  If(Prim("=", Var "x", CstI 0),
                     CstI 1,
                     Prim("*", Var "x", 
                               Call(Var "fac", 
-                                   Prim("-", Var "x", CstI 1)))),
-                 Call(Var "fac", Var "n"));;
+                                   [Prim("-", Var "x", CstI 1)]))),
+                 Call(Var "fac", [Var "n"]));;
 
 (* let fac10 = eval ex2 [("n", Int 10)];; *)
 
 (* Example: deep recursion to check for constant-space tail recursion *)
 
-let ex3 = Letfun("deep", "x", 
+let ex3 = Letfun("deep", ["x"], 
                  If(Prim("=", Var "x", CstI 0),
                     CstI 1,
-                    Call(Var "deep", Prim("-", Var "x", CstI 1))),
-                 Call(Var "deep", Var "count"));;
+                    Call(Var "deep", [Prim("-", Var "x", CstI 1)])),
+                 Call(Var "deep", [Var "count"]));;
     
 let rundeep n = eval ex3 [("count", Int n)];;
 
@@ -100,17 +105,17 @@ let rundeep n = eval ex3 [("count", Int n)];;
 
 let ex4 =
     Let("y", CstI 11,
-        Letfun("f", "x", Prim("+", Var "x", Var "y"),
-               Let("y", CstI 22, Call(Var "f", CstI 3))));;
+        Letfun("f", ["x"], Prim("+", Var "x", Var "y"),
+               Let("y", CstI 22, Call(Var "f", [CstI 3]))));;
 
 (* Example: two function definitions: a comparison and Fibonacci *)
 
 let ex5 = 
-    Letfun("ge2", "x", Prim("<", CstI 1, Var "x"),
-           Letfun("fib", "n",
-                  If(Call(Var "ge2", Var "n"),
+    Letfun("ge2", ["x"], Prim("<", CstI 1, Var "x"),
+           Letfun("fib", ["n"],
+                  If(Call(Var "ge2", [Var "n"]),
                      Prim("+",
-                          Call(Var "fib", Prim("-", Var "n", CstI 1)),
-                          Call(Var "fib", Prim("-", Var "n", CstI 2))),
-                     CstI 1), Call(Var "fib", CstI 25)));;
+                          Call(Var "fib", [Prim("-", Var "n", CstI 1)]),
+                          Call(Var "fib", [Prim("-", Var "n", CstI 2)])),
+                     CstI 1), Call(Var "fib", [CstI 25])));;                   
```

## PLC 4.4

This is how we solved exercise 4.4 by making changes in the `FunPar.fsy`:

```diff
commit 5e186c7c639050c166b7b079fd0d1583b3a4cff7
Author: Andreas Wachs <andreas@wachs.dk>
Date:   Mon Sep 26 20:05:07 2022 +0200

    Solved the hard part of 4.4

diff --git a/week4/Fun/FunPar.fsy b/week4/Fun/FunPar.fsy
index c0addbf..a146b61 100644
--- a/week4/Fun/FunPar.fsy
+++ b/week4/Fun/FunPar.fsy
@@ -27,6 +28,7 @@
 %start Main
 %type <Absyn.expr> Main Expr AtExpr Const
 %type <Absyn.expr> AppExpr
+%type <Absyn.expr list> AtExprs
+%type <string list> Names
 
 %%
 
@@ -56,13 +58,18 @@ AtExpr:
     Const                               { $1                     }
   | NAME                                { Var $1                 }
   | LET NAME EQ Expr IN Expr END        { Let($2, $4, $6)        }
-  | LET NAME NAME EQ Expr IN Expr END   { Letfun($2, $3, $5, $7) }
+  | LET NAME Names EQ Expr IN Expr END  { Letfun($2, $3, $5, $7) }
   | LPAR Expr RPAR                      { $2                     }
 ;
 
+Names:
+  NAME                                  { [$1] }
+  | NAME Names                          { $1 :: $2 }
+;
+
 AppExpr:
-    AtExpr AtExpr                       { Call($1, $2)           }
-  | AppExpr AtExpr                      { Call($1, $2)           }
+    AtExpr AtExprs                      { Call($1, $2) }
 ;

+AtExprs:
+    AtExpr AtExprs                      { $1 :: $2 }
+  | AtExpr                              { [$1] }
+;
```

Here is an example output to verify that our solution works:

```fsi
> open Parse;;
> open ParseAndRun;;
> fromString "let pow x n = if n=0 then 1 else x * pow x (n-1) in pow 3 8 end"
- ;;
val it : Absyn.expr =
  Letfun
    ("pow", ["x"; "n"],
     If
       (Prim ("=", Var "n", CstI 0), CstI 1,
        Prim
          ("*", Var "x",
           Call (Var "pow", [Var "x"; Prim ("-", Var "n", CstI 1)]))),
     Call (Var "pow", [CstI 3; CstI 8]))

> fromString "let pow x n = if n=0 then 1 else x * pow x (n-1) in pow 3 8 end" |> run;;
val it : int = 6561
```

## PLC 4.5


Here is a diff of the changes that we've made to solve this exercise. We chose to have the `&&` and `||` .operators to have lower precedence than other logical operators to reflect the behavior of other programming languages.

```diff
diff --git a/week4/Fun/FunLex.fsl b/week4/Fun/FunLex.fsl
index f3370a4..7a96812 100644
--- a/week4/Fun/FunLex.fsl
+++ b/week4/Fun/FunLex.fsl
@@ -53,6 +53,8 @@ rule Token = parse
   | '*'             { TIMES }                     
   | '/'             { DIV }                     
   | '%'             { MOD }
+  | "&&"            { LAND }
+  | "||"            { LOR }
   | '('             { LPAR }
   | ')'             { RPAR }
   | eof             { EOF }
diff --git a/week4/Fun/FunPar.fsy b/week4/Fun/FunPar.fsy
index a146b61..eff149c 100644
--- a/week4/Fun/FunPar.fsy
+++ b/week4/Fun/FunPar.fsy
@@ -14,11 +14,12 @@
 
 %token ELSE END FALSE IF IN LET NOT THEN TRUE
 %token PLUS MINUS TIMES DIV MOD
-%token EQ NE GT LT GE LE
+%token EQ NE GT LT GE LE LAND LOR
 %token LPAR RPAR 
 %token EOF
 
 %left ELSE              /* lowest precedence  */
+%left LAND LOR
 %left EQ NE 
 %left GT LT GE LE
 %left PLUS MINUS
@@ -52,6 +53,9 @@ Expr:
   | Expr LT    Expr                     { Prim("<",  $1, $3)     }
   | Expr GE    Expr                     { Prim(">=", $1, $3)     }
   | Expr LE    Expr                     { Prim("<=", $1, $3)     }
+  | Expr LAND  Expr                     { If($1, $3, CstB false) }
+  | Expr LOR   Expr                     { If($1, CstB true, $3)  }
+  
 ;
```

Here is some output from testing the implementation:

```fsi
> fromString "3 < 5 && (10 < 5 || 2 = 2)";;       
val it : Absyn.expr =
  If
    (Prim ("<", CstI 3, CstI 5),
     If (Prim ("<", CstI 10, CstI 5), CstB true, Prim ("=", CstI 2, CstI 2)),
     CstB false)

> run it;;
val it : int = 1
```