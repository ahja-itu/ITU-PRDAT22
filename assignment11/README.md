# Assignment 11

Authors: adbo, aljb and ahja

This code can also be found online at <https://github.com/andreaswachs/ITU-PRDAT22>.

We recommend reading this Markdown page on [GitHub](https://github.com/andreaswachs/ITU-PRDAT22/blob/main/assignment11/README.md) or VSCode because it makes the diffs easier to read.

## Solutions

### PLC 12.1

We added the `addIFZERO` and `addIFNZERO` helper functions, and called it from the appropriate locations.

```diff
diff --git a/assignment11/MicroC/Contcomp.fs b/assignment11/MicroC/Contcomp.fs
index d37440c..48ccc7e 100644
--- a/assignment11/MicroC/Contcomp.fs
+++ b/assignment11/MicroC/Contcomp.fs
@@ -70,11 +70,21 @@ let rec deadcode C =
     | Label lab :: _  -> C
     | _         :: C1 -> deadcode C1
 
+let rec addIFZERO lab3 C =
+  match C with
+  | GOTO lab1 :: Label lab2 :: C1 when lab2 = lab3 -> addIFNZERO lab1 (Label lab2 :: C1)
+  | _ -> IFZERO lab3 :: C
+
+and addIFNZERO lab3 C =
+  match C with
+  | GOTO lab1 :: Label lab2 :: C1 when lab2 = lab3 -> addIFZERO lab1 (Label lab2 :: C1)
+  | _ -> IFNZRO lab3 :: C
+
 let addNOT C =
     match C with
     | NOT        :: C1 -> C1
-    | IFZERO lab :: C1 -> IFNZRO lab :: C1 
-    | IFNZRO lab :: C1 -> IFZERO lab :: C1 
+    | IFZERO lab :: C1 -> addIFNZERO lab C1 
+    | IFNZRO lab :: C1 -> addIFZERO lab C1
     | _                -> NOT :: C
 
 let addJump jump C =                    (* jump is GOTO or RET *)
@@ -103,7 +113,7 @@ let rec addCST i C =
     | (0, IFNZRO lab :: C1) -> C1
     | (_, IFNZRO lab :: C1) -> addGOTO lab C1
     | _                     -> CSTI i :: C

 (* ------------------------------------------------------------------- *)
 
 (* Simple environment operations *)
@@ -188,12 +198,12 @@ let rec cStmt stmt (varEnv : varEnv) (funEnv : funEnv) (C : instr list) : instr
     | If(e, stmt1, stmt2) -> 
       let (jumpend, C1) = makeJump C
       let (labelse, C2) = addLabel (cStmt stmt2 varEnv funEnv C1)
-      cExpr e varEnv funEnv (IFZERO labelse 
-       :: cStmt stmt1 varEnv funEnv (addJump jumpend C2))
+      cExpr e varEnv funEnv (addIFZERO labelse 
+       (cStmt stmt1 varEnv funEnv (addJump jumpend C2)))
     | While(e, body) ->
       let labbegin = newLabel()
       let (jumptest, C1) = 
-           makeJump (cExpr e varEnv funEnv (IFNZRO labbegin :: C))
+           makeJump (cExpr e varEnv funEnv (addIFNZERO labbegin C))
       addJump jumptest (Label labbegin :: cStmt body varEnv funEnv C1)
     | Expr e -> 
       cExpr e varEnv funEnv (addINCSP -1 C) 
@@ -272,33 +282,33 @@ and cExpr (e : expr) (varEnv : varEnv) (funEnv : funEnv) (C : instr list) : inst
     | Andalso(e1, e2) ->
       match C with
       | IFZERO lab :: _ ->
-         cExpr e1 varEnv funEnv (IFZERO lab :: cExpr e2 varEnv funEnv C)
+         cExpr e1 varEnv funEnv (addIFZERO lab (cExpr e2 varEnv funEnv C))
       | IFNZRO labthen :: C1 -> 
         let (labelse, C2) = addLabel C1
         cExpr e1 varEnv funEnv
-           (IFZERO labelse 
-              :: cExpr e2 varEnv funEnv (IFNZRO labthen :: C2))
+           (addIFZERO labelse
+              (cExpr e2 varEnv funEnv (addIFNZERO labthen C2)))
       | _ ->
         let (jumpend,  C1) = makeJump C
         let (labfalse, C2) = addLabel (addCST 0 C1)
         cExpr e1 varEnv funEnv
-          (IFZERO labfalse 
-             :: cExpr e2 varEnv funEnv (addJump jumpend C2))
+          (addIFZERO labfalse 
+             (cExpr e2 varEnv funEnv (addJump jumpend C2)))
     | Orelse(e1, e2) -> 
       match C with
       | IFNZRO lab :: _ -> 
-        cExpr e1 varEnv funEnv (IFNZRO lab :: cExpr e2 varEnv funEnv C)
+        cExpr e1 varEnv funEnv (addIFNZERO lab (cExpr e2 varEnv funEnv C))
       | IFZERO labthen :: C1 ->
         let(labelse, C2) = addLabel C1
         cExpr e1 varEnv funEnv
-           (IFNZRO labelse :: cExpr e2 varEnv funEnv
-             (IFZERO labthen :: C2))
+           (addIFNZERO labelse (cExpr e2 varEnv funEnv
+             (addIFZERO labthen C2)))
       | _ ->
         let (jumpend, C1) = makeJump C
         let (labtrue, C2) = addLabel(addCST 1 C1)
         cExpr e1 varEnv funEnv
-           (IFNZRO labtrue 
-             :: cExpr e2 varEnv funEnv (addJump jumpend C2))
+           (addIFNZERO labtrue 
+             (cExpr e2 varEnv funEnv (addJump jumpend C2)))
     | Call(f, es) -> callfun f es varEnv funEnv C
```

Testing optimization of compiler with `ex16.c`, we see that `GETBP; LDI; IFNZRO L2;` has been generated instead of `GETBP; LDI; IFZERO L3; GOTO L2;`.

```fsi
$ make run
(* ... *)

> open ParseAndContcomp;;

> contCompileToFile (fromFile "ex16.c") "ex16.out";;
val it: Machine.instr list =
  [LDARGS; CALL (1, "L1"); STOP; Label "L1"; GETBP; LDI; IFNZRO "L2";
   Label "L3"; CSTI 1111; PRINTI; INCSP -1; Label "L2"; CSTI 2222; PRINTI;
   RET 1]
```

### PLC 12.2

We added comparison-based optimizations to the `addCST` helper function.

```diff
diff --git a/assignment11/MicroC/Contcomp.fs b/assignment11/MicroC/Contcomp.fs
index 48ccc7e..5e9d89d 100644
--- a/assignment11/MicroC/Contcomp.fs
+++ b/assignment11/MicroC/Contcomp.fs
@@ -112,6 +112,12 @@ let rec addCST i C =
     | (_, IFZERO lab :: C1) -> C1
     | (0, IFNZRO lab :: C1) -> C1
     | (_, IFNZRO lab :: C1) -> addGOTO lab C1
+    | (x, CSTI y :: EQ :: C1)                -> addCST (if x = y then 1 else 0) C1
+    | (x, CSTI y :: EQ :: NOT :: C1)         -> addCST (if x <> y then 1 else 0) C1
+    | (x, CSTI y :: LT :: C1)                -> addCST (if x < y then 1 else 0) C1
+    | (x, CSTI y :: LT :: NOT :: C1)         -> addCST (if x >= y then 1 else 0) C1
+    | (x, CSTI y :: SWAP :: LT :: C1)        -> addCST (if x > y then 1 else 0) C1
+    | (x, CSTI y :: SWAP :: LT :: NOT :: C1) -> addCST (if x <= y then 1 else 0) C1
     | _                     -> CSTI i :: C
```

We tested each of the comparison-based optimizations. Note that all of the results has been compiled to code that either unconditionally prints or does nothing. (All comparisons are between the values 11 and 22, and will print 33 if true)

```fsi
$ make run
(* ... *)

> open ParseAndContcomp;;

// Equal to
> contCompileToFile (fromFile "eq.c") "eq.out";;    
val it: Machine.instr list =
  [LDARGS; CALL (1, "L1"); STOP; Label "L1"; Label "L2"; RET 0]

// Not equal to
> contCompileToFile (fromFile "neq.c") "neq.out";;
val it: Machine.instr list =
  [LDARGS; CALL (1, "L1"); STOP; Label "L1"; CSTI 33; PRINTI; RET 1;
   Label "L2"; RET 0]

// Less than
> contCompileToFile (fromFile "lt.c") "lt.out";; 
val it: Machine.instr list =
  [LDARGS; CALL (1, "L1"); STOP; Label "L1"; CSTI 33; PRINTI; RET 1;
   Label "L2"; RET 0]

// Less than or equal to
> contCompileToFile (fromFile "lteq.c") "lteq.out";;
val it: Machine.instr list =
  [LDARGS; CALL (1, "L1"); STOP; Label "L1"; CSTI 33; PRINTI; RET 1;
   Label "L2"; RET 0]

// Greater than
> contCompileToFile (fromFile "gt.c") "gt.out";;    
val it: Machine.instr list =
  [LDARGS; CALL (1, "L1"); STOP; Label "L1"; Label "L2"; RET 0]

// Greater than or equal to
> contCompileToFile (fromFile "gteq.c") "gteq.out";;
val it: Machine.instr list =
  [LDARGS; CALL (1, "L1"); STOP; Label "L1"; Label "L2"; RET 0]
```

### PLC 12.3

We added the conditional operator to the abstract syntax, and extended the continuation-based micro-C compiler. We used the optimizations from addIFZERO, makeJump and addLabel.

```diff
diff --git a/assignment11/MicroC/Absyn.fs b/assignment11/MicroC/Absyn.fs
index da8f60c..455fb5a 100644
--- a/assignment11/MicroC/Absyn.fs
+++ b/assignment11/MicroC/Absyn.fs
@@ -23,6 +23,7 @@ and expr =
   | Andalso of expr * expr           (* Sequential and              *)
   | Orelse of expr * expr            (* Sequential or               *)
   | Call of string * expr list       (* Function call f(...)        *)
+  | Cond of expr * expr * expr       (* Conditional e1 ? e2 : e3    *)

diff --git a/assignment11/MicroC/Contcomp.fs b/assignment11/MicroC/Contcomp.fs
index 5e9d89d..b5e92ae 100644
--- a/assignment11/MicroC/Contcomp.fs
+++ b/assignment11/MicroC/Contcomp.fs
@@ -316,6 +316,13 @@ and cExpr (e : expr) (varEnv : varEnv) (funEnv : funEnv) (C : instr list) : inst
            (addIFNZERO labtrue 
              (cExpr e2 varEnv funEnv (addJump jumpend C2)))
     | Call(f, es) -> callfun f es varEnv funEnv C
+    | Cond(e1, e2, e3) ->
+      let (jumpend, C1) = makeJump C
+      let (l1, C2) = cExpr e3 varEnv funEnv C1 |> addLabel
+      addJump jumpend C2
+      |> cExpr e2 varEnv funEnv
+      |> addIFZERO l1
+      |> cExpr e1 varEnv funEnv
```

Testing this on the following two examples produces this result:

`1 ? 1111 : 2222`

```fsi
> cExpr (Cond(CstI 1, CstI 1111, CstI 2222)) ([], 0) [] [];; 
val it: Machine.instr list =
  [CSTI 1111; GOTO "L0"; Label "L1"; CSTI 2222; Label "L0"]
```

`0 ? 1111 : 2222`

```fsi
> cExpr (Cond(CstI 0, CstI 1111, CstI 2222)) ([], 0) [] [];;
val it: Machine.instr list = [Label "L3"; CSTI 2222; Label "L2"]
```

Note how the conditional has been optimized away.

### PLC 12.4

(Old)

```fsi
> contCompileToFile (fromFile "ex13.c") "ex13.out";;
val it: Machine.instr list =
  [LDARGS; CALL (1, "L1"); STOP; Label "L1"; INCSP 1; GETBP; CSTI 1; ADD;
   CSTI 1889; STI; INCSP -1; GOTO "L3"; Label "L2"; GETBP; CSTI 1; ADD; GETBP;
   CSTI 1; ADD; LDI; CSTI 1; ADD; STI; INCSP -1; GETBP; CSTI 1; ADD; LDI;
   CSTI 4; MOD; IFNZRO "L3"; GETBP; CSTI 1; ADD; LDI; CSTI 100; MOD;
   IFNZRO "L4"; GETBP; CSTI 1; ADD; LDI; CSTI 400; MOD; IFNZRO "L3";
   Label "L4"; GETBP; CSTI 1; ADD; LDI; PRINTI; INCSP -1; Label "L3"; GETBP;
   CSTI 1; ADD; LDI; GETBP; LDI; LT; IFNZRO "L2"; RET 1]
```
