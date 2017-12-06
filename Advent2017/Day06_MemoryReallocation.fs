namespace Advent2017

module Day06_MemoryReallocation =

    let testCases = [
        [0;2;7;0]
        [2;4;1;2]
        [3;1;2;3]
        [0;2;3;4]
    ]

    let reallocate (index, blocks) memoryBanks =
        let length = List.length memoryBanks
        let blockList = 
            [1..blocks]
            |> List.map (fun _ -> 1)
            |> List.chunkBySize length
            |> List.map Array.ofList

        let getSumAtIndex i (list:int array list) =
            list
            |> List.map (fun sublist -> if i < Array.length sublist then sublist.[i] else 0)
            |> List.sum

        let newBlockAllocations = 
            [0..length-1]
            |> List.map (fun x -> getSumAtIndex x blockList)
            |> List.mapi (fun i sum -> 
                    let blockIndex = (i+index+1) % length
                    (blockIndex, sum)
               )
            |> Array.ofList
        
        memoryBanks
        |> List.mapi (fun i bank -> 
                let (_,sum) = Array.find (fun (idx,_) -> i=idx) newBlockAllocations
                if i = index then
                    sum
                else 
                    bank+sum
           )

    let reallocationCycle memoryBanks = 
        let (index,max) = 
            memoryBanks 
            |> List.mapi (fun i x -> i,x)
            |> List.maxBy (fun (_,x) -> x)
        reallocate (index,max) memoryBanks

    let tryKey key map =
        if Map.containsKey key map then
            Some(map.[key])
        else
            None

    let repeat memoryBank =
        let configurations = Map([memoryBank,1])

        let rec reallocateRec (steps,loop,block) previous (memory:int list) =
            let memory' = reallocationCycle memory
            match tryKey memory' previous with
            | Some (1) -> 
                let config' = Map.add memory' 2 previous
                let next = if block = [] then (steps+1,0,memory') else (steps,loop+1,block)
                reallocateRec next config' memory' 
            | Some (_) ->
                (steps, loop+1)
            | None ->                
                let previous' = Map.add memory' 1 previous
                reallocateRec (steps+1,loop+1,block) previous' memory' 

        reallocateRec (0,0,[]) configurations memoryBank
    
    let test testMethod expected input =
        let answer = testMethod input
        if answer = expected then
            printfn "Success! Result for %A was %A and %A was expected" input answer expected
        else
            printfn "Failed! Result for %A was %A, but %A was expected" input answer expected

    // test (reallocationCycle) testCases.[1] testCases.[0]
    let realInput = [14;0;15;12;11;11;3;5;1;6;8;4;9;1;8;4]

    // test repeat (5,4) [0;2;7;0];;
    // test repeat (11137,1037) realInput;;