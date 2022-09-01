module Tests

open Xunit
open Intro2

[<Fact>]
let ``Using max expression should find max of constants 1 and 2`` () =
    let ex = Prim("max", CstI 1, CstI 2)
    let res = eval ex []
    Assert.Equal(2, res)

[<Theory>]
[<InlineData(1, 2, 1)>]  
[<InlineData(2, 0, 0)>] 
[<InlineData(13, -13, -13)>] 
let ``Using min expression should find min of constants`` (a, b, expected) =
    let ex = Prim("min", CstI a, CstI b)
    let res = eval ex []
    Assert.Equal(expected, res)
