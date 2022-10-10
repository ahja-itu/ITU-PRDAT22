# HelloLex

## Question 1

There is only one regular expression involved: `[0-9]`, which represents a single digit.

## Question 2

1. A `hello.fs` file is generated

2. There are 3 states according to the output:

Command line output to support the above:

```sh
$ ls
hello.fsl

$ ~/bin/fsharp/fslex --unicode hello.fsl
compiling to dfas (can take a while...)
3 states
writing output

$ ls
hello.fs  hello.fsl
```

## Question 3

```text
$ ls
FsLexYacc.Runtime.dll hello.fs              hello.fsl

$ fsharpc -r FsLexYacc.Runtime.dll hello.fs
Microsoft (R) F# Compiler version 11.0.0.0 for F# 5.0
Copyright (c) Microsoft Corporation. All Rights Reserved.

$ mono hello.exe
Hello World from FsLex!

Please pass a digit:
5
The lexer recognizes 5

$ mono hello.exe
Hello World from FsLex!

Please pass a digit:
321
The lexer recognizes 3
```

## Question 4

We simply added a `+` after `[0-9]`, assuming "more than 1 digit" still allows a single digit. See `hello2.fsl`

```text
$ ~/bin/fsharp/fslex --unicode hello2.fsl
compiling to dfas (can take a while...)
4 states
writing output

$ fsharpc -r FsLexYacc.Runtime.dll hello2.fs
Microsoft (R) F# Compiler version 11.0.0.0 for F# 5.0
Copyright (c) Microsoft Corporation. All Rights Reserved.

$ mono hello2.exe
Hello World from FsLex!

Please pass a digit:
1337
The lexer recognizes 1337
```

## Question 5

See our solution in `hello3.fsl` and our testing below.

```text
$ ~/bin/fsharp/fslex --unicode hello3.fsl
compiling to dfas (can take a while...)
10 states
writing output

$ fsharpc -r FsLexYacc.Runtime.dll hello3.fs
Microsoft (R) F# Compiler version 11.0.0.0 for F# 5.0
Copyright (c) Microsoft Corporation. All Rights Reserved.

$ mono hello3.exe
Hello World from FsLex!

Please pass a digit:
12.3
The lexer recognizes 12.3

$ mono hello3.exe
Hello World from FsLex!

Please pass a digit:
-4.1231
The lexer recognizes -4.1231

$ mono hello3.exe
Hello World from FsLex!

Please pass a digit:
+12456789.09876543
The lexer recognizes +12456789.09876543

$ mono hello3.exe
Hello World from FsLex!

Please pass a digit:
-.432
The lexer recognizes -.432

$ mono hello3.exe
Hello World from FsLex!

Please pass a digit:
4322
The lexer recognizes 4322

$ mono hello3.exe
Hello World from FsLex!

Please pass a digit:
.

Unhandled Exception:
System.Exception: Lexer error: illegal symbol
  at Hello_fslex.Tokenize (FSharp.Text.Lexing.LexBuffer`1[char] lexbuf) [0x0002d] in <631f8ce16b27cc8fa7450383e18c1f63>:0
  at Hello_fslex.main (System.String[] argv) [0x00034] in <631f8ce16b27cc8fa7450383e18c1f63>:0
[ERROR] FATAL UNHANDLED EXCEPTION: System.Exception: Lexer error: illegal symbol
  at Hello_fslex.Tokenize (FSharp.Text.Lexing.LexBuffer`1[char] lexbuf) [0x0002d] in <631f8ce16b27cc8fa7450383e18c1f63>:0
  at Hello_fslex.main (System.String[] argv) [0x00034] in <631f8ce16b27cc8fa7450383e18c1f63>:0

$ mono hello3.exe
Hello World from FsLex!

Please pass a digit:
12.
The lexer recognizes 12
```
