module RecursiveCircus

    open System.Text.RegularExpressions

    let testInput = [
        "pbga (66)"
        "xhth (57)"
        "ebii (61)"
        "havc (66)"
        "ktlj (57)"
        "fwft (72) -> ktlj, cntj, xhth"
        "qoyq (66)"
        "padx (45) -> pbga, havc, qoyq"
        "tknk (41) -> ugml, padx, fwft"
        "jptl (61)"
        "ugml (68) -> gyxo, ebii, jptl"
        "gyxo (61)"
        "cntj (57)"
    ]

    // answer for test = tknk 
    // ugml + (gyxo + ebii + jptl) = 68 + (61 + 61 + 61) = 251
    // padx + (pbga + havc + qoyq) = 45 + (66 + 66 + 66) = 243
    // fwft + (ktlj + cntj + xhth) = 72 + (57 + 57 + 57) = 243

    type Disk = {name:string;weight:int;supporting:string list}
    type Tower = {disk:Disk;supportingWeight:int list;parent:Disk option}

    let totalWeight (tower:Tower) = tower.disk.weight + (List.sum tower.supportingWeight)

    let getKeys map = map |> Map.toSeq |> Seq.map (fun (k,v) -> k)

    let addTower (disk:Disk) (allParents:Map<string,Disk>) (unprocessedDisks:Disk list,allTowers:Map<string,Tower>) =
        let parent =
            if Map.containsKey disk.name allParents then
                Some allParents.[disk.name]
            else
                None
        if (disk.supporting |> List.filter (fun n -> not (Map.containsKey n allTowers)) |> List.length) = 0 then
            let supporting = 
                disk.supporting
                |> List.map (fun name -> 
                    if Map.containsKey name allTowers then
                        totalWeight allTowers.[name]
                    else 
                        //printfn "  %s not present in allTowers %A" name (getKeys allTowers)
                        -1
                )

            let tower = {disk=disk;supportingWeight=supporting;parent=parent}
            //printfn "  Adding tower for %s" disk.name
            (unprocessedDisks,Map.add disk.name tower allTowers)
        else 
            (disk::unprocessedDisks,allTowers)

    let parseTestCases = [
        "pbga (66)",{name="pbga";weight=66;supporting=[]}
        "xhth (57)",{name="xhth";weight=57;supporting=[]}
        "fwft (72) -> ktlj, cntj, xhth",{name="fwft";weight=72;supporting=["ktlj";"cntj";"xhth"]}
    ]

    let parse line =
        let match' = Regex.Match(line, @"(\w+) \((\d+)\)(?: -> (.+))?");
        if match'.Success then
            let name = match'.Groups.[1].Value
            let weight = match'.Groups.[2].Value |> int
            let supporting = 
                match match'.Groups.[3].Value with
                | "" -> []
                | x -> x.Split(',') |> List.ofArray |> List.map (fun s -> s.Trim())
            Some {name=name;weight=weight;supporting=supporting}
        else
            None

    let parseAllText input = input |> List.map parse |> List.choose (fun x -> x)

    let plural count singularWord pluralWord = if count = 1 then singularWord else pluralWord

    let collectChildDisks (children:Map<string,Disk>) (parent:Disk) =
        if parent.supporting.Length = 0 then
            children
        else 
            List.fold (fun children' item -> Map.add item parent children') children parent.supporting
            
    let collectAllChildDisks disks = List.fold collectChildDisks Map.empty disks

    let getRootName lines =
        let disks = parseAllText lines
        let childDisks = collectAllChildDisks disks
        let isChildDisk disk = Map.containsKey disk.name childDisks

        let tmp = disks |> List.filter (fun d -> isChildDisk d |> not)
        let length = List.length tmp
        //printfn "%i %s found that aren't children themselves" length (plural length "disk" "disks")

        Some (tmp |> List.head).name


    let buildTower lines =
        let disks = parseAllText lines
        let parents = collectAllChildDisks disks
        
        let createTowers disks allTowers = 
            disks 
            |> List.fold (fun state d -> addTower d parents state) ([],allTowers)

        let isBalanced (tower:Tower) = 
            match tower.supportingWeight with
            | [] -> 
                (true,tower,0)
            | _ ->
                let weights = List.groupBy (fun x -> x) tower.supportingWeight
                if weights.Length = 1 then
                    (true, tower, 0)
                else
                    let max = 
                        weights 
                        |> List.map (fun (_,x) ->  List.length x) 
                        |> List.max
                    let (expectedWeight,_) = 
                        weights 
                        |> List.filter (fun (_,x) -> max = List.length x) 
                        |> List.head
                    //let (wrongWeight,_) = 
                    //    weights 
                    //    |> List.filter (fun (_,x) -> 1 = List.length x) 
                    //    |> List.head
                    (false,tower,expectedWeight)
        
        let rec getUnbalanced disks allTowers =
            //printfn "Looping through %A" (List.map (fun d -> d.name) disks)
            let (unprocessed,allTowers') = createTowers disks allTowers
            let towers = 
                disks 
                |> List.map (fun d -> if Map.containsKey d.name allTowers' then Some allTowers'.[d.name] else None)
                |> List.choose (fun t -> t)

            let unbalanced = 
                towers 
                |> List.map isBalanced 
                |> List.filter (fun (balanced,_,_) -> balanced |> not)
            
            match unbalanced with
            | [] ->
                //keep going
                let nextDisks = List.append unprocessed (towers |> List.choose (fun t -> t.parent)) |> List.distinct
                match nextDisks with
                | [] ->
                    0 // at the root and apparently all are balanced
                | _ ->
                    getUnbalanced nextDisks allTowers'

            | x ->
                let (_,tower,expectedWeight) = List.head x
                let imbalancedTower = 
                    tower.disk.supporting
                    |> List.map (fun name -> allTowers'.[name])
                    |> List.filter (fun t -> (totalWeight t) <> expectedWeight)
                    |> List.head
                //printfn "%s (%i) is unbalanced: %A" tower.disk.name tower.disk.weight tower.supportingWeight
                //let printSubTowers = 
                //    List.iter (fun name ->
                //        let t = allTowers'.[name]
                //        printfn "  %s (%i) supports %A" t.disk.name t.disk.weight t.supportingWeight
                //    ) 
                //printSubTowers tower.disk.supporting
                let weight = totalWeight imbalancedTower
                imbalancedTower.disk.weight + (expectedWeight - weight)
        
        let leaves = disks |> List.filter (fun d -> d.supporting.Length = 0) 
        Some (getUnbalanced leaves Map.empty)

    //let getWeight (disk:Disk) allChildren =
    //    let rec getWeightRec sum disk' allChildren' =
    //        match disk'.supporting with
    //        | [] ->
    //            sum + disk.weight
    //        | _ -> 
    //            sum + disk.weight
        
    //    getWeightRec 0 disk allChildren

    
    let test method cases =
        cases
        |> List.iter (fun (input,expected) ->
            let answer = method input
            match answer with 
            | None -> printfn "Failed! %A returned None. %A was expected" input expected
            | Some x when x = expected ->
                printfn "Success! Answer is %A." x
            | Some x ->
                printfn "Failed! Answer is %A, but %A was expected" x expected
        ) 
    
    let solutionInput =
        let path = @"C:\Users\priebc\Documents\Visual Studio 2017\Projects\FSharpTestLibrary\FSharpTestLibrary\RecursiveCircusInput.txt"
        System.IO.File.ReadAllLines(path) |> List.ofArray

    //let t1 = test parse parseTestCases;;

    //let t2 = test getRootName [(testInput,"tknk")];;

    //let t3 = test getRootName [(solutionInput,"ahnofa")];;

    //let t4 = test buildTower [(testInput,243)];;

    // 1786 is too high
    //let t5 = test buildTower [(solutionInput,802)];;
