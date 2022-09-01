module Exercise_1_2

// -----------------------------------------------------------------------------
// -- Exercise 1.2
// -----------------------------------------------------------------------------

// (i)
type aexpr =
    | CstI of int
    | Var of string
    | Add of aexpr * aexpr
    | Mul of aexpr * aexpr
    | Sub of aexpr * aexpr

// (ii)
let ae1 = Sub(Var "v", Add(Var "w", Var "z")) // v − (w + z)
let ae2 = Mul(CstI 2, Sub(Var "v", Add(Var "w", Var "z"))) // 2 ∗ (v − (w + z))
let ae3 = Add(Var "x", Add(Var "y", Add(Var "z", Var "v"))) // x + y + z + v

// (iii)
let rec fmt =
    function
    | CstI x -> x.ToString()
    | Var v -> v
    | Add (lhs, rhs) -> sprintf "(%s + %s)" (fmt lhs) (fmt rhs)
    | Mul (lhs, rhs) -> sprintf "(%s * %s)" (fmt lhs) (fmt rhs)
    | Sub (lhs, rhs) -> sprintf "(%s - %s)" (fmt lhs) (fmt rhs)
