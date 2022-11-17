
// Exercise 11.1 I
let rec lenc lst k =
    match lst with
    | []      -> k 0
    | _ :: xs -> lenc xs (fun v -> k(v + 1))


// Exercise 11.1 III
let rec lenny lst acc =
    match lst with
    | [] -> acc
    | _ :: xs -> lenny xs ((+) acc 1)

// Exercise 11.2 I
let rec revc lst k =
    match lst with
    | []      -> k []
    | x :: xs -> revc xs (fun vs -> k(vs @ [x]))

// Exercise 11.2 III
let rec revi lst acc =
    match lst with
    | []      -> acc
    | x :: xs -> revi xs (x :: acc)
    
// Exercise 11.3
let rec prodc lst k =
    match lst with
    | [] -> k 1
    | x :: xs -> prodc xs (fun v -> k(x * v))

// Exercise 11.4
let rec prodc' lst k =
    let rec aux lst' k' =
        match lst' with
        | [] -> k' 1
        | x :: _ when x = 0 -> k 0
        | x :: xs -> aux xs (fun v -> k'(x * v))

    aux lst k

let rec prodi lst acc =
    match lst with
    | [] -> acc
    | x :: _ when x = 0 -> 0
    | x :: xs -> prodi xs (acc * x)