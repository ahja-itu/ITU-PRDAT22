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
