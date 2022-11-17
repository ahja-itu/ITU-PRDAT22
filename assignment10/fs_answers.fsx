
// Exercise 10.1 I
let rec lenc lst k =
    match lst with
    | []      -> k 0
    | _ :: xs -> lenc xs (fun v -> k(v + 1))


// Exercise 10.1 III
let rec lenny lst acc =
    match lst with
    | [] -> acc
    | _ :: xs -> lenny xs ((+) acc 1)

// Exercise 10.2 I
let rec revc lst k =
    match lst with
    | []      -> k []
    | x :: xs -> revc xs (fun vs -> k(vs @ [x]))

// Exercise 10.2 III
let rec revi lst acc =
    match lst with
    | []      -> acc
    | x :: xs -> revi xs (x :: acc)
    