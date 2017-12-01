namespace Advent2017

module Day01_Captcha =
    let GetCaptchaResult (input:string) =
        let charAsInt c = (int c) - (int '0')

        let explode (s:string) = seq {
            for inx in [0..input.Length-1] -> 
                let next = (inx+1)%input.Length
                (input.[inx] |> charAsInt,input.[next] |> charAsInt)
        }

        let sumValues sumSoFar (current,next) =
            let value = (if current = next then current else 0)
            //printfn "curr=%i next=%i value=%i" current next value
            sumSoFar + value

        Seq.fold sumValues 0 (explode input)
        
    let GetHalfCaptchaResult (input:string) =
        let charAsInt c = (int c) - (int '0')

        let explode (s:string) = seq {
            for inx in [0..input.Length-1] -> 
                let next = (inx+input.Length/2)%input.Length
                (input.[inx] |> charAsInt,input.[next] |> charAsInt)
        }

        let sumValues sumSoFar (current,next) =
            let value = (if current = next then current else 0)
            //printfn "curr=%i next=%i value=%i" current next value
            sumSoFar + value

        Seq.fold sumValues 0 (explode input)
