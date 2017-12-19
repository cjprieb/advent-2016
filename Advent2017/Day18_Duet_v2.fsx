open System.Numerics
open System
open System.Web.UI.HtmlControls
open System.IO.Ports
open System.Windows.Forms

type Program = {
    id:int
    registers:Map<char,BigInteger>
    index:int
    sentCount:int
}
type Value = RegisterValue of char | IntValue of int
type InstructionResult = ProgramState of Program | ReceiveAction of (BigInteger -> Program) | SendAction of (Program*BigInteger)

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

type AgentMessage = Ready | ValueMessage of (BigInteger) | Done
type SupervisorMessage = Start of (int*MailboxProcessor<AgentMessage>) | Message of (int*BigInteger) | Waiting of int

type MessageAgent () = 
    let supervisor = MailboxProcessor.Start(fun inbox ->
        let postReady (mailboxOpt:MailboxProcessor<AgentMessage> option) =
            match mailboxOpt with
            | Some mailbox -> mailbox.Post Ready
            | None -> failwith "mailbox not ready"

        let postMessage (mailboxOpt:MailboxProcessor<AgentMessage> option) (value:BigInteger) =
            match mailboxOpt with
            | Some mailbox -> mailbox.Post (ValueMessage value)
            | None -> failwith "mailbox not ready"

        let postDone (mailboxOpt:MailboxProcessor<AgentMessage> option) =
            match mailboxOpt with
            | Some mailbox -> mailbox.Post Done
            | None -> failwith "mailbox not ready"

        let rec loop (agent0:MailboxProcessor<AgentMessage> option,agent1:MailboxProcessor<AgentMessage> option,waiting1,waiting2) = async {
            //printfn "Supervisor loop"
            let! message = inbox.Receive()
            let next =
                match message with
                | Start (id,mailbox) ->
                    printfn "  Starting [id=%i]" id
                    if id = 0 then
                        (Some mailbox,agent1,waiting1,waiting2)
                    else                        
                        postReady agent0
                        postReady (Some mailbox)
                        (agent0,Some mailbox,waiting1,waiting2)
                | Message (id,value) ->
                    printfn "  %i was sent to [id=%i]" (value |> int) id
                    if id = 0 then
                        postMessage agent0 value
                        (agent0,agent1,false,waiting2)
                    else
                        postMessage agent1 value
                        (agent0,agent1,waiting1,false)
                | Waiting id ->
                    printfn "  [id=%i] is waiting for sending" id
                    if id = 0 then
                        (agent0,agent1,true,waiting2) 
                    else
                        (agent0,agent1,waiting1,true)

            //if waiting1 && waiting2 then
            //    postDone agent0
            //    postDone agent1
            //    printfn "  Supervisor done. "
            //else
            return! loop next
        }
        loop(None,None,false,false)
    )

    let loopAgent id instructions (inbox:MailboxProcessor<AgentMessage>) =
        supervisor.Post (Start (id,inbox))
        let otherId = if id = 0 then 1 else 0
        let rec loop state = async {
            if state.index < 0 || state.index >= (List.length instructions) then
                return ()
            else
                let instruction = instructions.[state.index]
                let result = instruction state
                let! next = async {
                    match result with 
                    | ProgramState state ->
                        return state
                    | ReceiveAction receive ->
                        supervisor.Post (Waiting id)
                        let! msg = inbox.Receive()
                        match msg with 
                        | Ready -> 
                            return state
                        | Done -> 
                            if id = 1 then printfn "%i total items sent from [id=%i]" state.sentCount id
                            return {
                                id=state.id
                                registers=state.registers
                                sentCount=state.sentCount
                                index=(-1)
                            }
                        | ValueMessage value -> 
                            return receive value
                    | SendAction (state,value) ->
                        //if id = 1 then printfn "[id=%i] is sending %A to [id=%i]" id value otherId
                        supervisor.Post (Message (otherId,value))
                        return state
                }
                return! loop next
        }

        let ready = async { 
            return! inbox.Receive() 
        }
        
        let initState = {
            id=id
            registers=(Map.add 'p' (BigInteger id) Map.empty)
            index=0
            sentCount=0
        }
        loop initState

    member _x.Start instructions =
        MailboxProcessor.Start(loopAgent 0 instructions) |> ignore
        MailboxProcessor.Start(loopAgent 1 instructions)


let set x y state =
    let value = getValue y state.registers
    let registers' = Map.add x value state.registers
    ProgramState {
        id=state.id
        registers=registers'
        index=state.index+1
        sentCount=state.sentCount
    }

let opp x y opp state =
    let xValue = getValue (RegisterValue x) state.registers
    let yValue = getValue y state.registers
    let registers' = Map.add x (opp xValue yValue) state.registers
    ProgramState {
        id=state.id
        registers=registers'
        index=state.index+1
        sentCount=state.sentCount
    }

let add x y state = opp x y (+) state

let mul x y state = opp x y (*) state

let mod' x y state = opp x y (%) state

let rcv x state =
    let receive value =
        let registers' = Map.add x value state.registers
        {
            id=state.id
            registers=registers'
            index=state.index+1
            sentCount=state.sentCount
        }
    ReceiveAction receive
    
let snd x state =
    let value = 
        match x with 
        | RegisterValue r ->
            getValue (RegisterValue r) state.registers
        | IntValue i ->
            BigInteger i
    let next = {
        id=state.id
        registers=state.registers
        index=state.index+1
        sentCount=state.sentCount+1
    }
    if next.id = 1 then printfn "%i total items sent from [id=%i]" next.sentCount next.id
    SendAction (next,value)

let jgz x y state =
    let xValue = getValue x state.registers
    let yValue = getValue y state.registers
    let index' =
        if xValue > (BigInteger 0) then
            state.index + (int yValue)
        else
            state.index + 1
    ProgramState {
        id=state.id
        registers=state.registers
        index=index'
        sentCount=state.sentCount
    }

// solve2 realInstructions;; //248 is too low 7493
let solve2 instructions =
    printfn "Solving"
    let a = new MessageAgent()
    printfn "Starting"
    a.Start instructions
    printfn "Done?"


let testInstructions = [
    snd (IntValue 13)
    snd (IntValue 27)
    snd (RegisterValue 'p')
    rcv 'a'
    rcv 'b'
    rcv 'c'
    rcv 'd'
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
    snd (RegisterValue 'b')
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
    snd (RegisterValue 'a')
    set 'a' (RegisterValue 'b')
    jgz (IntValue 1) (IntValue 3)
    snd (RegisterValue 'b')
    set 'f' (IntValue 1)
    add 'i' (IntValue -1)
    jgz (RegisterValue 'i') (IntValue -11)
    snd (RegisterValue 'a')
    jgz (RegisterValue 'f') (IntValue -16)
    jgz (RegisterValue 'a') (IntValue -19)
]