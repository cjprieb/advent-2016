//module Day10_Knots

type State = {circle:int list;position:int;skip:int}

let tieKnot (item:State) knotLength =
    let circle = item.circle
    let circleLength = List.length circle
    let wrapIndex = circleLength - item.position
    let revSubList = 
        List.concat [circle;circle]
        |> List.skip item.position 
        |> List.take knotLength
        |> List.rev
    let (startList,middleList,endList) = 
        if wrapIndex < List.length revSubList then
            let (listToEnd, listFromStart) = List.splitAt wrapIndex revSubList
            let untouchedList = 
                circle 
                |> List.skip (List.length listFromStart)
                |> List.take (circleLength - knotLength)
            (listFromStart,untouchedList,listToEnd)
        else 
            let (listToEnd,listFromStart) = 
                List.concat [circle;circle]
                |> List.skip (item.position + knotLength)
                |> List.take (circleLength - knotLength)
                |> List.splitAt (circleLength - (item.position + knotLength))
            (listFromStart,revSubList,listToEnd)
    //printfn "Circle=%A" (circle |> List.map (fun i -> if i = item.position then sprintf "[%i]" i else i.ToString()))
    //printfn "Start=%A Middle=%A End=%A" startList middleList endList

    List.concat [startList;middleList;endList]

let processInstruction (item:State) knotLength =
    let circle' = tieKnot item knotLength
    let position' = (item.position + knotLength + item.skip) % (List.length item.circle)
    let skip' = item.skip+1
    {circle=circle';position=position';skip=skip'}

let processInstructions (circle:int list) (lengths:int list) =
    let startingState = {circle=circle;position=0;skip=0}
    let endState =
        lengths 
        |> List.fold processInstruction startingState
    endState.circle

let toAsciiList (input:string) =
    input
    |> Seq.map (fun ch -> int ch)
    |> List.ofSeq

let xor input = List.reduce (^^^) input

let toHexString i = sprintf "%02x" i

let denseHash input = 
    input 
    |> List.splitInto 16
    |> List.map xor
    |> List.map toHexString
    |> List.reduce (+)

let testCircle = [0..4]
let testLengths = [3;4;1;5]
let testInput = 
    [
        {circle=[0..4];position=0;skip=0},3,(3,1,[2;1;0;3;4])
        {circle=[2;1;0;3;4];position=3;skip=1},4,(3,2,[4;3;0;1;2])
        {circle=[4;3;0;1;2];position=3;skip=2},1,(1,3,[4;3;0;1;2])
        {circle=[4;3;0;1;2];position=1;skip=3},5,(4,4,[3;4;2;1;0])
    ]
    
let test testMethod expected input =
    let answer = testMethod input
    if answer = expected then
        printfn "Success! Result for %A was %A and %A was expected" input answer expected
    else
        printfn "Failed! Result for %A was %A, but %A was expected" input answer expected

let testTieKnot (state,length,expected) = 
    let wrappedMethod = processInstruction state
    test wrappedMethod expected length

let answer1 = //54675
    let circle = [0..255]
    let lengths = [34;88;2;222;254;93;150;0;199;255;39;32;137;136;1;167]
    let circle' = processInstructions circle lengths
    circle' 
    |> List.take 2
    |> List.reduce (*)

let answer2 = //54675
    let circle = [0..255]
    let lengths = 
        [17;31;73;47;23]
        |> List.append (
            "34,88,2,222,254,93,150,0,199,255,39,32,137,136,1,167" 
            |> toAsciiList
        )

    let endState = 
        [1..64]
        |> List.fold (fun state i ->
            lengths 
            |> List.fold processInstruction state
        ) {circle=circle;position=0;skip=0} 

    endState.circle
    |> denseHash

// testTieKnot testInput.[0];;
// processInstructions testCircle testLengths;;
