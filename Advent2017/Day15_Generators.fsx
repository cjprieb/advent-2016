open System.Numerics

let nextValue (factor:int) (previous:int) =
    let product = (BigInteger previous) * (BigInteger factor)
    (product % BigInteger 2147483647)|> int

let nextValue_picky (factor:int) (multiple:int) (previous:int) =
    let rec picky previous =
        let next = nextValue factor previous
        if (next % multiple) = 0 then
            next
        else 
            picky next
    picky previous

let generatorA previous = nextValue 16807 previous

let generatorB previous = nextValue 48271 previous

let generatorA_picky previous = nextValue_picky 16807 4 previous

let generatorB_picky previous = nextValue_picky 48271 8 previous

let getLast16Bits x = x &&& 0xffff

let isMatch a b = (getLast16Bits a) = (getLast16Bits b)

let testValues = [
    65,8921
    1092455,430625591
    1181022009,1233683848
    245556042,1431495498
    1744312007,137874439
    1352636452,285222916
]

let testNextValues =
    let results =
        testValues
        |> List.map (fun (a,b) ->
            let aNext = generatorA a
            let bNext = generatorB b
            aNext,bNext
        )
        |> List.take ((List.length testValues)-1)
    let combi = 
        testValues
        |> List.skip 1
        |> List.zip results
    List.iter (fun ((actA,actB),(expA,expB))->        
        if expA = actA && expB = actB then
            let matching = isMatch actA actB
            printfn "Success! results were A=%A B=%A isMatch=%A" actA actB matching
        else
            printfn "Failed! Results were A=%A and B=%A, but A=%A and B=%A were expected" actA actB expA expB
    ) combi

// solve1 65 8921;; // 588
// solve1 591 393;; // 619
//Real: 00:00:09.171, CPU: 00:00:09.157, GC gen0: 763, gen1: 763, gen2: 0
//val it : int * int * int = (619, 1559061521, 290823326)
let solve1 startA startB =
    let max = 40000000
    seq { 1..max }
    |> Seq.fold (fun (sum,a,b) i ->
        //if (i % 100000) = 0 then printfn "%i A=%A B=%A sum=%i" i a b sum
        let a' = generatorA a
        let b' = generatorB b
        let sum' = sum + (if isMatch a' b' then 1 else 0)
        (sum',a',b')
    ) (0,startA,startB)

    
// solve2 65 8921;; // 309
// solve2 591 393;; // 290
//Real: 00:00:06.313, CPU: 00:00:06.302, GC gen0: 334, gen1: 334, gen2: 0
//val it : int * int * int = (290, 2044281072, 180222328)
let solve2 startA startB =
    let max = 5000000
    seq { 1..max }
    |> Seq.fold (fun (sum,a,b) i ->
        if (i % 100000) = 0 then printfn "%i A=%A B=%A sum=%i" i a b sum
        let a' = generatorA_picky a
        let b' = generatorB_picky b
        let sum' = sum + (if isMatch a' b' then 1 else 0)
        (sum',a',b')
    ) (0,startA,startB)