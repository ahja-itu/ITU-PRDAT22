module Tests

open Xunit
open Intro2

[<Fact>]
let ``Using max expression should find max of constants 1 and 2`` () =
    let ex = Prim("max", CstI 1, CstI 2)
    let res = eval ex []
    Assert.Equal(2, res)
