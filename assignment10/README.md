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