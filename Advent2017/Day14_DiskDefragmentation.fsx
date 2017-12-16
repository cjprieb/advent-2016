open System

#load "KnotHashing.fsx"

let charToBinary (ch:char) =
    let binaryStr =
        match ch with 
        | '0' -> "0000"
        | '1' -> "0001"
        | '2' -> "0010"
        | '3' -> "0011"
        | '4' -> "0100"
        | '5' -> "0101"
        | '6' -> "0110"
        | '7' -> "0111"
        | '8' -> "1000"
        | '9' -> "1001"
        | 'a' -> "1010"
        | 'b' -> "1011"
        | 'c' -> "1100"
        | 'd' -> "1101"
        | 'e' -> "1110"
        | 'f' -> "1111"
        | _ -> ""
    binaryStr |> Seq.map (fun x -> if x = '1' then 1 else 0) |> List.ofSeq

let add (x',y') (x,y) = (x+x',y+y')

let includeCoordinates (grid:int [,]) (x,y) =
    if x >= 0 && y >= 0 && x < 128 && y < 128 then 
        grid.[x,y] = 1
    else
        false

let surrounding grid (x,y) = 
    [(0,1);(1,0);(0,-1);(-1,0)]
    |> List.map (add (x,y))
    |> List.filter (fun coord -> includeCoordinates grid coord)

let addCoord processed (x,y) = Map.add (x,y) 1 processed

let addAllInGroup grid inGroup1 (x,y) = 
    let rec addRec (inGroup:Map<int*int,int>) coordList = 
        match coordList with
        | [] ->
            inGroup
        | (x,y)::xs ->
            //printf "  adding surroundings for %ix%i" x y
            let boxes = surrounding grid (x,y)
            let (_,unprocessed) = boxes |> List.partition inGroup.ContainsKey
            match unprocessed with
            | [] ->
                printfn ""
                addRec inGroup xs
            | _ -> 
                let inGroup' = ((x,y)::unprocessed) |> List.fold addCoord inGroup
                let list' = List.append xs unprocessed
                //printfn " - %A" list'
                addRec inGroup' list'

    addRec inGroup1 [x,y]

let countGroups (grid:int [,]) =
    let processCoord (sum,inGroup:Map<int*int,int>) (x,y) =
        if inGroup.ContainsKey (x,y) || grid.[x,y] = 0 then
            (sum,inGroup)
        else 
            //printfn "%ix%i not processed" x y
            let inGroup' = addAllInGroup grid inGroup (x,y)
            //printfn "  %A" (inGroup' |> Map.toList |> List.map fst)
            (sum+1,inGroup')
            //let boxes = surrounding grid (x,y)
            //let processed = boxes |> List.filter inGroup.ContainsKey
            //let inGroup' = addCoord inGroup (x,y)
            //match processed with
            //| [] -> 
            //    printfn "  adding new group"
            //    (sum+1,inGroup')
            //| _ ->
            //    printfn "  in existing group"
            //    (sum,inGroup')

    let allCoord = seq {
        for x in [0..127] do
            //if x < 20 then printfn ""
            for y in [0..127] do 
                //if x < 20 && y < 20 then printf "\t%i" grid.[x,y]
                yield x,y
    }
    let (sum, _) = allCoord |> Seq.fold processCoord (0,Map.empty)
    sum

let toBinary (input:string) = 
    input
    |> Seq.map charToBinary
    |> Seq.concat
    |> Array.ofSeq

let countSpacedUsed input = 
    input 
    |> Array.Parallel.map (fun row -> row |> Array.sum)
    |> Array.sum

// unpackHashes "flqrgnkx";; // 8108 used,1242 regions
// unpackHashes "hxtvlmkl";; // 8214 used,1093 regions
let unpackHashes input =
    let diskRepresentation =
        [|0..127|]
        |> Array.map (fun idx -> input + "-" + idx.ToString())
        |> Array.Parallel.map (fun x -> (KnotHashing.Knot.hash x) |> toBinary)
    let used = countSpacedUsed diskRepresentation
    
    let grid = Array2D.init 128 128 (fun x y -> diskRepresentation.[x].[y])
    //printfn "%A" grid
    let groups = countGroups grid
    (used,groups)