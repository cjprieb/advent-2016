module Day04_Passphrase
    type IsPassphraseValid = Valid | Invalid
    let toBool = 
        function
        | Valid -> true
        | Invalid -> false

    let testCases1 = [
        "aa bb cc dd ee", Valid
        "aa bb cc dd aa", Invalid
        "aa bb cc dd aaa", Valid
    ]

    let testCases2 = [
        "abcde fghij", Valid
        "abcde xyz ecdab", Invalid
        "a ab abc adb abf abj", Valid
        "iiii oiii ooii ooooi oooo", Valid
        "oiii ioii iioi iiio", Invalid
    ]

    let isPassphraseValid (phrase:string) =
        let duplicateWordCount = 
            phrase.Split(' ')
            |> List.ofArray
            |> List.groupBy (fun word -> word)
            |> List.filter (fun (_,words) -> words.Length > 1)
            |> List.length
        if duplicateWordCount = 0 then Valid else Invalid

    let sortWord (word:string) =
        word.ToCharArray() |> List.ofArray |> List.sort

    let isPassphraseValid_v2 (phrase:string) =
        let duplicateWordCount = 
            phrase.Split(' ')
            |> List.ofArray
            |> List.map sortWord
            |> List.groupBy (fun word -> word)
            |> List.filter (fun (_,words) -> words.Length > 1)
            |> List.length
        if duplicateWordCount = 0 then Valid else Invalid

    let checkTests test testCases = 
        List.iter(
            fun (input, expected) ->
                let answer = test input
                match (answer,expected) with
                | Valid,Valid -> printfn "%s is valid" input
                | Invalid,Invalid -> printfn "%s is invalid" input
                | Invalid,Valid -> printfn "%s is marked as invalid when it should be valid" input
                | Valid,Invalid -> printfn "%s is marked as valid when it should be invalid" input
        ) testCases |> ignore

    //checkTests isPassphraseValid testCases1 isPassphraseValid;;
    //checkTests isPassphraseValid_v2 testCases2;;
    
    let answer1test = 
        let numberOfValidPassphrases = 
            TestResources.ReadAllLines "Day04.txt"
            |> List.filter (fun line -> (isPassphraseValid line) |> toBool)
            |> List.length
        printfn "%i passphrases are valid" numberOfValidPassphrases
    
    let answer2test = 
        let numberOfValidPassphrases = 
            TestResources.ReadAllLines "Day04.txt"
            |> List.filter (fun line -> (isPassphraseValid_v2 line) |> toBool)
            |> List.length
        printfn "%i passphrases are valid" numberOfValidPassphrases