module Exercise_PLC_2_4

open Intcomp1

let sinstrToInt =
    function
    | SCstI x -> [ 0; x ]
    | SVar x -> [ 1; x ]
    | SAdd -> [ 2 ]
    | SSub -> [ 3 ]
    | SMul -> [ 4 ]
    | SPop -> [ 5 ]
    | SSwap -> [ 6 ]

let assemble instrs =
    List.foldBack (fun instr assembly -> sinstrToInt instr @ assembly) instrs []
