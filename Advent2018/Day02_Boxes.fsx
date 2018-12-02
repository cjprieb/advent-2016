#load "TestResources.fs"

let input = 
    TestResources.ReadAllLines("Day02.txt")

let sample = [
    "abcdef"
    "bababc"
    "abbcde"
    "abcccd"
    "aabcdd"
    "abcdee"
    "ababab"
]

let sample2 = [
    "abcde"
    "fghij"
    "klmno"
    "pqrst"
    "fguij"
    "axcye"
    "wvxyz"
]

let anyWithExactly num letterMaps =
    Map.exists (fun _ value -> value = num) letterMaps

let count letterMaps =
    let hasTwo = anyWithExactly 2 letterMaps
    let hasThree = anyWithExactly 3 letterMaps
    (hasTwo, hasThree)

let parse str = 
    Seq.fold (fun mappings letter ->
        let count = if Map.containsKey letter mappings then mappings.[letter]+1 else 1
        Map.add letter count mappings
    ) Map.empty str

let ifIncrement total b = if b then total+1 else total
    
let checksum counts =
    let (totalTwo, totalThree) =        
        List.fold (fun (totalTwo, totalThree) (hasTwo, hasThree) ->
            ((ifIncrement totalTwo hasTwo), (ifIncrement totalThree hasThree))
        ) (0,0) counts
    totalTwo * totalThree

let solve1 input =
    input 
    |> List.map parse
    |> List.map count
    |> checksum

let areSimilar (str1:char list) (str2: char list) =
    let length = Seq.length str1
    let rec iterate i totalMisMatch =
        let i' = i+1
        let totalMisMatch' = 
            if str1.[i] = str2.[i] then totalMisMatch
            else totalMisMatch + 1

        if i' = length then totalMisMatch' = 1
        else if totalMisMatch' > 1 then false
        else iterate i' totalMisMatch'

    iterate 0 0

let commonLetters (str1:char list) (str2: char list) =
    let length = Seq.length str1
    List.fold (fun common i ->
        if str1.[i] = str2.[i] then List.append common [str1.[i]]
        else common
    ) [] [0..length-1]

let solve2 input =
    let length = List.length input
    let input' = input |> List.map (fun str -> Seq.toList str)
    let rec findSimilar i j =
        let str1 = input'.[i]
        let str2 = input'.[j]
        if areSimilar str1 str2 then
            Some (str1,str2)
        else
            let j' = (j+1) % length
            let i' = if j' = 0 then (i+1) else i
            if i' = length then
                None
            else 
                findSimilar i' j'

    let result = findSimilar 0 1
    match result with
        | None -> 
            printfn "None Found"
            ""
        | Some (str1, str2) ->
            printfn "Found: %A and %A" str1 str2
            new System.String(commonLetters str1 str2 |> List.toArray)