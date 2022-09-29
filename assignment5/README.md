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
