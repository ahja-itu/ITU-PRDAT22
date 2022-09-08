module Exercise_PLC_2_4_Tests

open Xunit
open Exercise_PLC_2_4
open Intcomp1

[<Fact>]
let ``assemble yields same result as slides`` () =
    let assembly = assemble (scomp e1 [])
    let expected = [ 0; 17; 1; 0; 1; 1; 2; 6; 5 ]
    Assert.Equal<int list>(assembly, expected)
