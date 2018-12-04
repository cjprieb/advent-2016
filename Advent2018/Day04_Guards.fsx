open System.IO
open System.Text.RegularExpressions
open System

type Id = Id of string
type LogMessage = GuardId of Id | WakeUp | Asleep
type Log = {
    Date:DateTime
    Message:LogMessage
}
type Range = {Start:int;End:int}
type SleepCycle = {GuardId:Id;Start:int option;End:int option}

let linePattern = new Regex("\[(\d{4}-\d{2}-\d{2} \d{2}:\d{2})\] (.+)")
let guardPattern = new Regex("Guard #(\d+)")

//[1518-11-05 00:45] falls asleep
let parse str =
    let aMatch = linePattern.Match(str)
    if aMatch.Success then
        let date = DateTime.Parse(aMatch.Groups.[1].Value)
        let msg = aMatch.Groups.[2].Value
        let bMatch = guardPattern.Match(msg)
        if bMatch.Success then
            Some { Date=date; Message=(GuardId (Id bMatch.Groups.[1].Value)) }
        else if msg = "falls asleep" then
            Some { Date=date; Message=Asleep }
        else if msg = "wakes up" then 
            Some { Date=date; Message=WakeUp }
        else
            printfn "Log message doesn't match - %A" msg
            None
    else
        printfn "Log doesn't match - %A" str
        None

let input = File.ReadAllLines("AdventOfCode\\Day04.txt")

let sample = File.ReadAllLines("AdventOfCode\\Day04Sample.txt")

let updateMinutes (range:Range) map =
    [range.Start..range.End-1] 
    |> List.fold (fun map minute ->
        let oldValue = if Map.containsKey minute map then map.[minute] else 0
        Map.add minute (oldValue+1) map
    ) map

let updateListMap id range map =
    let minuteMap = if Map.containsKey id map then map.[id] else Map.empty
    let minuteMap' = updateMinutes range minuteMap
    Map.add id minuteMap' map

let updateMinutesAsleep (cycle:SleepCycle) minutesAsleepByGuard =
    match (cycle.Start,cycle.End) with
    | (Some start,Some end') ->
        let map = updateListMap cycle.GuardId {Start=start;End=end'} minutesAsleepByGuard
        (Some {GuardId=cycle.GuardId;Start=None;End=None},map)
    | (_,_) -> (Some cycle,minutesAsleepByGuard)

let foldAction (cycleOpt:SleepCycle option,minutesAsleepByGuard) (log:Log) =
    match (cycleOpt,log.Message) with 
    | (_,GuardId id) -> 
        (Some {GuardId=id;Start=None;End=None},minutesAsleepByGuard)
    | (Some cycle,WakeUp) -> 
        let cycle' = {GuardId=cycle.GuardId;Start=cycle.Start;End=Some log.Date.Minute}
        updateMinutesAsleep cycle' minutesAsleepByGuard
    | (Some cycle,Asleep) -> 
        let cycle' = {GuardId=cycle.GuardId;Start=Some log.Date.Minute;End=None}
        (Some cycle',minutesAsleepByGuard)
    | (_,_) ->
        (None,minutesAsleepByGuard)

let total minutesAsleep =
    minutesAsleep
    |> Map.toList
    |> List.sumBy (fun (_,count) -> count)

let find (selectedGuardIdOpt,mostMinutesAsleep) id minutesAsleep =
    let totalAsleep =  total minutesAsleep
    if totalAsleep > mostMinutesAsleep then 
        (Some id,totalAsleep)
    else 
        (selectedGuardIdOpt,mostMinutesAsleep)

let max minutesAsleep =
    minutesAsleep
    |> Map.toList
    |> List.maxBy (fun (_,count) -> count)

let find2 (selectedGuardIdOpt,mostMinutesAsleep) id minutesAsleep =
    let (_,maxAsleep) =  max minutesAsleep
    if maxAsleep > mostMinutesAsleep then 
        (Some id,maxAsleep)
    else 
        (selectedGuardIdOpt,mostMinutesAsleep)

// Guard #Id "3557" was asleep the most on minute 30. Answer: 106710
let solve1 (input:string[]) =
    let (_,minutesAsleepByGuard) = 
        input
        |> Array.choose parse 
        |> Array.sortBy (fun log -> log.Date)
        |> Array.fold foldAction (None,Map.empty)

    let (idOpt,_) = Map.fold find (None,0) minutesAsleepByGuard
    match idOpt with
    | Some id -> 
        let (minute,_) = 
            minutesAsleepByGuard.[id]
            |> Map.fold (fun (key',most) key value -> if value > most then (key,value) else (key',most)) (0,0)
        let idNum = match id with | (Id idStr) -> idStr |> int
        printfn "Guard %A was asleep the most on minute %A. Answer: %A" id minute (idNum * minute)
    | None -> 
        printfn "No guard found"

// Guard Id "269" was asleep the most on minute 40. Answer: 10760
// Guard Id "269" was asleep the most on minute 39. Answer: 10491
let solve2 (input:string[]) =
    let (_,minutesAsleepByGuard) = 
        input
        |> Array.choose parse 
        |> Array.sortBy (fun log -> log.Date)
        |> Array.fold foldAction (None,Map.empty)

    let (idOpt,_) = Map.fold find2 (None,0) minutesAsleepByGuard
    match idOpt with
    | Some id -> 
        let (minute,_) = 
            minutesAsleepByGuard.[id]
            |> Map.fold (fun (key',most) key value -> if value > most then (key,value) else (key',most)) (0,0)
        let idNum = match id with | (Id idStr) -> idStr |> int
        printfn "Guard %A was asleep the most on minute %A. Answer: %A" id minute (idNum * minute)
    | None -> 
        printfn "No guard found"
