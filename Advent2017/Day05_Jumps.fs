module Day05_Jumps

    let testInstructions = [|0;3;0;1;-3|]

    let jump index (instructions:int array) =
        let jump = instructions.[index]
        instructions.[index] <- jump+1
        (index+jump, instructions)

    let strangeJump index (instructions:int array) =
        let jump = instructions.[index]
        instructions.[index] <- if jump >= 3 then jump-1 else jump+1
        (index+jump, instructions)

    let calculateJumpsToEscape jumpMethod instructions =
        let instructionsCopy = Array.copy instructions
        let rec jumpsToEscapeRec acc (index,instructions) =
            if index >= Array.length instructions then
                acc
            else
                let (index', instructions') = jumpMethod index instructions
                jumpsToEscapeRec (acc+1) (index',instructions')

        jumpsToEscapeRec 0 (0,instructionsCopy)

    let test jumpMethod expected input =
        let answer = calculateJumpsToEscape jumpMethod input
        printfn "Steps to escape"
        printfn "Expected: %i" expected
        printfn "Answer  : %i" answer

    // test jump 5 testInstructions;;
    // test jump 373543 Day05_Input.instructions;;

    // test strangeJump 10 testInstructions;;
    // test strangeJump 27502966 Day05_Input.instructions;;


    