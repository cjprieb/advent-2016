#load "TestResources.fs"

open System.Text.RegularExpressions

let pattern = new Regex("([\\+\\-])(\\d+)")

let parse (str:string) =
    let aMatch = pattern.Match(str)
    let sign = aMatch.Groups.[1].Value
    let num = aMatch.Groups.[2].Value |> int
    if sign = "-" then -num else num

let input = 
    TestResources.ReadAllLines("Day01.txt")
    |> List.map parse

let solve1 (input:int List) = 
    let result = input |> List.sum
    printfn "Resulting frequency is %A" result
    
let isDuplicate frequencies freq =
    Map.containsKey freq frequencies

let solve2 (input:int List) =
    let rec loop (frequencies,currFreq,i) =
        let nextFreq = currFreq + input.[i]
        if isDuplicate frequencies nextFreq then
            nextFreq
        else 
            let nextIndex = (i+1) % input.Length
            let newFrequencies = Map.add (nextFreq) 1 frequencies
            loop(newFrequencies,nextFreq,nextIndex)

    let result = loop(Map.empty,0,0)
    printfn "Duplicate frequency is %A" result