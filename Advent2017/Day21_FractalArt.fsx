type Rule = {pattern:char[,];enhancement:char[,]}

let toString pattern =
    let xLength = Array2D.length1 pattern
    let yLength = Array2D.length2 pattern    
    [0..(yLength-1)] 
    |> List.map (fun y ->
        [0..(xLength-1)]
        |> List.map (fun x -> sprintf "%c" pattern.[x,y])
        |> List.reduce (+)
    )
    |> List.reduce (fun state line -> sprintf "%s\n %s" state line)

let rotate pattern =
    let size = Array2D.length1 pattern
    Array2D.init size size (fun x y ->
        pattern.[y,size-x-1]
    )

let flipHorizontal pattern =
    let size = Array2D.length1 pattern
    Array2D.init size size (fun x y ->
        pattern.[(size-x-1),y]
    )
    
//let flipVertical pattern =
//    let size = Array2D.length1 pattern
//    Array2D.init size size (fun x y ->
//        pattern.[x,(size-y-1)]
//    )

let rotate2 pattern = rotate (rotate pattern) 
let rotate3 pattern = rotate (rotate2 pattern) 

let rotateFlip pattern = rotate (flipHorizontal pattern) 
let rotate2Flip pattern = rotate2 (flipHorizontal pattern) 
let rotate3Flip pattern = rotate3 (flipHorizontal pattern) 

let isMatch pattern rule =
    [
        (fun x -> x)
        rotate
        rotate2
        rotate3
        flipHorizontal
        rotateFlip
        rotate2Flip
        rotate3Flip
    ]
    |> List.exists (fun t -> t pattern = rule.pattern)
    
let applyEnhancement rulebook pattern =
    let matchingRules = rulebook |> List.filter (isMatch pattern)
    //let rule = rulebook |> List.find (isMatch pattern)
    if List.length matchingRules > 0 then
        let result = (List.head matchingRules).enhancement
        //printfn "\n %s -> \n %s" (toString pattern) (toString result)
        result
    else 
        failwith (sprintf "No matching rules for %s" (toString pattern))

let breakInto pattern size =
    let n = (Array2D.length1 pattern) / size
    //printfn "  dividing pattern into %ix%i squares of size %i" size size n

    Array2D.init n n (fun xOffset yOffset ->
        let square = 
            Array2D.init size size (fun x' y' ->
                let x = (size*xOffset) + x'
                let y = (size*yOffset) + y'
                pattern.[x,y]
            )
        //printfn "  Break (%i,%i)\n%A" xOffset yOffset (toString square)
        square
    )

let joinAll (squares:char[,][,]) =
    let n = (Array2D.length1 squares)
    let squareSize = Array2D.length1 squares.[0,0]
    let size = n * squareSize
    printfn "  squareCount=%i squareSize=%i newSize=%i" n squareSize size

    Array2D.init size size (fun x y ->
        let xSquare = (x / squareSize)
        let ySquare = (y / squareSize) 
        let x' = x % squareSize
        let y' = y % squareSize
        //printfn "(%i,%i) in square=(%i,%i) box=(%i,%i)" x y xSquare ySquare x' y'
        let value = squares.[xSquare,ySquare].[x',y']
        value
    )


let to2dArray item =
    let array = 
        item 
        |> List.map (fun row -> row |> Array.ofSeq)
        |> Array.ofList
    let yLength = array.Length
    let xLength = if yLength > 0 then array.[0].Length else 0
    Array2D.init xLength yLength (fun x y -> array.[y].[x])

let parse (rule:string) = rule.Split('/') |> List.ofArray |> to2dArray

let startPattern = [".#.";"..#";"###"] |> to2dArray
let testPattern = ["#.#.";"....";"#.#.";"...."] |> to2dArray
let samePatterns = 
    [|
        [".#.";"..#";"###"]
        [".#.";"#..";"###"]
        ["#..";"#.#";"##."]
        ["###";"..#";".#."]
        ["###";"#..";".#."]
    |] |> Array.map to2dArray

//isMatch samePatterns.[0] testRulebook.[1];;
let testRulebook = [
    "../.#","##./#../..."
    ".#./..#/###","#..#/..../..../#..#"
    //{pattern=(parse "#..#/..../#..#/.##.");enhancement=(parse "")}
]

// iterate (testRulebook) samePatterns.[0];;
let iterate rulebook pattern =
    let size = Array2D.length1 pattern
    let squares =
        if size % 2 = 0 then
            breakInto pattern 2 
        else 
            breakInto pattern 3 
    squares
    |> Array2D.map (applyEnhancement rulebook)
    |> joinAll    

let countBy f array =
    let xSize = Array2D.length1 array
    let ySize = Array2D.length2 array
    let coords = 
        [0..(xSize-1)] 
        |> List.map (fun x -> [0..(ySize-1)] |> List.map (fun y -> (x,y)))
        |> List.concat
    coords
    |> List.fold (fun cnt (x,y) -> if f array.[x,y] then cnt+1 else cnt) 0

// iterateN 2 testRulebook samePatterns.[0];;
// iterateN 12 rulebook startingPattern;; // 197
let iterateN n rulebookRaw pattern =
    let rulebook = rulebookRaw |> List.map (fun (a,b) -> {pattern=(parse a);enhancement=(parse b)})
    let rec loop n p =
        let count = p |> countBy (fun ch -> ch = '#')
        //printfn "%i - %i on\n%A" n count (toString p)
        if n = 0 then
            count
        else
            loop (n-1) (iterate rulebook p)
    loop n pattern

// testChange (flipHorizontal) samePatterns.[0] samePatterns.[1];;
// testChange (rotate) samePatterns.[0] samePatterns.[2];;
// testChange (rotate) samePatterns.[2] samePatterns.[4];;
let testChange method pattern expected =
    let actual = method pattern
    if actual = expected then
        printfn "Success! \n%A returned \n%A" (toString pattern) (toString actual)
    else
        printfn "Failed! \n%A returned \n%A, but \n%A was expected" (toString pattern) (toString actual) (toString expected)

let rulebook = [
    "../..","#.#/##./..#"
    "#./..","###/.##/..#"
    "##/..","..#/.#./##."
    ".#/#.","###/.##/###"
    "##/#.","###/#.#/.##"
    "##/##","#.#/..#/.#."
    ".../.../...","..../.#../##.#/#.#."
    "#../.../...",".##./#.../.##./#..#"
    ".#./.../...","...#/.#.#/###./##.#"
    "##./.../...","#.##/..#./.#.#/..##"
    "#.#/.../...","..#./.#../.#.#/###."
    "###/.../...","#.#./.#../.#../...."
    ".#./#../...","..#./##../.###/###."
    "##./#../...","..#./###./#.#./#.#."
    "..#/#../...","..##/###./.#.#/#..."
    "#.#/#../...","#.../...#/.#.#/#..."
    ".##/#../...","###./####/.###/#.##"
    "###/#../...","#.../#.##/#.../.#.#"
    ".../.#./...",".##./#.#./#..#/..#."
    "#../.#./...","#.../##.#/#.#./.##."
    ".#./.#./...","##../.###/####/...."
    "##./.#./...","#.#./..../###./.#.#"
    "#.#/.#./...","..../..../#.##/.##."
    "###/.#./...","####/#.##/.###/#.#."
    ".#./##./...","####/#..#/#.##/.##."
    "##./##./...",".#.#/#.##/####/.###"
    "..#/##./...",".##./...#/.#.#/..#."
    "#.#/##./...","#..#/...#/.#../.##."
    ".##/##./...","##../#..#/##../..##"
    "###/##./...","..##/..../#.../..##"
    ".../#.#/...","###./#.../##.#/.#.#"
    "#../#.#/...","..#./...#/#..#/#.##"
    ".#./#.#/...","##../..#./##../###."
    "##./#.#/...",".#.#/#.#./####/.##."
    "#.#/#.#/...",".##./.##./#.##/#..#"
    "###/#.#/...","#..#/.##./..#./##.."
    ".../###/...","###./#..#/.###/#.##"
    "#../###/...","#.../#..#/####/##.."
    ".#./###/...","###./.##./#..#/.###"
    "##./###/...","#..#/##../.##./#.#."
    "#.#/###/...","..#./...#/#.../...#"
    "###/###/...","...#/##../...#/#.##"
    "..#/.../#..","##.#/.#.#/.##./###."
    "#.#/.../#..","###./#..#/.#.#/#.##"
    ".##/.../#..","...#/.#.#/.###/###."
    "###/.../#..",".#../...#/..#./.#.."
    ".##/#../#..",".#../...#/.##./..#."
    "###/#../#..",".###/##.#/#.##/.###"
    "..#/.#./#..","##.#/##../##../#..."
    "#.#/.#./#..","#.../.###/#.#./#..."
    ".##/.#./#..","###./#.##/###./####"
    "###/.#./#..",".#../..##/##.#/##.#"
    ".##/##./#..","##.#/##../.##./...#"
    "###/##./#..",".#.#/.#../####/.##."
    "#../..#/#..","..##/###./...#/##.."
    ".#./..#/#..",".#../...#/.#../..##"
    "##./..#/#..","###./..##/###./.##."
    "#.#/..#/#..","####/.#.#/...#/..##"
    ".##/..#/#..","#..#/.#../#.##/####"
    "###/..#/#..",".#../#.##/#.##/.#.."
    "#../#.#/#..","..#./#.##/.#../.##."
    ".#./#.#/#..","##../#.../#.#./###."
    "##./#.#/#..","#..#/.##./####/.#.."
    "..#/#.#/#..","##.#/..#./..#./.#.#"
    "#.#/#.#/#..",".#../..#./..#./..##"
    ".##/#.#/#..","##../#.##/#.#./#.##"
    "###/#.#/#..","##.#/..##/##../##.#"
    "#../.##/#..",".###/####/#.##/..##"
    ".#./.##/#..","#.#./.##./###./#.##"
    "##./.##/#..","..#./#..#/####/...#"
    "#.#/.##/#..","####/.#.#/##../##.#"
    ".##/.##/#..","#.#./#..#/.#.#/.##."
    "###/.##/#..",".#../.##./.##./.###"
    "#../###/#..","#..#/###./##.#/##.."
    ".#./###/#..","#.#./#..#/..#./#..#"
    "##./###/#..","..../##.#/####/...#"
    "..#/###/#..","..../#.../##../#..#"
    "#.#/###/#..","..#./.#../..../##.#"
    ".##/###/#..","#..#/###./##.#/.###"
    "###/###/#..","#.../.##./#.##/.##."
    ".#./#.#/.#.","...#/#.../.#../##.#"
    "##./#.#/.#.",".#.#/#.#./.#../#.##"
    "#.#/#.#/.#.","#.##/.##./###./...."
    "###/#.#/.#.","##../#..#/#.../.###"
    ".#./###/.#.","###./#.../.#../#..#"
    "##./###/.#.","##../##../#.../#..."
    "#.#/###/.#.","##../.#.#/#.##/#.#."
    "###/###/.#.","#.##/##.#/#.#./#..."
    "#.#/..#/##.","..../..#./####/..##"
    "###/..#/##.","#.../...#/#.#./#.#."
    ".##/#.#/##.","..##/###./.##./#..."
    "###/#.#/##.",".#../###./##.#/...#"
    "#.#/.##/##.",".###/##../.###/..#."
    "###/.##/##.",".#.#/##.#/.##./.###"
    ".##/###/##.","..#./.#.#/.#../#..#"
    "###/###/##.","###./#..#/####/...#"
    "#.#/.../#.#",".#.#/.#../.#.#/#..."
    "###/.../#.#","#..#/##../.#../...#"
    "###/#../#.#","..../.#../#.../..##"
    "#.#/.#./#.#","#.#./####/.#.#/.##."
    "###/.#./#.#","..#./####/#..#/..##"
    "###/##./#.#",".##./.#../#.##/.#.#"
    "#.#/#.#/#.#","##../..##/##.#/#.#."
    "###/#.#/#.#",".##./#..#/#..#/.#.#"
    "#.#/###/#.#","..#./.###/#.##/#.##"
    "###/###/#.#","###./###./.#.#/###."
    "###/#.#/###","#.##/..##/#..#/...#"
    "###/###/###","...#/.#../##.#/.##."
]