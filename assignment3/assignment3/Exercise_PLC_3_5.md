# PLC 3.5

The following commands have been run in the `Expr` directory:

## Build

```text
$ ~/bin/fsharp/fslex --unicode ExprLex.fsl
compiling to dfas (can take a while...)
15 states
writing output

$ ~/bin/fsharp/fsyacc --module ExprPar ExprPar.fsy
        building tables
computing first function...        time: 00:00:00.0720975
building kernels...        time: 00:00:00.0509709
building kernel table...        time: 00:00:00.0145109
computing lookahead relations...........................        time: 00:00:00.0810510
building lookahead table...        time: 00:00:00.0218881
building action table...        time: 00:00:00.0191380
        building goto table...        time: 00:00:00.0043554
        returning tables.
        24 states
        3 nonterminals
        16 terminals
        10 productions
        #rows in action table: 24

$ dotnet fsi -r FsLexYacc.Runtime.dll Absyn.fs ExprPar.fs ExprLex.fs Parse.fs

# ... and you're now in interactive mode
```

## Running

Parsing well-formed expressions:

```fs
> open Parse;;

> fromString "1 + 2 * 3";;
val it : Absyn.expr = Prim ("+", CstI 1, Prim ("*", CstI 2, CstI 3))

> fromString "1 + 2 - 3";;
val it : Absyn.expr = Prim ("-", Prim ("+", CstI 1, CstI 2), CstI 3)

> fromString "1 - 2 - 3";;
val it : Absyn.expr = Prim ("-", Prim ("-", CstI 1, CstI 2), CstI 3)

> fromString "1 + -2";;
val it : Absyn.expr = Prim ("+", CstI 1, CstI -2)

> fromString "let z = (17) in z + 2 * 3 end";;
val it : Absyn.expr =
  Let ("z", CstI 17, Prim ("+", Var "z", Prim ("*", CstI 2, CstI 3)))
```

Parsing ill-formed expressions: (Stack traces omitted)

```fs
> open Parse;;

> fromString "x++";;
System.Exception: parse error near line 1, column 3

> fromString "1 + 1.2";;
System.Exception: Lexer error: illegal symbol near line 1, column 6

> fromString "1 + ";;
System.Exception: parse error near line 1, column 4

> fromString "let z = 17) in z + 2 * 3 end";;
System.Exception: parse error near line 1, column 11
```
