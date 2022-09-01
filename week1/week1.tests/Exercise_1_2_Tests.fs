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
