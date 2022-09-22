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

## PLC 4.4

## PLC 4.5
