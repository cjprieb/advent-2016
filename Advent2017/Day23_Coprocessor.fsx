type DebugProcessor = 
    {
        registers:Map<char,int>
        mulCount:int
        instruction:int
        totalCount:int
    }

type Value = RegisterValue of char | IntValue of int

let get x registers =
    match x with
    | RegisterValue ch ->
        if Map.containsKey ch registers then registers.[ch] else 0
    | IntValue i -> 
        i

let print registers =
    let str = 
        ['a'..'h'] 
        |> List.map (fun ch -> sprintf "%c = %i" ch (get (RegisterValue ch) registers))
        |> List.reduce (fun s1 s2 -> s1 + "; " + s2)
    sprintf "{%s}" str

let set x y (state:DebugProcessor) =
    //printfn "%2i Setting %A to %A" state.instruction x y
    let registers' = Map.add x (get y state.registers) state.registers
    {
        registers=registers'
        mulCount=state.mulCount
        instruction=state.instruction+1
        totalCount=state.totalCount+1
    }

let sub x y (state:DebugProcessor) =
    //printfn "%2i Subtracting %A from %A" state.instruction y x
    let value = (get (RegisterValue x) state.registers) - (get y state.registers) 
    let registers' = Map.add x value state.registers
    {
        registers=registers'
        mulCount=state.mulCount
        instruction=state.instruction+1
        totalCount=state.totalCount+1
    }

let mul x y (state:DebugProcessor) =
    //printfn "%2i Multiplying %A by %A" state.instruction x y
    let value = (get (RegisterValue x) state.registers) * (get y state.registers) 
    let registers' = Map.add x value state.registers
    {
        registers=registers'
        mulCount=state.mulCount+1
        instruction=state.instruction+1
        totalCount=state.totalCount+1
    }

let jnz x y (state:DebugProcessor) =
    let cmpValue = get x state.registers
    let jmpValue = get y state.registers
    //printfn "%2i Jumping %A (%i) (if %A (%i) <> 0)" state.instruction y jmpValue x cmpValue
    let instruction' = if cmpValue <> 0 then state.instruction + jmpValue else state.instruction + 1
    {
        registers=state.registers
        mulCount=state.mulCount
        instruction=instruction'
        totalCount=state.totalCount+1
    }

let run (instructions:(DebugProcessor -> DebugProcessor) []) = 
    //let startingState = {registers=Map.empty;mulCount=0;instruction=0;totalCount=0}
    let startingState = {registers=(Map.add 'a' 1 Map.empty);mulCount=0;instruction=0;totalCount=0}
    let rec loop state =
        let instruction = instructions.[state.instruction]
        let state' = instruction state
        //printfn "  %s" (print state.registers)
        //printfn "Multiply Count: %i" state.mulCount
        if (state.totalCount % 1000 = 0) then printfn "h = %i" (get (RegisterValue 'h') state.registers)
        if state'.instruction < 0 || state'.instruction >= (Array.length instructions) || state.totalCount > 100000
        then
            state'
        else
            loop state'
    let endingState = loop startingState
    (get (RegisterValue 'h') endingState.registers)

let instructions = [|
    set 'b' (IntValue 81)
    set 'c' (RegisterValue 'b')
    jnz (RegisterValue 'a') (IntValue 2)
    jnz (IntValue 1) (IntValue 5)
    mul 'b' (IntValue 100)
    sub 'b' (IntValue -100000)
    set 'c' (RegisterValue 'b')
    sub 'c' (IntValue -17000)
    set 'f' (IntValue 1)
    set 'd' (IntValue 2)
    set 'e' (IntValue 2)
    set 'g' (RegisterValue 'd')
    mul 'g' (RegisterValue 'e')
    sub 'g' (RegisterValue 'b')
    jnz (RegisterValue 'g') (IntValue 2)
    set 'f' (IntValue 0)
    sub 'e' (IntValue -1)
    set 'g' (RegisterValue 'e')
    sub 'g' (RegisterValue 'b')
    jnz (RegisterValue 'g') (IntValue -8)
    sub 'd' (IntValue -1)
    set 'g' (RegisterValue 'd')
    sub 'g' (RegisterValue 'b')
    jnz (RegisterValue 'g') (IntValue -13)
    jnz (RegisterValue 'f') (IntValue 2)
    sub 'h' (IntValue -1)
    set 'g' (RegisterValue 'b')
    sub 'g' (RegisterValue 'c')
    jnz (RegisterValue 'g') (IntValue 2)
    jnz (IntValue 1) (IntValue 3)
    sub 'b' (IntValue -17)
    jnz (IntValue 1) (IntValue -23)
|]

