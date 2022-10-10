# Assignment 5

Authors: adbo, aljb and ahja

This code can also be found online at <https://github.com/andreaswachs/ITU-PRDAT22>.

We recommend reading this Markdown page on [GitHub](https://github.com/andreaswachs/ITU-PRDAT22/blob/main/assignment5/README.md) or VSCode, because it makes the diffs easier to read.

## Solutions

### PLC 5.1

The F# function as seen in `merge.fs`.

```fs
let rec merge xs ys =
    match xs, ys with
    | [], _ -> ys
    | _, [] -> xs
    | x::xs, y::ys when x < y ->
        x :: merge xs (y::ys)
    | x::xs, y::ys ->
        y :: merge (x::xs) ys
```

The C# function as seen in `merge.csx`.

```cs
static int[] merge(int[] xs, int[] ys) {
    int[] zs = new int[xs.Length + ys.Length];

    int i = 0;
    int j = 0;

    while (i < xs.Length || j < ys.Length) {
        if (i < xs.Length && j < ys.Length) {
            if (xs[i] <= ys[j]) {
                zs[i + j] = xs[i];
                i++;
            } else {
                zs[i + j] = ys[j];
                j++;
            }
        } else if (i < xs.Length) {
            zs[i + j] = xs[i];
            i++;
        } else {
            zs[i + j] = ys[j];
            j++;
        }
    }

    return zs;
}
```

Output for the F# function:

```fs
$ dotnet fsi merge.fs

> open merge;;
> merge [3; 5; 12] [2; 3; 4; 7];;
val it: int list = [2; 3; 3; 4; 5; 7; 12]
```

Output for the C# function:

```cs
$ csi

> #load "merge.csx"
> var arr = merge(new int[] {3, 5, 12}, new int[] {2, 3, 4, 7});;
> Console.WriteLine($"[{string.Join(", ", arr)}]");
[2, 3, 3, 4, 5, 7, 12]
```

### PLC 5.7

The changes made to `TypeInference.fs` are shown below. We have not tested the changes, because the custom language doesn't support lists yet.

```diff
diff --git a/assignment5/Fun/TypeInference.fs b/assignment5/Fun/TypeInference.fs
index 45e8e7f..9cfed38 100644
--- a/assignment5/Fun/TypeInference.fs
+++ b/assignment5/Fun/TypeInference.fs
@@ -53,6 +53,7 @@ type typ =
      | TypB                                (* booleans                   *)
      | TypF of typ * typ                   (* (argumenttype, resulttype) *)
      | TypV of typevar                     (* type variable              *)
+     | TypL of typ                         (* list, element type is typ  *)
 
 and tyvarkind =  
      | NoLink of string                    (* uninstantiated type var.   *)
@@ -147,6 +148,7 @@ let rec showType t : string =
           | (NoLink name, _) -> name
           | _                -> failwith "showType impossible"
         | TypF(t1, t2) -> "(" + pr t1 + " -> " + pr t2 + ")"
+        | TypL(t1)     -> "[" + pr t1 + "]"
     pr t 
 
 let rec showTEnv tenv =
@@ -178,6 +180,7 @@ let rec unify t1 t2 : unit =
                                   else linkVarToType tv2 t1'
     | (TypV tv1, _       ) -> linkVarToType tv1 t2'
     | (_,        TypV tv2) -> linkVarToType tv2 t1'
+    | (TypL t1, TypL t2) -> unify t1 t2
     | (TypI,     t) -> failwith ("type error: int and " + typeToString t)
     | (TypB,     t) -> failwith ("type error: bool and " + typeToString t)
     | (TypF _,   t) -> failwith ("type error: function and " + typeToString t)
@@ -223,6 +226,7 @@ let rec copyType subst t : typ =
     | TypF(t1,t2) -> TypF(copyType subst t1, copyType subst t2)
     | TypI        -> TypI
     | TypB        -> TypB
+    | TypL(t1)    -> TypL(copyType subst t1)
```

### PLC 6.1

```fs
> open ParseAndRunHigher;;

> run (fromString @"let add x = let f y = x+y in f end
-                   in add 2 5 end");;
val it : HigherFun.value = Int 7

> run (fromString @"let add x = let f y = x+y in f end
- in let addtwo = add 2
-    in addtwo 5 end
- end");;
val it : HigherFun.value = Int 7

> run (fromString @"let add x = let f y = x+y in f end
- in let addtwo = add 2
-    in let x = 77 in addtwo 5 end
- end end");;
val it : HigherFun.value = Int 7
```

The result above is as expected because the language is statically scoped, so `f` will use the `x` from `let add x`, which means the `x` from `let x = 77` is never used.
If the language had been dynamically scoped, `x` would have been overwritten.

```fs
> run (fromString @"let add x = let f y = x+y in f end
- in add 2 end");;
val it : HigherFun.value =
  Closure
    ("f", "y", Prim ("+", Var "x", Var "y"),
     [("x", Int 2);
      ("add",
       Closure
         ("add", "x", Letfun ("f", "y", Prim ("+", Var "x", Var "y"), Var "f"),
          []))])
```

The result above shows partial application of a function. The return value of `add x` is a function that adds `2` to the given argument.

### PLC 6.2

The changes are showcased via this git diff:

```diff
diff --git a/assignment5/Fun/Absyn.fs b/assignment5/Fun/Absyn.fs
index b0dfc3f..8194c9a 100644
--- a/assignment5/Fun/Absyn.fs
+++ b/assignment5/Fun/Absyn.fs
@@ -11,3 +11,4 @@ type expr =
   | If of expr * expr * expr
   | Letfun of string * string * expr * expr    (* (f, x, fBody, letBody) *)
   | Call of expr * expr
+  | Fun of string * expr
diff --git a/assignment5/Fun/HigherFun.fs b/assignment5/Fun/HigherFun.fs
--- a/assignment5/Fun/HigherFun.fs
+++ b/assignment5/Fun/HigherFun.fs
@@ -30,6 +30,7 @@ let rec lookup env x =
 type value = 
   | Int of int
   | Closure of string * string * expr * value env       (* (f, x, fBody, fDeclEnv) *)
+  | Clos of string * expr * value env
 
 let rec eval (e : expr) (env : value env) : value =
     match e with
@@ -65,7 +66,9 @@ let rec eval (e : expr) (env : value env) : value =
         let xVal = eval eArg env
         let fBodyEnv = (x, xVal) :: (f, fClosure) :: fDeclEnv
         in eval fBody fBodyEnv
-      | _ -> failwith "eval Call: not a function";;
+      | _ -> failwith "eval Call: not a function"
+    | Fun(x, body) ->
+        Clos(x, body, env)
```

We test that the abstract syntax can be evaluated in the next exercise.

### PLC 6.3

The changes are showcased via this git diff:

```diff
diff --git a/assignment5/Fun/FunLex.fsl b/assignment5/Fun/FunLex.fsl
index f65ce27..775d9b0 100644
--- a/assignment5/Fun/FunLex.fsl
+++ b/assignment5/Fun/FunLex.fsl
@@ -30,6 +30,7 @@ let keyword s =
     | "not"   -> NOT
     | "then"  -> THEN
     | "true"  -> CSTBOOL true
+    | "fun"   -> FUN
     | _       -> NAME s
 }
 
@@ -48,6 +49,7 @@ rule Token = parse
   | '<'             { LT }
   | ">="            { GE }
   | "<="            { LE }
+  | "->"            { RARROW }
   | '+'             { PLUS }                     
   | '-'             { MINUS }                     
   | '*'             { TIMES }                     
--- a/assignment5/Fun/FunPar.fsy
+++ b/assignment5/Fun/FunPar.fsy
@@ -11,13 +11,15 @@
 %token <string> NAME
 %token <bool> CSTBOOL
 
-%token ELSE END FALSE IF IN LET NOT THEN TRUE
+%token ELSE END FALSE IF IN LET NOT THEN TRUE FUN
+%token RARROW
 %token PLUS MINUS TIMES DIV MOD
 %token EQ NE GT LT GE LE
 %token LPAR RPAR 
 %token EOF
 
 %left ELSE              /* lowest precedence  */
+%left RARROW
 %left EQ NE 
 %left GT LT GE LE
 %left PLUS MINUS
@@ -38,6 +40,7 @@ Expr:
     AtExpr                              { $1                     }
   | AppExpr                             { $1                     }
   | IF Expr THEN Expr ELSE Expr         { If($2, $4, $6)         }
+  | FUN NAME RARROW Expr                { Fun($2, $4)            }
   | MINUS Expr                          { Prim("-", CstI 0, $2)  }
   | Expr PLUS  Expr                     { Prim("+",  $1, $3)     }
   | Expr MINUS Expr                     { Prim("-",  $1, $3)     }
```

In order to avoid an ambiguous language, we have to explicitly define precedence on the new `->` token. Otherwise, both of the follow would be a valid parse result for `fun x -> 2 * x`:

- `(fun x -> 2) * x`
- `fun x -> (2 * x)` (we want this one)

Therefore, we have defined the `->` token as left associative with lower precedence than all the logical and arithmetic operators.

```fsi
$ ~/bin/fsharp/fslex --unicode FunLex.fsl
...
$ ~/bin/fsharp/fsyacc --module FunPar FunPar.fsy
...
$ fsharpi -r /root/.local/bin/fsharp/FsLexYacc.Runtime.dll Absyn.fs HigherFun.fs FunPar.fs FunLex.fs Parse.fs ParseAndRunHigher.fs
...
> fromString "fun x -> 2 * x";;    
val it : Absyn.expr = Fun ("x", Prim ("*", CstI 2, Var "x"))

> run it;;
val it : HigherFun.value = Clos ("x", Prim ("*", CstI 2, Var "x"), [])

> fromString "let y = 22 in fun z -> z + y end";;
val it : Absyn.expr =
  Let ("y", CstI 22, Fun ("z", Prim ("+", Var "z", Var "y")))

> run it;;
val it : HigherFun.value =
  Clos ("z", Prim ("+", Var "z", Var "y"), [("y", Int 22)])
```


### PLC 6.4

#### PLC 6.4.1

The reason for `f` being polymorphic in the let-body, we cannot derive a specific type for `x`, therefore can be any type.

![type derivation tree for PLC 6.4.1](PLC%206.4.1.png)

#### PLC 6.4.2

The reason why `f` should not be polymorphic in the `f` let-body is due to the `+` operator limits the polymorphic types that `f` can assume. Also, `x` needs to be an integer to match the return type of the first branch of the if expression.

![type derivation tree for PLC 6.4.2](PLC%206.4.2.png)
