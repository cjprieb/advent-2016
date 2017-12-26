#load "TestResources.fs"

type Bridge = {id:int;port1:int;port2:int}
type LinkedBridge = {inPort:int;outPort:int}

let parse i (str:string) =
    let intArray = str.Split('/') |> Array.map (fun s -> s |> int)
    {id=i;port1=intArray.[0];port2=intArray.[1]}

let realInput = 
    TestResources.ReadAllLines("Day24.txt")
   |> List.mapi parse

let testInput = 
    [
        "0/2"
        "2/2"
        "2/3"
        "3/4"
        "3/5"
        "0/1"
        "10/1"
        "9/10"
    ] |> List.mapi parse

let createLink bridge outPort = 
    if bridge.port1 = outPort then
        {inPort=bridge.port1;outPort=bridge.port2}
    else
        {inPort=bridge.port2;outPort=bridge.port1}

let getNext bridges last remaining = 
    remaining
    |> List.filter (fun b -> b.port1 = last.outPort || b.port2 = last.outPort)
    |> List.map (fun b ->
        let link = createLink b last.outPort
        let remaining' = List.filter (fun b2 -> b2.id <> b.id) remaining
        (List.append bridges [link],remaining')
    )

let possibilities input =
    let rec loop bridge remaining =
        let last = List.last bridge
        let next = getNext bridge last remaining
        let bridges = 
            if (List.length next) > 0 then
                next |> List.map (fun (b,r) -> loop b r) |> List.concat
            else 
                [bridge]
        bridges
    let starting = [{inPort=0;outPort=0}]
    loop starting input

let strength bridges = bridges |> List.sumBy (fun b -> b.inPort + b.outPort)

// solve1 realInput;; 1868
let solve1 input = 
    let possibilities = possibilities input
    let best = possibilities |> List.maxBy strength
    printfn "Best bridge by strength: %A" best
    strength best
    
// solve2 realInput;; 1841
let solve2 input = 
    let possibilities = possibilities input
    let longestLength = possibilities |> List.map (List.length) |> List.max
    let best = possibilities |> List.filter (fun b -> List.length b = longestLength) |> List.maxBy strength
    printfn "Longest bridge: %i" (List.length best)
    strength best