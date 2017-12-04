namespace Advent2017.Tests.Day03_SpiralMemory

open Advent2017
open NUnit.Framework

[<TestFixture>]
type Day03_SpiralMemory_Tests() = 

    let Test1 input expected =
        let actual = Day03_SpiralMemory.StepsToAccessPort input
        printfn "Data from square %i is carried %i steps. %i was expected." input actual expected
        Assert.AreEqual(expected, actual)

    let Test2 input expected =
        let actual = Day03_SpiralMemory.FirstValueLargerThanInput input
        printfn "The first value larger than %i is %i. %i was expected." input actual expected
        Assert.AreEqual(expected, actual)
        
    [<Test>]
    member this.Test1_1() = 
        Test1 1 0
        
    [<Test>]
    member this.Test1_12() = 
        Test1 12 3
        
    [<Test>]
    member this.Test1_23() = 
        Test1 23 2
        
    [<Test>]
    member this.Test1_1024() = 
        Test1 1024 31

    [<Test>]
    member this.Answer1() =
        let input = 265149
        Test1 input 438
        
    [<Test>]
    member this.Test2_1() = 
        Test2 0 1
        
    [<Test>]
    member this.Test2_5() = 
        Test2 4 5
        
    [<Test>]
    member this.Test2_23() = 
        Test2 11 23
        
    [<Test>]
    member this.Test2_747() = 
        Test2 747 806

    [<Test>]
    member this.Answer2() =
        let input = 265149
        Test2 input 266330