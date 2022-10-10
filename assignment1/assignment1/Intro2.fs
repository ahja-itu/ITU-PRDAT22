(* Programming language concepts for software developers, 2010-08-28 *)

(* Evaluating simple expressions with variables *)

module Intro2

(* Association lists map object language variables to their values *)

let env =
    [ ("a", 3)
      ("c", 78)
      ("baf", 666)
      ("b", 111) ]

let emptyenv = [] (* the empty environment *)

let rec lookup env x =
    match env with
    | [] -> failwith (x + " not found")
    | (y, v) :: r -> if x = y then v else lookup r x

let cvalue = lookup env "c"


(* Object language expressions with variables *)

type expr =
    | CstI of int
    | Var of string
    | Prim of string * expr * expr
    | If of expr * expr * expr // Exercise 1.1.v

let e1 = CstI 17
let e2 = Prim("+", CstI 3, Var "a")
let e3 = Prim("+", Prim("*", Var "b", CstI 9), Var "a")


(* Evaluation within an environment *)

// Used as helper for exercise 1.1.v
let eq a b = if a = b then 1 else 0

let rec eval e (env: (string * int) list) : int =
    match e with
    | CstI i -> i
    | Var x -> lookup env x
    // Exercise 1.1.{i -> v}
    | Prim (op, e1, e2) ->
        let res1 = eval e1 env
        let res2 = eval e2 env

        match op with
        | "+" -> (+) res1 res2
        | "*" -> (*) res1 res2
        | "-" -> (-) res1 res2
        | "max" -> max res1 res2
        | "min" -> min res1 res2
        | "==" -> eq res1 res2
        | _ -> failwith "unknown primitive"
    | If (e1, e2, e3) ->
        if eval e1 env <> 0 then
            eval e2 env
        else
            eval e3 env

let e1v = eval e1 env
let e2v1 = eval e2 env
let e2v2 = eval e2 [ ("a", 314) ]
let e3v = eval e3 env
