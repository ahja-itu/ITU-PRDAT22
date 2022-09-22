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

## PLC 4.3

## PLC 4.4

## PLC 4.5
