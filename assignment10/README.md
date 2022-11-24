# Assignment 10

Authors: adbo, aljb and ahja

This code can also be found online at <https://github.com/andreaswachs/ITU-PRDAT22>.

We recommend reading this Markdown page on [GitHub](https://github.com/andreaswachs/ITU-PRDAT22/blob/main/assignment10/README.md) or VSCode because it makes the diffs easier to read.

## Solutions

### 11.1

#### I


We implemented the `lenc` function using CSP:

```fs
let rec lenc lst k =
    match lst with
    | []      -> k 0
    | _ :: xs -> lenc xs (fun v -> k(v + 1))
```

This produced the output, when running in FSI:

```fsi
> lenc [2; 5; 7] (printf "The answer is ’%d’\n");;
The answer is ’3’
val it: unit = ()
```

#### II

Using the `lenc xs (fun v -> 2*v)` call, we get the following output:

```fsi
> let xs = [2; 5; 7];;
val xs: int list = [2; 5; 7]

> lenc xs (fun v -> 2*v);;
val it: int = 6
```

We see that the calculated length of the function is doubled, as the base continuation function doubles the result, which is then *propagated* throughout the counting, which results in the length calculation returning the double of the actual length.

#### III

We implemented the tail-recursive list length function `lenny`:

```fs
let rec lenny lst acc =
    match lst with
    | [] -> acc
    | _ :: xs -> lenny xs ((+) acc 1)
```

Running it produces the following output:

```fsi
> let xs = [2; 5; 7];;
val xs: int list = [2; 5; 7]

> lenny xs 0;;
val it: int = 3

> lenny [] 0;;
val it: int = 0
```

The relation between the CPS and tail-recursive accumulator functions is that the tail-recursive accumulator `acc` is a simpler function relative to the continuation function `k`.

## 11.2


### I

We've implemented the CPS style function for reversing a list:

```fs
let rec revc lst k =
    match lst with
    | []      -> k []
    | x :: xs -> revc xs (fun vs -> k(vs @ [x]))
```

The function outputs the reversed list:

```fsi
> revc [1; 2; 3] id;;
val it: int list = [3; 2; 1]
```

#### II

Running the code

```fs
> revc xs (fun v -> v @ v);;
val it: int list = [7; 5; 2; 7; 5; 2]
```

We see that the list gets reversed, but the list is duplicated and appended to itself.

#### III

We implemented the tail-recursive function `revi` using an accumulator to reverse the given list:

```fs
let rec revi lst acc =
    match lst with
    | []      -> acc
    | x :: xs -> revi xs (x :: acc)
```

We run the function:

```fsi
> revi xs [];;
val it: int list = [7; 5; 2]
```

### Exercise 11.3

We implemented `prodc` in the CSP. 

```fs
let rec prodc lst k =
    match lst with
    | [] -> k 1
    | x :: xs -> prodc xs (fun v -> k(x * v))
```

Running it produces the following output:

```fsi
> prodc [1; 2; 3; 4; 5; 6; 7] id;;
val it: int = 5040

> prodc [] id;;
val it: int = 1
```

### 11.4

We created an optimized version of `prodc` named `prodc'`:

```fs
let rec prodc' lst k =
    let rec aux lst' k' =
        match lst' with
        | [] -> k' 1
        | x :: _ when x = 0 -> k 0
        | x :: xs -> aux xs (fun v -> k'(x * v))

    aux lst k
```

This outputs the following results:

```fsi
> prodc' [1; 2; 3] id;;
val it: int = 6

> prodc' [1; 0; 2; 3] id;;
val it: int = 0
```

We also implemented the tail-recursive function `prodi`:


```fs
let rec prodi lst acc =
    match lst with
    | [] -> acc
    | x :: _ when x = 0 -> 0
    | x :: xs -> prodi xs (acc * x)
```

We ran it and it produced the following output:

```fsi
> prodi [1; 2; 3] 1;;
val it: int = 6

> prodi [1; 0; 2; 3] 1;;
val it: int = 0
```

### Exercise 11.8

#### I

We wrote an expression that writes `3 5 7 9`:

```fsi
> run (Every(Write(Prim("+", CstI 1, (Prim("*", CstI 2, FromTo(1, 4)))))));; 
3 5 7 9 val it: value = Int 0
```

We wrote the problem of crating the abstract syntax with informal textual syntax:

```txt
1. (2 to 4) * 10 -> 20 30 40
2. ((2 to 4) * 10) + (1 to 2) -> 21 22 31 32 41 42
```

We implemented the MicroIcon abstract syntax to produce the output `21 22 31 32 41 42`:

```fsi
> run (Every(Write(Prim("+", Prim("*", FromTo(2, 4), CstI 10), FromTo(1, 2)))));;
21 22 31 32 41 42 val it: value = Int 0
```

#### II

We wrote the abstract syntax to find the least multiple of 7, that is greater than 50:

```fsi
> run (Write(Prim("<", CstI 50, Prim("*", FromTo(1, 10), CstI 7))));;
56 val it: value = Int 56
```

#### III

```diff
diff --git a/assignment10/Cont/Icon.fs b/assignment10/Cont/Icon.fs
index c838470..635874f 100644
--- a/assignment10/Cont/Icon.fs
+++ b/assignment10/Cont/Icon.fs
@@ -31,6 +31,7 @@ type expr =
   | Write of expr
   | If of expr * expr * expr
   | Prim of string * expr * expr 
+  | Prim1 of string * expr
   | And of expr * expr
   | Or  of expr * expr
   | Seq of expr * expr
@@ -88,6 +89,17 @@ let rec eval (e : expr) (cont : cont) (econt : econt) =
               | _ -> Str "unknown prim2")
               econt1)
           econt
+    | Prim1(ope, e) ->
+      eval e (fun v -> fun econt ->
+        match (ope, v) with
+        | ("sqr", Int i) -> 
+          cont (Int (i * i)) econt
+        | ("even", Int i) -> 
+          match i % 2 with
+          | 0 -> cont (Int i) econt
+          | _ -> econt ()
+        | _ -> Str "unknown prim1"
+      ) econt
     | And(e1, e2) -> 
       eval e1 (fun _ -> fun econt1 -> eval e2 cont econt1) econt
     | Or(e1, e2) -> 
```

We validate our changes:

```fsi
> run (Every(Write(Prim1("sqr", FromTo(3, 6)))));;
9 16 25 36 val it: value = Int 0

> run (Every(Write(Prim1("even", FromTo(1, 7)))));;
2 4 6 val it: value = Int 0
```

#### IV

We made the following changes to impement the *Multiples* function

```diff
diff --git a/assignment10/Cont/Icon.fs b/assignment10/Cont/Icon.fs
index 635874f..962e48c 100644
--- a/assignment10/Cont/Icon.fs
+++ b/assignment10/Cont/Icon.fs
@@ -98,6 +98,10 @@ let rec eval (e : expr) (cont : cont) (econt : econt) =
           match i % 2 with
           | 0 -> cont (Int i) econt
           | _ -> econt ()
+        | ("multiples", Int i) ->
+          let rec loop n  =
+            cont (Int (i * n)) (fun () -> loop (n + 1))
+          loop 1
         | _ -> Str "unknown prim1"
       ) econt
     | And(e1, e2) -> 

```

