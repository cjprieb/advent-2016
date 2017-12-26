type Tape = Set<int>
type Value = Zero | One
type State = A | B | C | D | E | F
type TuringMachine = {tape:Tape;state:State;cursor:int}

let getValue tape cursor = 
    if Set.contains cursor tape then 
        One 
    else 
        Zero

let setValue tape cursor value =
    match value with
    | Zero -> Set.remove cursor tape
    | One -> Set.add cursor tape

let update tape cursor (newValue,newCursor,newState) = 
    {
        tape=(setValue tape cursor newValue);
        cursor=newCursor;
        state=newState
    }


let stateA tape cursor =
    let value = getValue tape cursor
    match value with 
    | Zero -> update tape cursor (One,(cursor+1),B)
    | One -> update tape cursor (Zero,(cursor-1),E)

let stateB tape cursor =
    let value = getValue tape cursor
    match value with 
    | Zero -> update tape cursor (One,(cursor-1),C)
    | One -> update tape cursor (Zero,(cursor+1),A)

let stateC tape cursor =
    let value = getValue tape cursor
    match value with 
    | Zero -> update tape cursor (One,(cursor-1),D)
    | One -> update tape cursor (Zero,(cursor+1),C)

let stateD tape cursor =
    let value = getValue tape cursor
    match value with 
    | Zero -> update tape cursor (One,(cursor-1),E)
    | One -> update tape cursor (Zero,(cursor-1),F)

let stateE tape cursor =
    let value = getValue tape cursor
    match value with 
    | Zero -> update tape cursor (One,(cursor-1),A)
    | One -> update tape cursor (One,(cursor-1),C)

let stateF tape cursor =
    let value = getValue tape cursor
    match value with 
    | Zero -> update tape cursor (One,(cursor-1),E)
    | One -> update tape cursor (One,(cursor+1),A)

let move machine =
    let f = 
        match machine.state with
        | A -> stateA
        | B -> stateB
        | C -> stateC
        | D -> stateD
        | E -> stateE
        | F -> stateF
    f machine.tape machine.cursor

// doDiagnosticTest 12386363;; // 4385
let doDiagnosticTest steps =
    let initialMachine = {tape=Set.empty;cursor=0;state=A}
    let endingMachine =
        [1..steps]
        |> List.fold (fun machine _ -> move machine) initialMachine
    Set.count endingMachine.tape