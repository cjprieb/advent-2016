//module Day12_DigitalPlumber

open System.Text.RegularExpressions

type Program = Program of int
type Pipe = {input:Program;output:Program list}

let parseLine line =
    let matches = Regex.Match(line, @"(\d+) <-> ([\d, ]+)")
    if matches.Success then
        let input = matches.Groups.[1].Value |> int |> Program
        let output =
            matches.Groups.[2].Value.Split(',')
            |> Array.map (fun s -> s.Trim() |> int |> Program)
            |> List.ofArray
        Some {input=input;output=output}
    else 
        None

let toKeyValuePair (pipe:Pipe) = (pipe.input,pipe)
 
let parseLines input = 
    input 
    |> List.choose parseLine
    |> List.map toKeyValuePair
    |> dict

let solve1 input max =
    let pipeDict = parseLines input

    let rec countConnections state (pipeList:Program list) =
        let (count,processed) = state
        match pipeList with
        | [] -> 
            //printfn "pipe list is empty"
            state
        | x::xs ->
            let isUnprocessed program = Set.contains program processed |> not
            let newPipes = pipeDict.[x].output |> List.filter isUnprocessed
            let newProcessed = List.fold (fun pr pi -> Set.add pi pr) processed newPipes
            let newCount = count + (List.length newPipes)
            let newList = List.append xs newPipes
            //printfn "%i programs connected, %A processed, %A to process" newCount x newList
            countConnections (newCount,newProcessed) newList

    let countGroups state program =
        let (count,proccessed) = state
        if Set.contains program proccessed then
            //printfn "%A is already part of a group (count=%i)" program count
            state
        else
            //printfn "Processing new group for %A (count=%i)" program count
            let newCount = count + 1
            let (_,newProcessed) = countConnections (0, proccessed) [program]
            (newCount,newProcessed)
    
    let (count,processed) = countConnections (0, Set.empty) [Program 0]
    printfn "%i programs connected to 0" count // real answer is 145

    let allPrograms = [0..max] |> List.map Program
    let (groupCount,_) = List.fold countGroups (0,Set.empty) allPrograms
    printfn "%i groups connected" groupCount // real answer is 207

let testInput = [
    "0 <-> 2"
    "1 <-> 1"
    "2 <-> 0, 3, 4"
    "3 <-> 2, 4"
    "4 <-> 2, 3, 6"
    "5 <-> 6"
    "6 <-> 4, 5"
]


let realInput = TestResources.ReadAllLines "Day12.txt"