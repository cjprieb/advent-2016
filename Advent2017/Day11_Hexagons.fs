//module Hexagons

module Hex =
    type Direction = N | NW | NE | S | SW | SE
    type Coordinate = {x:int;y:int;z:int}

    let parse s =
        match s with 
        | "n" -> Some N
        | "nw" -> Some NW
        | "ne" -> Some NE
        | "s" -> Some S
        | "sw" -> Some SW
        | "se" -> Some SE
        | _ -> printfn "Invalid string %s" s;None

    let add (c1:Coordinate) (c2:Coordinate) =
        {
            x = c1.x + c2.x
            y = c1.y + c2.y
            z = c1.z + c2.z
        }

    let move coord (d:Direction) =
        let moveDistance =
            match d with
            | N -> {x=0;y=1;z=(-1)}
            | NW -> {x=(-1);y=1;z=0}
            | NE -> {x=1;y=0;z=(-1)}
            | S -> {x=0;y=(-1);z=1}
            | SW -> {x=(-1);y=0;z=1}
            | SE -> {x=1;y=(-1);z=0}
        add coord moveDistance

    let distance coord1 coord2 =
        [coord2.x-coord1.x;coord2.y-coord1.y;coord2.z-coord1.z]
        |> List.map (fun i -> if i < 0 then (-1)*i else i)
        |> List.max
            
let test testMethod expected input =
    let answer = testMethod input
    if answer = expected then
        printfn "Success! Result was %A and %A was expected" answer expected
    else
        printfn "Failed! Result was %A, but %A was expected" answer expected

// How far is he from the start after all the movements are done
let solvePart1 (input:string,expected,_) =
    let movement = 
        input.Split(',')
        |> Array.map Hex.parse
        |> Array.choose (fun move -> move)
    let startCoord = {Hex.x=0;Hex.y=0;Hex.z=0}
    let distanceFromStart = Hex.distance startCoord
    let endCoord = Array.fold Hex.move startCoord movement
    test distanceFromStart expected endCoord

// How far is he from the start after all the movements are done
let solvePart2 (input:string,expectedEndDistance,expectedMaxDistance) =
    let movement = 
        input.Split(',')
        |> Array.map Hex.parse
        |> Array.choose (fun move -> move)
    let startCoord = {Hex.x=0;Hex.y=0;Hex.z=0}
    let distanceFromStart = Hex.distance startCoord
    let (endCoord,maxDistance) = 
        Array.fold (fun (coord,maxDistance) d ->
            let coord' = Hex.move coord d
            let maxDistance' = List.max [maxDistance;(distanceFromStart coord')]
            (coord',maxDistance')
        ) (startCoord,0) movement
    test distanceFromStart expectedEndDistance endCoord
    test (fun x -> x) expectedMaxDistance maxDistance
        
let testCases =
    [
        "ne,ne,ne",3,3
        "ne,ne,sw,sw",0,2
        "ne,ne,s,s",2,2
        "se,sw,se,sw,sw,n",2,3
    ]

let solutionInput =
    let path = @"C:\Users\priebc\Documents\Visual Studio 2017\Projects\FSharpTestLibrary\FSharpTestLibrary\HexagonsInput.txt"
    System.IO.File.ReadAllText(path).Trim()
// solvePart1 (solutionInput,696,1461);;
// solvePart2 (solutionInput,696,1461);;

//ne,ne,ne is 3 steps away.
//ne,ne,sw,sw is 0 steps away (back where you started).
//ne,ne,s,s is 2 steps away (se,se).
//se,sw,se,sw,sw is 3 steps away (s,s,sw).
