module Day08_Registers
    open System.Text.RegularExpressions

    type Value = Value of int
    type Register = Register of string
    type Input = RegisterInput of Register | ValueInput of Value
    type Comparison = (Map<Register,Value> -> bool)
    type Action = (Map<Register,Value> -> Map<Register,Value>)
    type Instruction = {register:Register;action:Action;comparison:Comparison}

    let getRegister (register) (map) = 
        if Map.containsKey register map then 
            map.[register] 
        else 
            Value(0)
    
    let getValue input map =
        match input with 
        | ValueInput (i) -> i
        | RegisterInput (r) -> getRegister r map

    let update f a b map =
        let (Value a') = getRegister a map
        let (Value b') = b
        let newValue = Value (f a' b')
        Map.add a newValue map

    let inc a b map = update (+) a b map

    let dec a b map = update (-) a b map
    
    let parseInput a =
        let (success,value) = System.Int32.TryParse a
        if success then ValueInput (Value value) else RegisterInput (Register a)

    module ComparisonModule = 
        type Comparer = NotEqual | Equal | GreaterThan | GreaterThanOrEqual | LessThan | LessThanOrEqual

        let parseComparer op =
            match op with 
            | "!=" -> NotEqual
            | "==" -> Equal
            | ">" -> GreaterThan
            | ">=" -> GreaterThanOrEqual
            | "<" -> LessThan
            | "<=" -> LessThanOrEqual
            | _ -> failwith (invalidArg "op" (sprintf "Operater %s is invalid" op))

        let compareValues (compareType:Comparer) a b =
            match compareType with 
            | NotEqual -> 
                a <> b
            | Equal -> 
                a = b
            | GreaterThan -> 
                a > b
            | GreaterThanOrEqual -> 
                a >= b
            | LessThan -> 
                a < b
            | LessThanOrEqual -> 
                a <= b

    let compare compareType a b map =
        let a' = getValue a map
        let b' = getValue b map
        ComparisonModule.compareValues compareType a' b'

    let maxRegister map = 
        map 
        |> Map.toSeq 
        |> Seq.map (fun (_,v) -> v) 
        |> Seq.max

    let parse (list,map) line =
        let match' = Regex.Match(line, @"(\w+) (inc|dec) ([-\d]+) if (\w+) (..?) ([-\w]+)")
        if match'.Success then
            let r = match'.Groups.[1].Value |> Register
            
            let actionType = match'.Groups.[2].Value
            let actionValue = match'.Groups.[3].Value |> int |> Value
            let actionMethod = (if actionType = "inc" then inc r actionValue else dec r actionValue)

            let a = match'.Groups.[4].Value |> parseInput
            let op = match'.Groups.[5].Value |> ComparisonModule.parseComparer
            let b = match'.Groups.[6].Value |> parseInput
            let compareMethod = compare op a b

            let instruction = {register=r;action=(fun map -> actionMethod map);comparison=(fun map -> compareMethod map)}
            let newList = List.append list [instruction]
            if Map.containsKey r map then 
                (newList,map)
            else
                (newList,Map.add r (Value 0) map)
        else 
            printfn "%s did not match pattern" line
            (list,map)
            
    let processLines lines =
        let rec parseRec (list,map) lines =
            match lines with
            | [] -> 
                (list,map)
            | x::xs -> 
                let (list',map') = parse (list,map) x
                parseRec (list',map') xs

        let rec processRec (map,max) (instructions:Instruction list) =
            match instructions with
            | [] -> 
                (map,max)
            | x::xs -> 
                let map' = if x.comparison map then x.action map else map
                let max' = List.max [maxRegister map';max]
                processRec (map',max') xs

        let (list,map) = parseRec ([],Map.empty) lines
        let (map',max') = processRec (map,(Value 0)) list        
        let finalMax = maxRegister map'
        let (Value answer1, Value answer2) = (finalMax, max')
        (answer1,answer2)
        
    let testInput = [
        "b inc 5 if a > 1"
        "a inc 1 if b < 5"
        "c dec -10 if a >= 1"
        "c inc -20 if c == 10"
    ]
    
    let test testMethod expected input =
        let answer = testMethod input
        if answer = expected then
            printfn "Success! Result was %A and %A was expected" answer expected
        else
            printfn "Failed! Result was %A, but %A was expected" answer expected
    
    //test (processLines) (1,10) (testInput);;
    //test (processLines) (5075,7310) (actualInput |> List.ofArray);;
