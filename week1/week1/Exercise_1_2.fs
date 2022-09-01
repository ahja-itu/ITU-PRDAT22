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

// (iv)
let rec simplify =
    function
    | Add (CstI 0, rhs) -> rhs
    | Add (lhs, CstI 0) -> lhs
    | Add (CstI x, CstI y) -> CstI (x + y)
    | Sub (lhs, CstI 0) -> lhs
    | Sub (lhs, rhs) when lhs = rhs -> CstI 0
    | Sub (CstI x, CstI y) -> CstI (x - y)
    | Mul (CstI 1, rhs) -> rhs
    | Mul (lhs, CstI 1) -> lhs
    | Mul (CstI 0, _)
    | Mul (_, CstI 0) -> CstI 0
    | Mul (CstI x, CstI y) -> CstI (x * y)
    | aExpr -> aExpr
