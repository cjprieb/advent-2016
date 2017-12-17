#load "TestResources.fs"

open System.Text.RegularExpressions

let Spin n dancers =
    //printfn "Spinning %i - %A" n dancers
    let skip = (Array.length dancers) - n
    let (first,last) = dancers |> Array.splitAt skip
    Array.append last first

let Exchange i j dancers =
    //printfn "Exchanging %i with %i - %A" i j dancers
    dancers
    |> Array.mapi (fun idx d -> 
        match idx with
        | x when x = i -> dancers.[j]
        | x when x = j -> dancers.[i]
        | _ -> d
    )

let Partner a b dancers =
    //printfn "Partnering %A with %A - %A" a b dancers
    dancers
    |> Array.map (fun d -> 
        match d with
        | x when x = b -> a
        | x when x = a -> b
        | _ -> d
    )

let Split s = s |> Array.ofSeq

let ParseMove move =
    let matches = Regex.Match (move, "([sxp])(\w+)(?:/(\w+))?")
    if matches.Success then
        let m = matches.Groups.[1].Value
        let a = matches.Groups.[2].Value
        let b = matches.Groups.[3].Value
        match m with 
        | "s" -> 
            Some (Spin (a |> int))
        | "x" ->
            Some (Exchange (a |> int) (b |> int))
        | "p" ->
            Some (Partner (a.Chars 0) (b.Chars 0))
        | _ ->
            printfn "%A doesn't match pattern" move
            None
    else
        printfn "%A doesn't match pattern" move
        None

let Parse (input:string) =
    input.Split(',')
    |> Seq.ofArray
    |> Seq.choose ParseMove

let Dance dancers moves = 
    Seq.fold (fun dancers move -> move dancers) dancers moves;

let actualDancers = "abcdefghijklmnop" |> Split
let moves = Parse (TestResources.ReadAllLines "Day16.txt").Head

let testDancers = "abcde" |> Split

// solve1 actualDancers;;//fgmobeaijhdpkcln
//Real: 00:00:00.022, CPU: 00:00:00.031, GC gen0: 3, gen1: 1, gen2: 0
//val it : string = "fgmobeaijhdpkcln"
let solve1 startingDancers =
    moves
    |> Dance startingDancers
    |> Array.map (fun d -> d.ToString())
    |> Array.reduce (+)

// solve2 actualDancers;; // lgmkacfjbopednhi
// Real: 00:00:00.577, CPU: 00:00:00.578, GC gen0: 70, gen1: 0, gen2: 0
// val it : string = "lgmkacfjbopednhi"
let solve2 startingDancers =
    let max = 1000000000
    let repeatsAt = 24
    let (_,all) =
        seq {1..repeatsAt}
        |> Seq.fold (fun (dancers,all) idx -> 
            let dancers' = Dance dancers moves
            let all' = Map.add (idx-1) dancers all
            (dancers',all')
        ) (startingDancers,Map.empty)
    let index = (max % repeatsAt)
    printfn "Getting dance at index %i" index
    let dancers = all.[index]
    dancers
    |> Array.map (fun d -> d.ToString())
    |> Array.reduce (+)