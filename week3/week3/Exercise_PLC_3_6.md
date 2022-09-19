# PLC 3.6

We have added `compString` to the bottom of `Expr.fs` by the `(* Exercise 3.5 *)` comment. For practicality's sake, the function is also inserted here:

```fs
let compString: string -> sinstr list 
      = fun source ->
            source |> Parse.fromString |> fun e -> scomp e []
```

An example run can be seen here:

```
$ dotnet fsi -r FsLexYacc.Runtime.dll Absyn.fs ExprPar.fs ExprLex.fs Parse.fs Expr.fs

# Lots of output...

> compString "let x = 5 in x + 2 end";;
val it: sinstr list = [SCstI 5; SVar 0; SCstI 2; SAdd; SSwap; SPop]
```
