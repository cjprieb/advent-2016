let input = 5153

type Cell = {x:int;y:int}
let toCell x y = {x=x;y=y}

let extractHundredDigit num = ((num % 1000) - (num % 100)) / 100
let rackId cell = cell.x + 10 
let power serialNumber cell =
    let id = rackId cell
    let initialPower = (id * cell.y) + serialNumber
    (extractHundredDigit (initialPower * id)) - 5

let cellList min max = 
    [min.x..max.x]
    |> List.collect (fun x -> [min.y..max.y] |> List.map (toCell x))

let allCells = cellList {x=1;y=1} {x=300;y=300}

let squareList size cell = cellList cell {x=cell.x+size-1;y=cell.y+size-1}

let computePower serialNumber cells =
    cells
    |> List.fold (fun map cell -> 
        Map.add cell (power serialNumber cell) map
    ) Map.empty

let sumCells (map:Map<Cell,int>) cells = cells |> List.sumBy (fun cell -> map.[cell])

let computeBest computer (bestCell,bestSize,bestPower) input =
    let (cell,size,value) = computer input
    if (value > bestPower) then
        cell,size,value
    else 
        bestCell,bestSize,bestPower

let squareComputer map size cell = cell,size,(sumCells map (squareList size cell))

let computeBestFor map size =
    let calculator = squareComputer map size
    let cells = cellList {x=1;y=1} {x=301-size;y=301-size}
    cells |> List.fold (computeBest calculator) ({x=0;y=0},size,0)

// 235,18
let solve1 serialNumber =
    let map = computePower serialNumber allCells
    let (cell,size,power) = computeBestFor map 3
    printfn "%A,%A,%A (%A)" cell.x cell.y size power

let computeBestSquareSize map =
    let calculator = computeBestFor map
    [1..300]
    |> List.fold (computeBest calculator) ({x=0;y=0},0,0)

let solve2 serialNumber =
    let map = computePower serialNumber allCells
    let (cell,size,power) = computeBestSquareSize map
    printfn "%A,%A,%A (%A)" cell.x cell.y size power