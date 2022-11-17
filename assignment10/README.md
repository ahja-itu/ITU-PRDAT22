# Assignment 8

Authors: adbo, aljb and ahja

This code can also be found online at <https://github.com/andreaswachs/ITU-PRDAT22>.

We recommend reading this Markdown page on [GitHub](https://github.com/andreaswachs/ITU-PRDAT22/blob/main/assignment10/README.md) or VSCode because it makes the diffs easier to read.

## Solutions

### 10.1

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