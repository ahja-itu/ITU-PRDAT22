module merge

let rec merge xs ys =
    match xs, ys with
    | [], _ -> ys
    | _, [] -> xs
    | x::xs, y::ys when x < y ->
        x :: merge xs (y::ys)
    | x::xs, y::ys ->
        y :: merge (x::xs) ys
