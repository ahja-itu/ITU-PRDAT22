module Exercise_2

(* Taken from Intcomp1.fs but Let has been changed *)
type expr =
    | CstI of int
    | Var of string
    | Let of (string * expr) list * expr // Exercise 2.1
    | Prim of string * expr * expr

(* Taken directly from Intcomp1.fs *)
let rec lookup env x =
    match env with
    | [] -> failwith (x + " not found")
    | (y, v) :: r -> if x = y then v else lookup r x

(* Taken from Intcomp1.fs but handling of Let has been changed *)
let rec eval e (env: (string * int) list) : int =
    match e with
    | CstI i -> i
    | Var x -> lookup env x
    // Exercise 2.1 - start
    | Let (xs, ebody) ->
        List.fold (fun env' (x, erhs) -> (x, eval erhs env') :: env') env xs
        |> eval ebody
    // Exercise 2.1 - end
    | Prim ("+", e1, e2) -> eval e1 env + eval e2 env
    | Prim ("*", e1, e2) -> eval e1 env * eval e2 env
    | Prim ("-", e1, e2) -> eval e1 env - eval e2 env
    | Prim _ -> failwith "unknown primitive"

(* Taken directly from Intcomp1.fs *)
let rec mem x vs =
    match vs with
    | [] -> false
    | v :: vr -> x = v || mem x vr

(* Taken directly from Intcomp1.fs *)
let rec union (xs, ys) =
    match xs with
    | [] -> ys
    | x :: xr ->
        if mem x ys then
            union (xr, ys)
        else
            x :: union (xr, ys)

(* Taken directly from Intcomp1.fs *)
let rec minus (xs, ys) =
    match xs with
    | [] -> []
    | x :: xr ->
        if mem x ys then
            minus (xr, ys)
        else
            x :: minus (xr, ys)

(* Taken from Intcomp1.fs but handling of Let has been changed *)
let rec freevars e : string list =
    match e with
    | CstI i -> []
    | Var x -> [ x ]
    // Exercise 2.2 - start
    | Let (xs, ebody) ->
        List.foldBack (fun (x, erhs) acc -> union (freevars erhs, minus (acc, [ x ]))) xs (freevars ebody)
    // Exercise 2.2 - end
    | Prim (ope, e1, e2) -> union (freevars e1, freevars e2)
