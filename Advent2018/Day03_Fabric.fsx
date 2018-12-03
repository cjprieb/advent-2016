open System.IO
open System.Text.RegularExpressions

type Id = Id of string
type Coordinate = {Left:int;Top:int}
type Dimension = {Width:int;Height:int}

type Claim = {
    ClaimId:Id
    Location:Coordinate
    Size:Dimension
}

let parse str =
    let aMatch = Regex.Match(str, "#(\d+) @ (\d+),(\d+): (\d+)x(\d+)")
    if aMatch.Success then
        let claim = aMatch.Groups.[1].Value |> Id
        let left = aMatch.Groups.[2].Value |> int
        let top = aMatch.Groups.[3].Value |> int
        let width = aMatch.Groups.[4].Value |> int
        let height = aMatch.Groups.[5].Value |> int
        Some {
            ClaimId = claim
            Location = { Left=left; Top=top }
            Size = { Width=width; Height=height }
        }
    else
        None
      
let sample =  [|"#1 @ 1,3: 4x4";"#2 @ 3,1: 4x4";"#3 @ 5,5: 2x2"|]  |> Array.choose parse

let input = 
    File.ReadAllLines("AdventOfCode\\Day03.txt")
    |> Array.choose parse

let claimPoint id x y (claimed,overlapping) =
    let coord = {Left=x;Top=y}
    let value = if Map.containsKey coord claimed then claimed.[coord] else []
    let value' = id::value
    // let value = if Map.containsKey coord fabric then fabric.[coord] else 0
    // let value' = value + 1
    let claimed' = Map.add coord value' claimed
    let overlapping' = 
        if (List.length value) = 1 then
            Map.add id true (Map.add value.[0] true overlapping)
        else if (List.length value) > 1 then
            Map.add id true overlapping
        else 
            overlapping

    (claimed',overlapping')

let claimRow id columns x fabric =
    List.fold (fun fabric' y -> claimPoint id x y fabric') fabric columns

let stakeClaim (claimed,overlapping) claim =
    let x_start = claim.Location.Left
    let x_end = x_start + claim.Size.Width - 1
    let rows = [x_start..x_end]

    let y_start = claim.Location.Top
    let y_end = y_start + claim.Size.Height - 1
    let columns = [y_start..y_end]

    let overlapping' = Map.add claim.ClaimId false overlapping
    List.fold (fun fabric' x -> claimRow claim.ClaimId columns x fabric') (claimed,overlapping') rows

let countOverlap fabric =
    Map.fold (fun totalOverlap coord claims ->
        if (List.length claims) > 1 then totalOverlap+1 else totalOverlap
    ) 0 fabric
        
let solve1 input =
    let (claimedFabric,_) =
        Array.fold stakeClaim (Map.empty,Map.empty) input
    let total = countOverlap claimedFabric
    printfn "Total inches overlapping is %A" total

let findFirstNonOverlapping fabric =
    let ids = 
        fabric
        |> Map.toList
        |> List.filter (fun (id,value) -> 
            // printfn "Is claim %A overlapping another? %A" id value
            value |> not
        )
    ids.[0]

let solve2 input =
    let (_,overlappingFabric) = Array.fold stakeClaim (Map.empty,Map.empty) input
    let id = findFirstNonOverlapping overlappingFabric
    printfn "Claim with no overlap is %A" id
