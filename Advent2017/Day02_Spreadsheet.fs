namespace Advent2017

module Day02_Spreadsheet =
    let CalculateChecksum (input:string list) = 
        let rows = [
            for line in input ->
                [for x in (line.Split(' ','\t')) -> x]
                |> List.map(fun x -> x |> int)
        ]
        let findMinMax row =
            let getMinMax (min,max) x = 
                match x with
                | a when x < min -> (a,max)
                | b when x > max -> (min,b)
                | _ -> (min,max)
            match row with 
            | x::xs -> Seq.fold getMinMax (x,x) row
            | [] -> (0,0)

        let difference (min,max) = max - min

        let checksum sumSoFar row = 
            let diff = findMinMax row |> difference
            sumSoFar + diff

        Seq.fold checksum 0 rows
        
    let CalculateChecksum2 (input:string list) = 
        let rows = [
            for line in input ->
                [for x in (line.Split(' ','\t')) -> x]
                |> List.map(fun x -> x |> int)
        ]
        let divisor (x,y) =
            match (x,y) with
            | (_,0)|(0,_) -> 0
            | (_,_) when x>y && x%y = 0 -> x/y
            | (_,_) when y>x && y%x = 0 -> y/x
            | (_,_) -> 0
        
        let checkForDivisor (head:int) (tail:int list) = 
            let divisorCheck divisorSoFar x = 
                //printfn "    Comparing %i and %i" head x
                match divisorSoFar with
                | 0 -> divisor (head,x)
                | _ -> divisorSoFar

            List.fold divisorCheck 0 tail
        
        let rec checkForDivisorRec (head:int) (tail:int list) = 
            //printfn "  Head %i" head
            let a = checkForDivisor head tail
            let b = 
                match tail with
                | [] -> 0
                | x::xs -> checkForDivisorRec x xs
            if a > 0 then a else b

        let checksum sumSoFar (row:int list) = 
            //printfn "new row %A" row
            let rowValue = 
                match row with 
                | [] -> 0
                | head::tail -> checkForDivisorRec head tail
            sumSoFar + rowValue

        Seq.fold checksum 0 rows

