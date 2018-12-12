open System.Text.RegularExpressions

// 419 players; last marble is worth 72164 points
let input = (419,72164)

let sample = (9,25)

let samples = [
    ((10,1618),8317)
    ((13,7999),146373)
    ((17,1104),2764)
    ((21,6111),54718)
    ((30,5807),37305)
]

// type Marble = Marble of int
// let getMarbleValue marble = match marble with | Marble i -> i

// type Player = Player of int
// let getPlayerValue player = match player with | Player i -> i

type Game = {
    currentIndex:int
    marbles:int list
    playerScores:Map<int,int>
    currentPlayer:int    
}

let getIndexToInsertAt game = 
    let length = List.length game.marbles
    let i = (game.currentIndex + 2) % length
    if i = 0 then length else i

let getIndexToRemoveAt game = 
    let length = List.length game.marbles
    let i = (game.currentIndex - 7) % length
    if i < 0 then i + length else i

let insertAt index x list =
    // printfn "  Inserting %A at %A" x index
    // let (_,list') =
    //     Seq.fold (fun (i,partial) a -> 
    //         let i' = i+1
    //         let additions = if i' = index then [a;x] else [a]
    //         (i',(Seq.append partial additions))
    //     ) (0,Seq.empty) list
    // list'
    let (part1,part2) = List.splitAt index list
    // printfn "   %A [%A] %A" part1 x part2
    List.concat [part1;[x];part2]
    // let length = List.length list
    // seq {
    //     for i in 0..length do 
    //         if i = index then yield x
    //         else if i > index then yield list.[i-1]
    //         else yield list.[i]
    // }
    // |> Seq.toList

let removeAt index list =
    // let (_,removed,list') =
    //     Seq.fold (fun (i,removed,partial) a -> 
    //         let i' = i+1
    //         if i = index then (i',a,partial)
    //         else (i',removed,Seq.append partial [a])
    //     ) (0,0,Seq.empty) list
    // removed,list'
    let length = (List.length list) - 2
    let list' =
        seq {
            for i in 0..length do 
                if i >= index then yield list.[i+1]
                else yield list.[i]
        }
        |> Seq.toList
    (list.[index],list')

let updateScore player score (players:Map<int,int>) = Map.add player (players.[player] + score) players

let initializeScore players = List.fold (fun map p -> Map.add p 0 map) Map.empty players

let getNextPlayer players current =
    let i = List.findIndex (fun p -> p = current) players
    let i' = (i + 1) % (List.length players)
    players.[i']


let finishTurn index marbles scores nextPlayer = 
    let list = marbles |> List.ofSeq
    // printfn "Marbles: %A" list
    {
        currentIndex=index
        marbles=marbles
        playerScores=scores
        currentPlayer=nextPlayer
    }

let extractMarbles game =
    let removeIndex = getIndexToRemoveAt game
    let (marble,newCircle) = removeAt removeIndex game.marbles
    let index' = (removeIndex) % (List.length newCircle)
    // printfn "  Removed %A: %A" marble newCircle
    (marble,newCircle,index')
    
let normalAction players game marble =
    let insertIndex = getIndexToInsertAt game
    let newCircle = insertAt insertIndex marble game.marbles
    let nextPlayer = getNextPlayer players game.currentPlayer
    finishTurn insertIndex newCircle game.playerScores nextPlayer

let specialAction players game marble =
    let (removedMarble,newCircle,newIndex) = extractMarbles game
    // printfn "Adding %A and %A to %A" marble removedMarble game.currentPlayer
    let score = marble + removedMarble
    let players' = updateScore game.currentPlayer score game.playerScores
    let nextPlayer = getNextPlayer players game.currentPlayer
    finishTurn newIndex newCircle players' nextPlayer


let turn players game nextMarble =
    if (nextMarble % 23 = 0) then
        specialAction players game nextMarble
    else
        normalAction players game nextMarble

let solve1 (players,lastMarbleValue) =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let players = [1..players]
    let startGame = {
        currentIndex = 0
        marbles = [0]
        playerScores = initializeScore players
        currentPlayer = 1
    }

    let endGame =
        [1..lastMarbleValue]
        |> List.fold (fun game index ->
            turn players game index
        ) startGame
        
    stopWatch.Stop()
    printfn "%f ms" stopWatch.Elapsed.TotalMilliseconds

    endGame.playerScores
    |> Map.toList
    |> List.map (fun (_,score) -> score)
    |> List.max

let test samples =
    List.iter (fun ((players,last),expected) ->
        let actual = solve1 (players,last)
        if (actual = expected) then
            printfn "Success! %A players and %A last marble worth is %A" players last expected
        else 
            printfn "Fail! Expected %A, but got %A (%A players and %A last marble worth)" expected actual players last
    ) samples

// 1042.154200 ms
// Success! 10 players and 1618 last marble worth is 8317
// 124583.731600 ms
// Success! 13 players and 7999 last marble worth is 146373
// 388.604100 ms
// Success! 17 players and 1104 last marble worth is 2764
// 55704.102500 ms
// Success! 21 players and 6111 last marble worth is 54718
// 48190.941800 ms
// Success! 30 players and 5807 last marble worth is 37305


// 62.787500 ms
// Success! 10 players and 1618 last marble worth is 8317
// 5528.432900 ms
// Success! 13 players and 7999 last marble worth is 146373
// 21.701900 ms
// Success! 17 players and 1104 last marble worth is 2764
// 2560.149600 ms
// Success! 21 players and 6111 last marble worth is 54718
// 2132.773000 ms
// Success! 30 players and 5807 last marble worth is 37305