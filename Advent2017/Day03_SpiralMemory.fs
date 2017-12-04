namespace Advent2017

module Day03_SpiralMemory =
    open System

    let StepsToAccessPort input:int = 
        let calculateCoordinateFor previous n = 
            let coord = match n with 
            | 1 -> 
                (0,0,0)
            | 2 -> 
                (1,0,1)
            | _ -> 
                //let (x,y,level) = previous
                match previous with
                | (x,y,level) when x = level && y = -level ->
                    (x+1,y,level+1)
                | (x,y,level) when x = level && y < level ->
                    (x,y+1,level)
                | (x,y,level) when x > -level && y = level ->
                    (x-1,y,level)
                | (x,y,level) when x = -level && y > -level ->
                    (x,y-1,level)
                | (x,y,level) when x < level && y = -level ->
                    (x+1,y,level)
                | _  ->
                    previous
            //printfn "%i at %A" n coord
            coord
                

        let (x:int,y:int,_) = List.fold calculateCoordinateFor (0,0,0) [1..input]
        //printfn "%i at (%ix%i)" input x y
        Math.Abs(x) + Math.Abs(y)

    let FirstValueLargerThanInput input:int = 
        let initCoord = (0,0)
        let coordMap = Map([(initCoord, 1)])

        let calculateSum (x,y) map = 
            [(x+1,y-1);(x+1,y);(x+1,y+1);(x,y+1);(x-1,y+1);(x-1,y);(x-1,y-1);(x,y-1)]
            |> List.map(fun coord -> 
                let sum = if Map.containsKey coord map then map.[coord] else 0                
                //printfn "    %A = %i" coord sum
                sum
            )
            |> List.reduce(+)

        let calculateCoordinateFor (prevX, prevY, prevLevel, (prevMap:Map<(int*int),int>)) = 
            let prevSum = prevMap.[(prevX,prevY)]
            if prevSum > input then 
                None
            else
                let (x',y',level') = 
                    match (prevX,prevY) with
                    | (x,y) when x = prevLevel && y = -prevLevel ->
                        (x+1,y,prevLevel+1)
                    | (x,y) when x = prevLevel && y < prevLevel ->
                        (x,y+1,prevLevel)
                    | (x,y) when x > -prevLevel && y = prevLevel ->
                        (x-1,y,prevLevel)
                    | (x,y) when x = -prevLevel && y > -prevLevel ->
                        (x,y-1,prevLevel)
                    | (x,y) when x < prevLevel && y = -prevLevel ->
                        (x+1,y,prevLevel)
                    | _  ->
                        (prevX,prevY,prevLevel)
                let newsum = calculateSum (x',y') prevMap
                let newmap = Map.add (x',y') newsum prevMap
                //printfn "%A = %i" (x',y') newsum
                Some (newsum, (x',y',level',newmap))

        match input with
        | 0 -> 
            1
        | _ ->
            Seq.unfold calculateCoordinateFor (0,0,0,coordMap) |> Seq.last

