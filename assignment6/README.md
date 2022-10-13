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
