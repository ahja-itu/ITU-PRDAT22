
// Exercise 10.1
let rec lenc lst k =
    match lst with
    | []      -> k 0
    | _ :: xs -> lenc xs (fun v -> k(v + 1))