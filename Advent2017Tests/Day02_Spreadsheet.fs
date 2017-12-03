namespace Advent2017.Tests.Day02_Spreadsheet

open Advent2017
open NUnit.Framework

[<TestFixture>]
type Day01_Tests() = 

    let Test1 input expected =
        let actual = Day02_Spreadsheet.CalculateChecksum input
        printfn "The spreadsheet checksum result is %i. %i was expected." expected actual
        Assert.AreEqual(expected, actual)

    let Test2 input expected =
        let actual = Day02_Spreadsheet.CalculateChecksum2 input
        printfn "The spreadsheet checksum result is %i. %i was expected." expected actual
        Assert.AreEqual(expected, actual)
        
    [<Test>]
    member this.Test1_3_Rows() = 
        Test1 ["5 1 9 5";"7 5 3";"2 4 6 8"] 18

    [<Test>]
    member this.Answer1() =
        let input = TestResources.ReadAllLines "Day02.txt"
        Test1 input 41919
        
    [<Test>]
    member this.Test2_3_Rows() = 
        Test2 ["5 9 2 8";"9 4 7 3";"3 8 6 5"] 9

    [<Test>]
    member this.Answer2() =
        let input = TestResources.ReadAllLines "Day02.txt"
        Test2 input 303