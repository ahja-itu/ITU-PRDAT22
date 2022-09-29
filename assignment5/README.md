# Assignment 5

Authors: adbo, aljb and ahja

This code can also be found online at <https://github.com/andreaswachs/ITU-PRDAT22>.

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
