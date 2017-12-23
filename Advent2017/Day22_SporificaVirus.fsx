#load "TestResources.fs"

type Point = {x:int;y:int}
type Direction = N | S | E | W
type Virus = {direction:Direction;position:Point}
type NodeStatus = Clean | Infected

let parseStatus = 
    function
    | '.' -> Some Clean
    | '#' -> Some Infected
    | _ -> None

let turnLeft direction = 
    match direction with
    | N -> W
    | W -> S
    | S -> E
    | E -> N

let turnRight direction = 
    match direction with
    | N -> E
    | W -> N
    | S -> W
    | E -> S

let turn nodeStatus = if nodeStatus = Clean then turnLeft else turnRight

let toggle point status nodes = 
    if status = Infected then 
            Map.remove point nodes 
        else
            Map.add point Infected nodes 

let move virus direction =
    let (x',y') =
        match direction with
        | N -> 0,(-1)
        | W -> (-1),0
        | S -> 0,(1)
        | E -> 1,0
    {direction=direction;position={x=virus.position.x+x';y=virus.position.y+y'}}

let getNodeStatus (point:Point) (nodes:Map<Point,NodeStatus>) =
    if Map.containsKey point nodes then
        Infected
    else 
        Clean

let print (nodes:Map<Point,NodeStatus>) virus =
    let size = 50
    [0..(size-1)] |> List.iter (fun y ->
        printfn ""
        [0..(size-1)] |> List.iter (fun x ->
            let x' = x - 10
            let y' = y - 10
            let point = {x=x';y=y'}
            let status = getNodeStatus point nodes
            if virus.position = point then
                printf "%c" (if status = Infected then '@' else '!')
            else
                printf "%c" (if status = Infected then '#' else '.')
        )
    )
    printfn ""

let burst (count,virus,nodes) i =
    let status = getNodeStatus virus.position nodes 
    let nodes' = toggle virus.position status nodes
    let virus' = (turn status) virus.direction |> move virus
    let count' = if status = Clean then count+1 else count
    //printfn "Burst %i (%i infected)" i count
    //printfn "  Virus starting at (%i,%i) %A" virus.position.x virus.position.y virus.direction
    //printfn "  Virus moving to (%i,%i) %A" virus'.position.x virus'.position.y virus'.direction
    //print nodes' virus'
    (count',virus',nodes')

let parse line = line |> List.ofSeq |> List.choose parseStatus

let testInput = 
    [
        {x=1;y=1},Infected
        {x=(-1);y=0},Infected
    ]
    |> Map.ofList

let realInput = 
    let lines = TestResources.ReadAllLines("Day22.txt") 
    //let lines = ["..#";"#..";"..."]
    lines
    |> List.mapi (fun y line ->
        (parse line)
        |> List.mapi (fun x status ->
            {x=x+1;y=y+1},status
        )
        |> List.filter (fun (_,status) -> status = Infected)
    )
    |> List.concat
    |> Map.ofList

// solve1 10000 {x=0;y=0} testInput;; 5587
// solve1 10000 {x=13;y=13} realInput;; (part 1) 5538
let solve1 iterations start input =
    let startVirus = {position=start;direction=N}
    printfn "Input" 
    print input startVirus
    let (count,endVirus,endNodes) =
        [1..iterations]
        |> List.fold burst (0,startVirus,input)
    //printfn "end nodes"
    //print endNodes endVirus
    printfn "Ending count is %i" count