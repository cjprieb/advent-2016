open System.Numerics

type RegisterState = {registers:Map<char,BigInteger>;soundsPlayed:BigInteger list;index:int}
type Value = RegisterValue of char | IntValue of int

let getValue x registers =
    match x with
    | RegisterValue r ->
        if r < 'a' || r > 'p' then
            printfn "Illegal register value: %A" r
            BigInteger 0
        else if Map.containsKey r registers then
            registers.[r]
        else
            BigInteger 0
    | IntValue i ->
        BigInteger i

let set x y state =
    let value = getValue y state.registers
    let registers' = Map.add x value state.registers
    {
        registers=registers'
        soundsPlayed=state.soundsPlayed
        index=state.index+1
    }

let opp x y opp state =
    let xValue = getValue (RegisterValue x) state.registers
    let yValue = getValue y state.registers
    let registers' = Map.add x (opp xValue yValue) state.registers
    {
        registers=registers'
        soundsPlayed=state.soundsPlayed
        index=state.index+1
    }

let add x y state = opp x y (+) state

let mul x y state = opp x y (*) state

let mod' x y state = opp x y (%) state

let rcv x state =
    let xValue = getValue (RegisterValue x) state.registers
    let index' =
        if not (xValue = (BigInteger 0)) && (0 < List.length state.soundsPlayed) then 
            printfn "Recovering last played sound: %A" (List.last state.soundsPlayed)
            -1
        else
            state.index+1    
    {
        registers=state.registers
        soundsPlayed=state.soundsPlayed
        index=index'
    }
    
let snd x state =
    let value = getValue (RegisterValue x) state.registers
    let list' = List.concat [state.soundsPlayed;[value]]
    printfn "Played sound: %A" value
    {
        registers=state.registers
        soundsPlayed=list'
        index=state.index+1
    }

let jgz x y state =
    let xValue = getValue x state.registers
    let yValue = getValue y state.registers
    let index' =
        if xValue > (BigInteger 0) then
            state.index + (int yValue)
        else
            state.index + 1
    {
        registers=state.registers
        soundsPlayed=state.soundsPlayed
        index=index'
    }

// solve1 realInstructions;; // 3423
let solve1 instructions =
    let rec runRec state =
        if state.index < 0 || state.index >= (List.length instructions) then
            state
        else
            let instruction = instructions.[state.index]
            let next = instruction state
            runRec next

    let initState = {registers=Map.empty;soundsPlayed=List.empty;index=0}
    let n = runRec initState
    0


let testInstructions = [
    set 'a' (IntValue 1)
    add 'a' (IntValue 2)
    mul 'a' (RegisterValue 'a')
    mod' 'a' (IntValue 5)
    snd 'a'
    set 'a' (IntValue 0)
    rcv 'a'
    jgz (RegisterValue 'a') (IntValue -1)
    set 'a' (IntValue 1)
    jgz (RegisterValue 'a') (IntValue -2)
]

let realInstructions = [
    set 'i' (IntValue 31)
    set 'a' (IntValue 1)
    mul 'p' (IntValue 17)
    jgz (RegisterValue 'p') (RegisterValue 'p')
    mul 'a' (IntValue 2)
    add 'i' (IntValue -1)
    jgz (RegisterValue 'i') (IntValue -2)
    add 'a' (IntValue -1)
    set 'i' (IntValue 127)
    set 'p' (IntValue 618)
    mul 'p' (IntValue 8505)
    mod' 'p' (RegisterValue 'a')
    mul 'p' (IntValue 129749)
    add 'p' (IntValue 12345)
    mod' 'p' (RegisterValue 'a')
    set 'b' (RegisterValue 'p')
    mod' 'b' (IntValue 10000)
    snd 'b'
    add 'i' (IntValue -1)
    jgz (RegisterValue 'i') (IntValue -9)
    jgz (RegisterValue 'a') (IntValue 3)
    rcv 'b'
    jgz (RegisterValue 'b') (IntValue -1)
    set 'f' (IntValue 0)
    set 'i' (IntValue 126)
    rcv 'a'
    rcv 'b'
    set 'p' (RegisterValue 'a')
    mul 'p' (IntValue -1)
    add 'p' (RegisterValue 'b')
    jgz (RegisterValue 'p') (IntValue 4)
    snd 'a'
    set 'a' (RegisterValue 'b')
    jgz (IntValue 1) (IntValue 3)
    snd 'b'
    set 'f' (IntValue 1)
    add 'i' (IntValue -1)
    jgz (RegisterValue 'i') (IntValue -11)
    snd 'a'
    jgz (RegisterValue 'f') (IntValue -16)
    jgz (RegisterValue 'a') (IntValue -19)
]