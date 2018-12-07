open System.Text.RegularExpressions
#load "TestResources.fs"

let input = TestResources.ReadAllLines("Day07.txt")

let sample = [
    "Step C must be finished before step A can begin."
    "Step C must be finished before step F can begin."
    "Step A must be finished before step B can begin."
    "Step A must be finished before step D can begin."
    "Step B must be finished before step E can begin."
    "Step D must be finished before step E can begin."
    "Step F must be finished before step E can begin."
]

type Step = Step of string
type Requirement = {
    prerequisite:Step
    forStep:Step
}

let pattern = new Regex("Step (\w) must be finished before step (\w) can begin.")

let parse str =
    let isMatch = pattern.Match(str)
    if (isMatch.Success) then
        Some {
            prerequisite = Step isMatch.Groups.[1].Value
            forStep = Step isMatch.Groups.[2].Value 
        }
    else
        None

let addRequirement map step =
    let value = if Map.containsKey step.forStep map then map.[step.forStep] else []
    let value' = step.prerequisite::value
    Map.add step.forStep value' map

let toSteps list =
    list
    |> List.collect (fun x -> [x.prerequisite;x.forStep])
    |> List.distinct

let toChar step = match step with Step c -> c

let getPrerequisites step prereqMap =
    if Map.containsKey step prereqMap then
        prereqMap.[step] |> List.sort
    else
        []

let satisfied prereqs order = 
    // let cnt = List.length prereqs
    // cnt = List.length (prereqs |> List.filter (fun step -> List.contains step order))
    // cnt = List.length (prereqs |> List. (fun step -> List.contains step order))
    List.length (prereqs |> List.except order) = 0

let getNextAvailable order prereqMap =
    prereqMap 
    |> Map.toList
    |> List.fold (fun state (step,prereqs) ->
        if satisfied prereqs order then step::state else state
    ) []
    |> List.except order

// Answer: PFKQWJSVUXEMNIHGTYDOZACRLB
let solve1 input =
    let requirementList = input |> List.choose parse
    let steps = requirementList |> toSteps
    let prereqMap = requirementList |> List.fold addRequirement Map.empty
    let firstSteps = steps |> List.filter (fun step -> List.length (getPrerequisites step prereqMap) = 0) 

    let rec buildOrder order availableSteps =
        printfn "Current Order: %A; Available Steps: %A" order availableSteps
        match availableSteps |> List.distinct |> List.sort with
        | [] -> order
        | nextStep::available ->
            let order' = List.append order [nextStep]
            let nextAvailable = getNextAvailable order' prereqMap
            buildOrder order' (List.append nextAvailable available)

    let order = buildOrder [] firstSteps
    List.fold (fun a b -> a + (b |> toChar)) "" order