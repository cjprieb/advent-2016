open System.Collections

type Packet = {layer:int;caughtOn:int list}

let getScannerPosition picoSeconds range =
    let actualCycle = range + (range - 2)
    let p = picoSeconds % actualCycle
    if p < range then p else actualCycle - p

let infiltrate (firewallDict:Generic.IDictionary<int,int>) maxLayer startingPico stopWhenCaught =
    let rec infiltrateRec (packet:Packet) (picoSeconds:int) =
        if packet.layer = maxLayer then
            packet.caughtOn
        else 
            let caughtOn' = 
                if firewallDict.ContainsKey packet.layer then
                    let range = firewallDict.[packet.layer]
                    let scannerPosition = getScannerPosition picoSeconds range
                    //printfn "Pico=%i Layer=%i Scanner At=%i" picoSeconds packet.layer scannerPosition
                    if scannerPosition = 0 then 
                        packet.layer::packet.caughtOn
                    else
                        packet.caughtOn
                else
                    //printfn "Pico=%i Layer=%i" picoSeconds packet.layer
                    packet.caughtOn
            //let layers' = picoFirewall layers
            if (List.length caughtOn') > 0 && stopWhenCaught then
                caughtOn'
            else 
                infiltrateRec {layer=packet.layer+1;caughtOn=caughtOn'} (picoSeconds+1)            
            
    //printfn "Firewalls: %A" firewallDict
    let caughtOn = infiltrateRec {layer=0;caughtOn=[]} startingPico
    //printfn "Caught On: %A" caughtOn
    if stopWhenCaught then
        if List.length caughtOn > 0 then -1 else startingPico
    else
        let severity (caughtOn:int list) =
            caughtOn 
            |> List.map (fun layer -> firewallDict.[layer] * layer)
            |> List.sum
        severity (caughtOn)
     
// Test.test (solve1) Test.scanners 24;;
// Test.test (solve1) Input.rawInput 1840;;
let solve1 (input:(int*int) list) = 
    let scannedLayers = dict (Seq.ofList input)
    let maxLayer = (scannedLayers.Keys |> Seq.max) + 1
    infiltrate scannedLayers maxLayer 0 false
     
     
// Test.test (solve2) Test.scanners 10;;
// Test.test (solve2) Input.rawInput 3850260;;
let solve2 (input:(int*int) list) = 
    let scannedLayers = dict (Seq.ofList input)
    let maxLayer = (scannedLayers.Keys |> Seq.max) + 1
    let rec repeat startingPico =
        //printfn "Repeating at %i" startingPico
        let pico = infiltrate scannedLayers maxLayer startingPico true
        if pico > 0 then
            pico
        else
            repeat (startingPico+1)
    repeat 1        
    
module Test = 
    let scanners = [
        0,3
        1,2
        4,4
        6,4
    ]

    let scannerPositions = [
        (0,2,0)
        (1,2,1)
        (2,2,0)
        (3,2,1)
        (4,2,0)
        (0,3,0)
        (1,3,1)
        (2,3,2)
        (3,3,1)
        (4,3,0)
        (5,3,1)
        (0,4,0)
        (1,4,1)
        (2,4,2)
        (3,4,3)
        (4,4,2)
        (5,4,1)
        (6,4,0)
        (7,4,1)
        (8,4,2)
        (9,4,3)
        (10,4,2)
        (11,4,1)
    ]

    let test_printinput method input expected = 
        let actual = method input
        if actual = expected then
            printfn "Success! %A returned %A" input actual
        else
            printfn "Failed! %A returned %A, but %A was expected" input actual expected

    let test method input expected = 
        let actual = method input
        if actual = expected then
            printfn "Success! Result was %A" actual
        else
            printfn "Failed! Result was %A, but %A was expected" actual expected

    let testScannerPositioning list =
        List.iter (fun (pico, range, expected) ->
            //test_printinput (getScannerPosition pico) range expected
            let actual = getScannerPosition pico range
            if actual = expected then
                printfn "Success! Range %i at pico %i is %i" range pico actual
            else
                printfn "Failed! Range %i at pico %i was %i, but %i was expected" range pico actual expected
        ) list

module Input = 
    let rawInput = [
        0, 5
        1, 2
        2, 3
        4, 4
        6, 6
        8, 4
        10, 8
        12, 6
        14, 6
        16, 8
        18, 6
        20, 9
        22, 8
        24, 10
        26, 8
        28, 8
        30, 12
        32, 8
        34, 12
        36, 10
        38, 12
        40, 12
        42, 12
        44, 12
        46, 12
        48, 14
        50, 12
        52, 14
        54, 12
        56, 14
        58, 12
        60, 14
        62, 14
        64, 14
        66, 14
        68, 14
        70, 14
        72, 14
        76, 14
        80, 18
        84, 14
        90, 18
        92, 17
    ]

