open System.Text.RegularExpressions
open System.Diagnostics
type Pattern = {pattern:char list;result:char}

let rules = [    
    "..### => #"
    "..... => ."
    "..#.. => ."
    ".###. => ."
    "...## => #"
    "#.### => ."
    "#.#.# => #"
    "##..# => ."
    "##.## => #"
    "#...# => ."
    "..##. => ."
    "##.#. => ."
    "...#. => ."
    "#..#. => #"
    ".#### => #"
    ".#..# => #"
    "##... => #"
    ".##.# => ."
    "....# => ."
    "#.... => ."
    ".#.#. => #"
    ".##.. => ."
    "###.# => #"
    "####. => ."
    "##### => #"
    "#.##. => #"
    ".#... => #"
    ".#.## => #"
    "###.. => #"
    "#..## => ."
    "#.#.. => #"
    "..#.# => ."
]

let input = rules,"###....#..#..#......####.#..##..#..###......##.##..#...#.##.###.##.###.....#.###..#.#.##.#..#.#"

let sampleRules = [
    "...## => #"
    "..#.. => #"
    ".#... => #"
    ".#.#. => #"
    ".#.## => #"
    ".##.. => #"
    ".#### => #"
    "#.#.# => #"
    "#.### => #"
    "##.#. => #"
    "##.## => #"
    "###.. => #"
    "###.# => #"
    "####. => #"
]

let sample = sampleRules,"#..#.#..##......###...###"

let extractNeighbors plants i = 
    [i-2..i+2] |> List.map (fun j -> 
        if Map.containsKey j plants then plants.[j] else '.'
    )

let count plants = 
    plants 
    |> Map.toList
    |> List.filter (fun (_,value) -> value = '#')
    |> List.map (fun (key,_) -> key)
    |> List.sum
    // let length = List.length plants
    // [0..(length-1)]
    // |> List.filter (fun i -> plants.[i] = '#')
    // |> List.map (fun i -> i-offset)
    // |> List.sum
    // //plants |> List.filter (fun p -> p = '#') |> List.length

let getMatch array rule = (rule.pattern = array)

let apply rule array = (if (rule.pattern = array) then Some rule.result else None)

let toList map = 
    map 
    |> Map.toList 
    |> List.map (fun (_,v) -> v)

let toString list = new string(list |> List.toArray)

let getBounds plants =
    let (minOpt,maxOpt) =
        plants
        |> Map.toList
        |> List.fold (fun (minOpt,maxOpt) (key,value) ->
            if value = '#' then
                let min' = 
                    match minOpt with
                    | Some x -> if key < x then key else x
                    | None -> key
                let max' = 
                    match maxOpt with
                    | Some x -> if key > x then key else x
                    | None -> key
                (Some min',Some max')
            else (minOpt,maxOpt)

        ) (None,None)
    match (minOpt,maxOpt) with
    | Some min,Some max -> min-2,max+2
    | _ -> 0,0

let rec generation rules plants (i:int64) =
    // if (i % 50000000L) = 0 then printfn "Generation %i" i
    let generationId = (50000000000L - i)
    if (i = 0L) then plants
    else 
        let (min,max) = getBounds plants
        let plants' =
            [min..max]
            |> List.fold (fun map index ->
                let neighbors = extractNeighbors plants index
                let matches = List.filter (getMatch neighbors) rules
                let result =
                    match matches with
                    | x::_ -> x.result
                    | [] -> '.'
                // if result = '#' then Map.add index result map else map
                Map.add index result map
            ) Map.empty

        let previous = toList plants
        let current = toList plants'
        let i' =
            if current = previous then 
                let sum = int64(count plants)
                let sum' = int64(count plants')
                let diff = sum' - sum
                let total = i * int64(diff)
                if (generationId = 191L) then
                    printfn "Expected %A" (sum + total)
                
                printfn "Generation %i: %A (Diff: %A)" generationId (sum') (diff)
                0L
            else 
                i - 1L
        generation rules plants' i'


let parse line =
    let isMatch = Regex.Match(line, "([\.#]+) => ([\.#])")
    if isMatch.Success then
        Some {
            pattern = isMatch.Groups.[1].Value |> Seq.toList
            result = isMatch.Groups.[2].Value.[0]
        }
    else 
        None

let parseRules lines = lines |> List.choose parse

// 2040
let solve1 (lines,initial) =
    let rules = parseRules lines
    let initialState = 
        initial
        |> Seq.mapi (fun i c -> (i,c))
        |> Seq.fold (fun map (i,c) -> Map.add i c map) Map.empty

    let endingState = generation rules initialState 20L 
    count endingState

// 1700000000011L is too low
let solve2 (lines,initial) =
    let stopwatch = Stopwatch.StartNew()
    let rules = parseRules lines
    let initialState = 
        initial
        |> Seq.mapi (fun i c -> (i,c))
        |> Seq.fold (fun map (i,c) -> Map.add i c map) Map.empty

    generation rules initialState 50000000000L |> ignore
    stopwatch.Stop()
    printfn "Elapsed time: %i" stopwatch.ElapsedMilliseconds