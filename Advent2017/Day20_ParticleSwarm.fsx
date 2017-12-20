open System
open System.Text.RegularExpressions
#load "TestResources.fs"

    //Increase the X velocity by the X acceleration.
    //Increase the Y velocity by the Y acceleration.
    //Increase the Z velocity by the Z acceleration.
    //Increase the X position by the X velocity.
    //Increase the Y position by the Y velocity.
    //Increase the Z position by the Z velocity.
type Coordinate = {x:int;y:int;z:int}
type Particle = {id:int;position:Coordinate;velocity:Coordinate;acceleration:Coordinate}

let distance a b =
    [
        Math.Abs(a.x-b.x) |> float
        Math.Abs(a.y-b.y) |> float
        Math.Abs(a.z-b.z) |> float
    ] |> List.sum

let distanceToOrigin = distance {x=0;y=0;z=0} 

let tick (i:int) (a:Particle) =
    let velocity' = 
        {
            x=a.acceleration.x+a.velocity.x
            y=a.acceleration.y+a.velocity.y
            z=a.acceleration.z+a.velocity.z
        }
    let position' = 
        {
            x=velocity'.x+a.position.x
            y=velocity'.y+a.position.y
            z=velocity'.z+a.position.z
        }
    //let runningAverage' = ((a.runningAverage * float i) + (distanceToOrigin position')) / float (i+1)
    {
        id=a.id
        position=position'
        velocity=velocity'
        acceleration=a.acceleration
        //runningAverage=runningAverage'
    }

//let closestParticle particles =
//    let averageDistance p = p.runningAverage
//    Array.minBy averageDistance particles

let parseCoord s =
    let matches = Regex.Match(s, @"[apv]=<(-?\d+),(-?\d+),(-?\d+)>")
    if matches.Success then
        let x = matches.Groups.[1].Value |> int
        let y = matches.Groups.[2].Value |> int
        let z = matches.Groups.[3].Value |> int
        {x=x;y=y;z=z}
    else
        failwith (sprintf "%s is not a valid coordinate" s)

let parseParticle i (line:string) =
    let coordinates = 
        line.Split(' ') 
        |> Array.map parseCoord
    let position = coordinates.[0]
    {
        id=i
        position=position
        velocity=coordinates.[1]
        acceleration=coordinates.[2]
        //runningAverage=(distanceToOrigin position)
    }

let testInput = [
    "p=<3,0,0>, v=<2,0,0>, a=<-1,0,0>"
    "p=<4,0,0>, v=<0,0,0>, a=<-2,0,0>"
]

let realInput = TestResources.ReadAllLines("Day20.txt")

// solve1 testInput;; // 0
// solve1 realInput;; // 150
let solve1 input =
    let particles = input |> List.mapi parseParticle
    let a =
        particles
        |> List.minBy (fun p -> distanceToOrigin p.acceleration)
    //let final =
    //    [1..max]
    //    |> List.fold (fun state _ -> state |> List.mapi tick) particles
    //    |> Array.ofList

    //let a = closestParticle final

    //printfn "150: %A" final.[150]
    //printfn "270: %A" final.[270]
    //printfn "285: %A" final.[285]

    a

let collisionFree particles p = 
    (particles 
    |> List.filter (fun q -> 
        if q.id = p.id then
            false
        else if q.position = p.position then
            true
        else
            false
    ) 
    |> List.length) = 0 // p equals itself

let solve2 max input =
    let particles = input |> List.mapi parseParticle
    let final =
        [1..max]
        |> List.fold (fun state i -> 
            let particles' =
                state 
                |> List.mapi tick
            List.filter (collisionFree particles') particles'
        ) particles
    List.length final
