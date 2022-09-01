module Exercise_1_2_Tests

open Xunit
open Exercise_1_2

[<Fact>]
let ``fmt formats string correctly`` () =
    let ae1 = Sub(Var "v", Add(Var "w", Var "z")) // v - (w + z)
    let ae2 = Mul(CstI 2, Sub(Var "v", Add(Var "w", Var "z"))) // 2 * (v - (w + z))
    let ae3 = Add(Var "x", Add(Var "y", Add(Var "z", Var "v"))) // x + y + z + v

    Assert.Equal(fmt ae1, "(v - (w + z))")
    Assert.Equal(fmt ae2, "(2 * (v - (w + z)))")
    Assert.Equal(fmt ae3, "(x + (y + (z + v)))")

[<Fact>]
let ``simplify simplifies addition correctly`` () =
    let ae1 = Add(Var "x", Var "y")
    let ae2 = Add(Var "x", CstI 0)
    let ae3 = Add(Var "x", CstI 1)
    let ae4 = Add(CstI 0, Var "y")
    let ae5 = Add(CstI 1, Var "y")
    let ae6 = Add(CstI 2, CstI 2)

    Assert.Equal(simplify ae1, Add(Var "x", Var "y"))
    Assert.Equal(simplify ae2, Var "x")
    Assert.Equal(simplify ae3, Add(Var "x", CstI 1))
    Assert.Equal(simplify ae4, Var "y")
    Assert.Equal(simplify ae5, Add(CstI 1, Var "y"))
    Assert.Equal(simplify ae6, CstI 4)


[<Fact>]
let ``simplify simplifies subtraction correctly`` () =
    let ae1 = Sub(Var "x", Var "y")
    let ae2 = Sub(Var "x", CstI 0)
    let ae3 = Sub(Var "x", CstI 1)
    let ae4 = Sub(CstI 0, Var "y")
    let ae5 = Sub(CstI 1, Var "y")
    let ae6 = Sub(CstI 2, CstI 2)

    Assert.Equal(simplify ae1, Sub(Var "x", Var "y"))
    Assert.Equal(simplify ae2, Var "x")
    Assert.Equal(simplify ae3, Sub(Var "x", CstI 1))
    Assert.Equal(simplify ae4, Sub(CstI 0, Var "y"))
    Assert.Equal(simplify ae5, Sub(CstI 1, Var "y"))
    Assert.Equal(simplify ae6, CstI 0)

[<Fact>]
let ``simplify simplifies multiplication correctly`` () =
    let ae1 = Mul(Var "x", Var "y")
    let ae2 = Mul(Var "x", CstI 0)
    let ae3 = Mul(Var "x", CstI 1)
    let ae4 = Mul(Var "x", CstI 2)
    let ae5 = Mul(CstI 0, Var "y")
    let ae6 = Mul(CstI 1, Var "y")
    let ae7 = Mul(CstI 2, Var "y")
    let ae8 = Mul(CstI 2, CstI 2)

    Assert.Equal(simplify ae1, Mul (Var "x", Var "y"))
    Assert.Equal(simplify ae2, CstI 0)
    Assert.Equal(simplify ae3, Var "x")
    Assert.Equal(simplify ae4, Mul(Var "x", CstI 2))
    Assert.Equal(simplify ae5, CstI 0)
    Assert.Equal(simplify ae6, Var "y")
    Assert.Equal(simplify ae7, Mul(CstI 2, Var "y"))
    Assert.Equal(simplify ae8, CstI 4)
