
type Coord = {x:int;y:int}

type NearestType = Id of int | Multiple

type Point = {
    point:Coord
    id:int option
    nearest:NearestType
}

let input = 
    [
        (336, 308)
        (262, 98)
        (352, 115)
        (225, 205)
        (292, 185)
        (166, 271)
        (251, 67)
        (266, 274)
        (326, 85)
        (191, 256)
        (62, 171)
        (333, 123)
        (160, 131)
        (211, 214)
        (287, 333)
        (231, 288)
        (237, 183)
        (211, 272)
        (116, 153)
        (336, 70)
        (291, 117)
        (156, 105)
        (261, 119)
        (216, 171)
        (59, 343)
        (50, 180)
        (251, 268)
        (169, 258)
        (75, 136)
        (305, 102)
        (154, 327)
        (187, 297)
        (270, 225)
        (190, 185)
        (339, 264)
        (103, 301)
        (90, 92)
        (164, 144)
        (108, 140)
        (189, 211)
        (125, 157)
        (77, 226)
        (177, 168)
        (46, 188)
        (216, 244)
        (346, 348)
        (272, 90)
        (140, 176)
        (109, 324)
        (128, 132)
    ]

let sample = 
    [
        (1, 1)
        (1, 6)
        (8, 3)
        (3, 4)
        (5, 5)
        (8, 9)
    ]

let sample2 =
    [
        (5,5)
        (25,25)
        (5,25)
        (25,5)
        (15,15)
    ]

let getEdges coordinates =
    let top = List.minBy (fun a -> a.y) coordinates
    let left = List.minBy (fun a -> a.x) coordinates
    let bottom = List.maxBy (fun a -> a.y) coordinates
    let right = List.maxBy (fun a -> a.x) coordinates
    (top.y,left.x,bottom.y,right.x)

let getCenter (top,left,bottom,right) = 
    let y = top + ((bottom - top) / 2)
    let x = left + ((right - left) / 2)
    {x=x;y=y}

let distance a b = System.Math.Abs(a.x - b.x) + System.Math.Abs(a.y - b.y)

let getNearest a coordinates =
    let distances = 
        coordinates 
        |> List.map (fun b -> (b,distance a b)) 
        |> List.sortBy (fun (_,d) -> d)
    let (minCoord,minDistance) = distances.[0]
    let nearestCoords = List.takeWhile (fun (_,d) -> d = minDistance) distances
    if (List.length nearestCoords) > 1 then
        None
    else
        Some minCoord

let applyOffset center (x,y) = { x = center.x + x; y = center.y + y } 

// printList (getRing {x=2;y=2} 2);;
// let coordinates = sample2 |> List.map (fun (x,y) -> {x=x;y=y});;
// let p = {x=5;y=0};;

let getRing center ringIndex =
    if (ringIndex = 0) then [center]
    else
        let offsets = [-ringIndex..ringIndex]         
        offsets 
        |> List.map (fun i -> applyOffset center (i,-ringIndex)) 
        |> List.append (offsets |> List.map (fun i -> applyOffset center (-ringIndex,i)))
        |> List.append (offsets |> List.map (fun i -> applyOffset center (i,ringIndex)))
        |> List.append (offsets |> List.map (fun i -> applyOffset center (ringIndex,i)))
        |> List.distinct

let printList list =
    List.map (fun coord -> printfn "(%A,%A)" coord.x coord.y) list

let buildMap input = 
    let coordinates = input |> List.map (fun (x,y) -> {x=x;y=y})
    let edges = getEdges coordinates
    let center = getCenter edges
    let coordinateIdMap = 
        [1..List.length input]
        |> List.fold (fun m i ->
            let coord = coordinates.[i-1]
            Map.add coord i m
        ) Map.empty

    let coordinateMap =
        coordinateIdMap
        |> Map.map (fun key value -> {point=key;id=Some value;nearest=Id value})

    printfn "Center: (%A,%A); Edges: %A" center.x center.y edges
    let reduce ringIndex map =
        // printfn "Ring: %A" ringIndex
        getRing center ringIndex
        |> List.fold (fun map' a ->
            if Map.containsKey a coordinateMap then
                map'
            else 
                let nearest = getNearest a coordinates
                let point =
                    match nearest with
                    | None -> 
                        // printfn "    (%A,%A); Nearest: (Multiple)" a.x a.y
                        {point=a;id=None;nearest=Multiple}
                    | Some c -> 
                        let id = (coordinateIdMap.[c])
                        // printfn "    (%A,%A); Nearest: (%A,%A)" a.x a.y c.x c.y
                        {point=a;id=None;nearest=Id id}
                Map.add a point map'
        ) map
    let max = List.max [center.x;center.y]
    let endMap = [0..max] |> List.fold (fun map' i -> reduce i map') coordinateMap
    (coordinateIdMap,endMap,edges)

let getPointsWithInfiniteArea (map:Map<Coord,Point>) (left,top,bottom,right) =
    let origin = {x=0;y=0}
    let xOffsets = [left..right]
    let yOffsets = [top..bottom]
    let pointsOnEdge =
        xOffsets 
        |> List.map (fun i -> applyOffset origin (i,top)) 
        |> List.append (yOffsets |> List.map (fun i -> applyOffset origin (left,i)))
        |> List.append (xOffsets |> List.map (fun i -> applyOffset origin (i,bottom)))
        |> List.append (yOffsets |> List.map (fun i -> applyOffset origin (right,i)))
        |> List.distinct
    pointsOnEdge
    |> List.choose (fun pt -> 
        if Map.containsKey pt map then 
            let point = map.[pt] 
            match point.id,point.nearest with 
            | (Some id1,_) -> Some id1
            | (_,Id id2) -> Some id2
            | (_,_) -> None
        else None
    )
    |> List.distinct

let letters = '0'::List.append ['a'..'z'] ['A'..'Z']

let print pt =    
    match pt.id,pt.nearest with 
    | (Some _,_) -> '*'
    | (_,Id id2) -> letters.[id2]
    | (_) -> '.'

let solve1 input =
    let (coordinates,map,edges) = buildMap input
    // map |> Map.map (fun key value -> printfn "    (%A,%A); Nearest: %A" key.x key.y value.nearest) |> ignore
    let (top,left,bottom,right) = edges
    [left..right] 
    |> List.map (fun x -> 
        printfn ""
        [top..bottom] |> List.map (fun y -> 
            let pt = map.[{x=x;y=y}]
            printf "%c" (print pt)
        )
    )
    |> ignore
    printfn ""
    
    let infiniteAreas = getPointsWithInfiniteArea map edges
    printfn "Infinite areas: %A" (infiniteAreas |> List.map(fun id -> letters.[id]))
    let (coord,max) =
        coordinates
        |> Map.map (fun _ id ->
            if List.contains id infiniteAreas then 0
            else 
                Map.fold (fun state _ details ->  
                    match details.nearest with 
                    | (Id id2) -> if (id2 = id) then state+1 else state
                    | (_) -> state
                ) 0 map
        )
        |> Map.toList
        |> List.maxBy (fun (a,area) -> 
            let ch = letters.[coordinates.[a]]
            printfn "    Coordinate %c: (%A,%A); Size: %A" ch a.x a.y area
            area
        )
    let maxId = letters.[coordinates.[coord]]
    printfn "MAX - Coordinate %A: (%A,%A); Size: %A" maxId coord.x coord.y max
    if (49327 = max) then printfn "Success!" else printfn "Fail! Expected 49327, but found %A" max
    
// Center: (207,199); Edges: (67, 46, 348, 352)
// Infinite areas: [20; 9; 3; 5; 35; 1; 46; 25; 49; 31; 32; 16; 15; 37; 29; 11; 44; 42; 36; 22; 7]
// MAX - Coordinate G: (270,225); Size: 2832 // too low

// Center: (207,199); Edges: (67, 46, 348, 352)
// Infinite areas: [20; 9; 3; 5; 35; 1; 46; 25; 49; 31; 32; 16; 15; 37; 29; 11; 44; 42; 36; 22; 7]
// MAX - Coordinate z: (50,180); Size: 5323 // too high

// Coordinate (103,301): 3401