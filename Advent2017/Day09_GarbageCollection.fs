//module Day09_GarbageCollection
type Group = {score:int;nested:Group list;parent:Group option}
type State = {escape:bool;isGarbage:bool;groups:Group list;current:Group option;garbageCount:int}

let addGroup (groups:Group list) (current:Group option) =
    let newScore = match current with | Some x -> x.score + 1 | None -> 1
    let newGroup = {score=newScore;nested=[];parent=current}
    (groups, Some newGroup)

let closeGroup (groups:Group list) (currentOpt:Group option) =
    match currentOpt with
    | Some current -> 
        let groups' = current::groups
        match current.parent with 
        | Some x -> 
            let nested = current::(x.nested)
            let newParent = {score=x.score;nested=nested;parent=x.parent}
            (groups',Some newParent)
        | None -> 
            (groups',None)
    | None ->
        (groups,currentOpt)

let processCharacter (state:State) (ch:char) =
    //printfn "Processing %c" ch
    let newState = 
        match (ch,state.isGarbage,state.escape) with
        | (_,true,true) | ('<',false,_) -> 
            {
                escape=false;isGarbage=true
                groups=state.groups;current=state.current
                garbageCount=state.garbageCount
            } 
        | ('!',true,false) -> 
            {
                escape=true;isGarbage=true
                groups=state.groups;current=state.current
                garbageCount=state.garbageCount
            }
        | ('>',true,_) -> 
            {
                escape=false;isGarbage=false
                groups=state.groups;current=state.current
                garbageCount=state.garbageCount
            }
        | ('{',false,_) -> 
            let (groups',current') = addGroup state.groups state.current
            {
                escape=false;isGarbage=false
                groups=groups';current=current'
                garbageCount=state.garbageCount
            }
        | ('}',false,_) -> 
            let (groups',current') = closeGroup state.groups state.current
            {
                escape=false;isGarbage=false
                groups=groups';current=current'
                garbageCount=state.garbageCount
            }
        | (_,true,_) -> 
            {
                escape=false;isGarbage=true
                groups=state.groups;current=state.current
                garbageCount=state.garbageCount+1
            }
        | _ -> 
            state
    //printfn "    New state %A" newState
    newState

let parse (input:string) =
    let rec parseRec state list =
        match list with
        | [] -> 
            (state.groups,state.garbageCount)
        | x::xs ->
            let nextState = processCharacter state x
            parseRec nextState xs

    let characterStream = List.ofSeq input
    let startingState = {escape=false;isGarbage=false;groups=[];current=None;garbageCount=0}
    let (groups,garbageCount) = parseRec startingState characterStream
    let score = groups |> List.map (fun x -> x.score) |> List.sum
    ((List.length groups),score,garbageCount)
    
let test testMethod expected input =
    //printfn "Test %A" input
    let answer = testMethod input
    if answer = expected then
        printfn "Success! Result for %A was %A and %A was expected" input answer expected
    else
        printfn "Failed! Result for %A was %A, but %A was expected" input answer expected
    
let testWithoutInput testMethod expected input =
    //printfn "Test %A" input
    let answer = testMethod input
    if answer = expected then
        printfn "Success! Result was %A and %A was expected" answer expected
    else
        printfn "Failed! Result was %A, but %A was expected" answer expected

let garbageTests = [    
    "<>",0 //, empty garbage.
    "<random characters>",17 //, garbage containing random characters.
    "<<<<>",3 //, because the extra < are ignored.
    "<{!>}>",2 //, because the first > is canceled.
    "<!!>" ,0//, because the second ! is canceled, allowing the > to terminate the garbage.
    "<!!!>>",0 //, because the second ! and the first > are canceled.
    "<{o\"i!a,<{i<a>",10 //, which ends at the first >.
]

let groupTest = [    
    "{}", 1,1,0
    "{{{}}}", 3,6,0
    "{{},{}}", 3,5,0
    "{{{},{},{{}}}}", 6,16,0
    "{<{},{},{{}}>}", 1,1,10
    "{<a>,<a>,<a>,<a>}", 1,1,4
    "{{<ab>},{<ab>},{<ab>},{<ab>}}", 5,9,8
    "{{<!>},{<!>},{<!>},{<a>}}", 2,3,13
    "{{<!!>},{<!!>},{<!!>},{<a>}}", 5,9,1
]

let testGarbage list = 
    List.iter (fun (item,garbageCount) -> test (parse) (0,0,garbageCount) item ) list

let testValid list = 
    List.iter (fun (item,count,score,garbageCount) -> test (parse) (count,score,garbageCount) item ) list

let testValidItem index = 
    let (item,groupCnt,groupScore,garbageCount) = groupTest.[index]
    test (parse) (groupCnt,groupScore,garbageCount) item

let input = List.head (TestResources.ReadAllLines "Day09.txt")

//testAnswer input;;
let testAnswer input = 
    testWithoutInput parse (1875, 16689,0) input
