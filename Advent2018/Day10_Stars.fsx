open System.IO
open System.Text.RegularExpressions
#load "TestResources.fs"

let input = TestResources.ReadAllLines("Day10.txt")

let sample = TestResources.ReadAllLines("Day10_Sample.txt")

type Position = {x:int;y:int}
type Velocity = {vx:int;vy:int}
type Star = {position:Position;velocity:Velocity}
type Bounds = {top:int;left:int;bottom:int;right:int}

let parse line =
    let matches = Regex.Match(line, "position=<\s*(\-?\d+),\s*(\-?\d+)> velocity=<\s*(\-?\d+),\s*(\-?\d+)>")
    let px = matches.Groups.[1].Value |> int
    let py = matches.Groups.[2].Value |> int
    let vx = matches.Groups.[3].Value |> int
    let vy = matches.Groups.[4].Value |> int
    {
        position = { x = px; y = py }
        velocity = { vx = vx; vy = vy }
    }

let move scale star =     
    let x' = (scale * star.velocity.vx) + star.position.x
    let y' = (scale * star.velocity.vy) + star.position.y
    {
        position = { x = x'; y = y' }
        velocity = star.velocity
    }

let getBounds stars =
    let positions = Seq.map (fun star -> star.position) stars
    let initial = 
        match Seq.tryHead positions with
        | Some first -> {top=first.y;left=first.x;bottom=first.y;right=first.x}
        | None -> {top=0;left=0;bottom=0;right=0}

    Seq.fold (fun bounds position ->
        {
            top = if (bounds.top <= position.y) then bounds.top else position.y
            left = if (bounds.left <= position.x) then bounds.left else position.x
            bottom = if (bounds.bottom >= position.y) then bounds.bottom else position.y
            right = if (bounds.right >= position.x) then bounds.right else position.x
        }
    ) initial positions

let contains (x,y) stars = 
    Seq.isEmpty (Seq.filter (fun star -> star.position.x = x && star.position.y = y) stars) |> not

let print bounds stars =
    let yRange = [bounds.top..bounds.bottom]
    let xRange = [bounds.left..bounds.right]
    // printfn "Y: %A to %A; X: %A to %A" bounds.top bounds.bottom bounds.left bounds.right
    yRange
    |> List.iter(fun y ->
        let s = xRange |> List.fold (fun l x -> l + (if (contains (x,y) stars) then "#" else " ")) ""
        printfn "%s" s
    )  
    printfn ""
    //TestResources.WriteAllLines "Day10_result.txt" lines

let isPotentialMessage bounds = (bounds.bottom - bounds.top) <= 10

let toStars input = input |> Seq.map parse

let advance scale stars = Seq.map (move scale) stars

let getScale bounds = 
    let height = bounds.bottom - bounds.top
    let width = bounds.right - bounds.left
    if width < 300 && height < 300 then 1
    else if height < 500 then 5
    else if height < 1000 then 10
    else if height < 5000 then 50
    else if height < 10000 then 100
    else 1000


let rec advanceMany second scaleOpt stars =
    let bounds = getBounds stars
    let scale = getScale bounds
    if isPotentialMessage bounds then   
        printfn "Bounds after %A seconds: %A x %A" second (bounds.bottom-bounds.top) (bounds.right-bounds.left)
        // printfn "%A" stars     
        print bounds stars
        stars
    else if second > 20000 then
        stars
    else 
        let stars' = advance scale stars
        advanceMany (second + scale) scale stars'


let advanceTo second stars =
    let bounds = getBounds stars
    printfn "Bounds after %A seconds: %A x %A" second (bounds.bottom-bounds.top) (bounds.right-bounds.left)
    let stars' = advance second stars
    print bounds stars'


// Answer: LKPHZHHJ after 10159 seconds
let solve1 input = advanceMany 0 0 (toStars input) |> ignore

//let x = solve1 input;;