open System.IO
open System
#load "TestResources.fs"

let input = File.ReadAllText("Advent2018\\Resources\\Day05.txt")

let testInput = [
        ("aA", "")
        ("abBA", "")
        ("abBAextratext", "extratext")
        ("abBAextratextabBA", "extratext")
        ("aAbBcCDdabBa", "aa")
        ("abBa", "aa")
        ("abAB", "abAB")
        ("aabAAB", "aabAAB")
        ("dabAcCaCBAcCcaDA", "dabCBAcaDA")
        ("dabAaCBAcCcaDA", "dabCBAcaDA")
        ("dabCBAcCcaDA", "dabCBAcaDA")
    ] 

// runTests test testInput;;

let sample = "dabAcCaCBAcCcaDA"
let zValue = ('z' |> int) + 1
let ZValue = ('Z' |> int) + 1

let normalize c = 
    if (c >= 'a' && c <= 'z') then
        zValue - (c |> int)
    else
        -1 * (ZValue - (c |> int))

let opposite (c1:char) (c2:char) = (-1 * normalize c1) = (normalize c2)

// let reduce ((newString:char list),(previousOpt:char option)) (current:char) =
//     match previousOpt with 
//     | None -> (newString, Some current)
//     | Some previous ->
//         if (opposite previous current) then
//             (newString, None)
//         else 
//             (List.append newString [previous], Some current)

// let reduceString input = 
//     let (result,prevOpt) = Seq.fold reduce ([],None) input
//     match prevOpt with
//     | None -> result
//     | Some prev -> List.append result [prev]

let toString str = new string(str |> List.toArray)

let validate str =
    if (List.length str % 2 = 1) then false
    else 
        let half = (List.length str / 2) 
        let matches = 
            [0..half-1]
            |> List.filter (fun i -> 
                let j = List.length str - i - 1
                // printfn "comparing %A with %A" i j
                opposite str.[i] str.[j]
            )
            |> List.length
        // printfn "expected %A matches, found %A" half matches
        matches = half

let validatePolymer input =
    let length = Seq.length input
    let filteredLength =
        input
        |> Seq.toList
        |> List.filter (fun c -> (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
        |> List.length
    length = filteredLength

let removeAt fromIndex toIndex str =
    let (part1,part2) = List.splitAt fromIndex str
    let (removed,part3) = List.splitAt (toIndex-fromIndex) part2
    // printfn "Removed from %A to %A in %A: %A" fromIndex toIndex (toString str) (toString removed)
    // let length = List.length removed
    // if (length > 4) then 
    // printfn "Removed %A characters (%A to %A)" removed fromIndex toIndex
    List.append part1 part3
    // if validate removed then
    //     List.append part1 part3
    // else 
    //     printfn "Invalid! %A characters (%A to %A)" removed fromIndex toIndex
    //     []

let reduceString2 input =
    let rec reduce current (fromIndex,toIndex) =
        let length = List.length current
        if (toIndex) >= (List.length current) || fromIndex < 0 then
            current
        else 
            let c1 = current.[fromIndex]
            let c2 = current.[toIndex]
            let (next',from',to') = 
                if opposite c1 c2 then 
                    if (fromIndex = 0) || (toIndex+1 >= length) then 
                        (removeAt (fromIndex) (toIndex+1) current),fromIndex-1,fromIndex
                    else
                        current,fromIndex-1,toIndex+1
                else if (fromIndex+1) = toIndex |> not then
                    (removeAt (fromIndex+1) (toIndex) current),fromIndex,fromIndex+1
                else
                    current,fromIndex+1,toIndex+1
            reduce next' (from',to')

    reduce input (0,1)

let test input = 
    let result = reduceString2 (input |> Seq.toList)
    new string(result |> List.toArray)

let runTests method input =
    input
    |> List.map (fun (input, expected) ->
        let actual = method input
        if (actual = expected) then printfn "Success! %A -> %A" input actual
        else printfn "Fail! Expected %A but %A was found" expected actual
    )

let rec processPolymer prevLength str =
    let length = Seq.length str
    // printfn "    Current length of polymer: %A" length
    if length = prevLength then
        str
    else
        let str' = reduceString2 str
        processPolymer length str'

// total milliseconds: 10611.034700 (10.6 seconds)
// Answer: 10598
let solve1 input =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let result = processPolymer 0 (input |> List.ofSeq)
    stopWatch.Stop()
    printfn "%f ms" stopWatch.Elapsed.TotalMilliseconds
    List.length result

let isMatch c1 c2 = ((c1 = c2) || (opposite c1 c2)) |> not

let removeAll c1 = List.filter (fun c2 -> isMatch c1 c2)

// total milliseconds: 245484.241500  (254 seconds)
// Answer: 5312
let solve2 input =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let seqInput = input |> List.ofSeq
    let mostMin =
        ['a'..'z']
        |> List.fold (fun (minC,min) c ->
            let cleanedInput = removeAll c seqInput
            // printfn "Cleaned input: %A" (toString cleanedInput)
            let result = processPolymer 0 cleanedInput
            let length = List.length result
            if (length < min) then (c,length) else (minC,min)
        ) ('a',List.length seqInput)
    stopWatch.Stop()
    printfn "%f ms" stopWatch.Elapsed.TotalMilliseconds
    mostMin


