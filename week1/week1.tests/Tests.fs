module Tests

open Xunit
open Intro2

[<Theory>]
[<InlineData(1, 2, 2)>]
[<InlineData(3, 2, 3)>]
let ``max expression should max of constants a and b`` (a: int, b: int, expected: int) =
    let ex = Prim("max", CstI a, CstI b)
    let res = eval ex []
    Assert.Equal(expected, res)

[<Theory>]
[<InlineData(1, 2, 1)>]
[<InlineData(2, 0, 0)>]
[<InlineData(13, -13, -13)>]
let ``Using min expression should find min of constants`` (a, b, expected) =
    let ex = Prim("min", CstI a, CstI b)
    let res = eval ex []
    Assert.Equal(expected, res)

[<Theory>]
[<InlineData(1, 1, 1)>]
[<InlineData(1, 0, 0)>]
let ``== should determine if integers are equal`` (a: int, b: int, expected: int) =
    let exp = Prim("==", CstI a, CstI b)
    let res = eval exp []
    Assert.Equal(expected, res)
