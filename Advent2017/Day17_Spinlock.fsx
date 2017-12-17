let Step idx increment list = (idx + increment) % (List.length list) + 1

let Update idx iteration list = 
    let (first,second) = List.splitAt idx list
    List.concat [first;[iteration];second]

let Evolve increment (idx,list) iteration =
    let nextStep = Step idx increment list
    let list' = Update nextStep iteration list
    if nextStep = 1 then printfn "%i: %i" nextStep iteration
    (nextStep,list')

let Step2 idx increment iteration = (idx + increment) % (iteration) + 1

let Evolve2 increment (idx,one) value =
    let nextStep = Step2 idx increment (value)
    let one' = 
        if nextStep = 1 then 
            printfn "%i: %i" nextStep value
            value
        else
            one
    if (value % 1000000) = 0 then printfn "#%i: %i" nextStep value
    (nextStep,one')

let testCases = [
    0,0,[0]
    1,1,[0;1]
    2,1,[0;2;1]
    3,2,[0;2;3;1]
    4,2,[0;2;4;3;1]
    5,1,[0;5;2;4;3;1]
    6,5,[0;5;2;4;3;6;1]
]

let testOne increment prev curr =
    let (_,idx,input) = prev
    let (iteration,insertAt,expected) = curr
    let (actualIdx, actualList) = Evolve increment (idx,input) iteration
    if actualList = expected && actualIdx = insertAt then
        printfn "Success! %i was inserted at %i: %A" iteration actualIdx actualList
    else
        printfn "Failed! %i was inserted at %i: %A, but %i and %A were expected" iteration actualIdx actualList insertAt expected

let test =
    testCases
    |> List.windowed 2
    |> List.map (fun x -> testOne 3 x.Head (List.last x))    

// solve1 3;; 638
// solve1 301;; 1642
//Real: 00:00:00.021, CPU: 00:00:00.031, GC gen0: 10, gen1: 1, gen2: 0
//val it : int = 1642
//1: 1
//1: 2
//1: 4
//1: 10
//1: 17
//1: 131
let solve1 increment =
    let evolveIncrement = Evolve increment
    let (idx,list)=
        [1..2017]
        |> List.fold evolveIncrement (0,[0])
    printfn "%A" list
    list.[idx+1]
    
// solve2 301;; 33601318
//Real: 00:00:08.053, CPU: 00:00:08.031, GC gen0: 700, gen1: 127, gen2: 2
//val it : int = 33601318
let solve2 increment =
    let max = 50000000
    let evolveIncrement = Evolve2 increment
    let (_,one) =
        [1..max]
        |> List.fold evolveIncrement (0,0)
    one