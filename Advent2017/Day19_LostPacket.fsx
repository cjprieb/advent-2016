#load "TestResources.fs"
open System.IO

type Coordinate = {x:int;y:int}
type Direction = N | S | E | W
type State = {current:Coordinate;visited:Map<Coordinate,int>;word:string;direction:Direction;steps:int}
type ProgramIteration = GridState of State | Done

let valid position grid =
    position.x > 0 && 
    position.y > 0 && 
    position.x < Array2D.length1 grid && 
    position.y < Array2D.length2 grid

let canMove state grid (position,_) =
    if not (valid position grid) then
        false
    else
        let ch = grid.[position.x,position.y]
        match ch with
        | '+' | '|' | '-'  ->
            true
        | x when x >= 'A' && x <= 'Z' ->
            true
        | _ ->
            false

let opposite = 
    function
    | N -> S
    | S -> N
    | E -> W
    | W -> E

let surrounding current direction = 
    [
        {x=0;y=1},N
        {x=1;y=0},E
        {x=0;y=(-1)},S
        {x=(-1);y=0},W
    ] 
    |> List.filter(fun (_,dir) -> not (dir = opposite direction))
    |> List.map(fun (coord,dir) -> {x=current.x+coord.x;y=current.y+coord.y},dir)

let straightPositions state positions =
    positions 
    |> List.filter (fun (_,dir) -> dir = state.direction)

let update word ch =
    if ch >= 'A' && ch <= 'Z' then
        word + ch.ToString()
    else 
        word

let move state grid =
    let nextPositions = (surrounding state.current state.direction) |> List.filter (canMove state grid)
    let straightPosition = nextPositions |> (straightPositions state)
    if List.length nextPositions > 0 then
        let (nextPosition,nextDirection) =
            if (List.length straightPosition) = 1 then
                List.head straightPosition
            else          
                //printfn "switching direction at (%i,%i) %A" state.current.x state.current.y state.direction
                //if (List.length nextPositions) <> 1 then printfn "More than one position to choose from: %A" nextPositions
                List.head nextPositions
            
        let word' = update state.word grid.[nextPosition.x,nextPosition.y]
        GridState {
            current = nextPosition
            word = word'
            visited = (Map.add nextPosition 1 state.visited)
            steps = state.steps+1
            direction = nextDirection
        }
    else
        printfn "Done at (%i,%i)" state.current.x state.current.y
        Done

let testInput = [|
    "     |          "
    "     |  +--+    "
    "     A  |  C    "
    " F---|----E|--+ "
    "     |  |  |  D " 
    "     +B-+  +--+ "
|]
    
let realInput = TestResources.ReadAllLinesRaw("Day19.txt")

let getStartingPosition grid =
    let xMax = (Array2D.length1 grid) - 1
    let x =
        [0..xMax]
        |> List.filter (fun x -> grid.[x,0] = '|')
        |> List.head
    {x=x;y=0}

// solve1 realInput;; //PBAZYFMHT
let solve1 (lines:string[]) =
    let xMax = lines.[0].Length
    let yMax = lines.Length
    let grid = Array2D.init xMax yMax (fun x y -> lines.[y].[x])
    let rec moveRec state =
        let nextState = move state grid
        match nextState with
        | Done ->
            state
        | GridState s ->
            moveRec s
    
    let startingPosition = getStartingPosition grid
    let startingState = {
        current=startingPosition
        visited=(Map.add startingPosition 1 Map.empty)
        word=""
        steps=1
        direction=N
    }
    let result = moveRec startingState
    printfn "Word: %A; Steps: %i" result.word result.steps