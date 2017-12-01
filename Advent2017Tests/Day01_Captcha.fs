namespace Advent2017.Tests

open Advent2017
open NUnit.Framework

[<TestFixture>]
type Day01_Tests() = 

    let Test1 input expected =
        let actual = Day01_Captcha.GetCaptchaResult input
        printfn "The captcha result is %i. %i was expected." expected actual
        Assert.AreEqual(expected, actual)

    let Test2 input expected =
        let actual = Day01_Captcha.GetHalfCaptchaResult input
        printfn "The captcha result is %i. %i was expected." expected actual
        Assert.AreEqual(expected, actual)
        
    //1122 produces a sum of 3 (1 + 2) because the first digit (1) matches the second digit and the third digit (2) matches the fourth digit.
    //1111 produces 4 because each digit (all 1) matches the next.
    //1234 produces 0 because no digit matches the next.
    //91212129 produces 9 because the only digit that matches the next one is the last digit, 9.
    [<Test>]
    member this.Test_1122() = Test1 "1122" 3

    [<Test>]
    member this.Test_1111() = Test1 "1111" 4

    [<Test>]
    member this.Test_1234() = Test1 "1234" 0

    [<Test>]
    member this.Test_91212129() = Test1 "91212129" 9

    [<Test>]
    member this.Answer1() =
        // TODO: move to common location for use by other tests.
        let dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)
        let path = System.IO.Path.Combine(dir, "Visual Studio 2017", "Projects", "Advent", "Advent2017", "Resources", "Day01.txt")
        let input = System.IO.File.ReadAllText(path)
        Test1 input 1102
            
    //1212 produces 6: the list contains 4 items, and all four digits match the digit 2 items ahead.
    //1221 produces 0, because every comparison is between a 1 and a 2.
    //123425 produces 4, because both 2s match each other, but no other digit has a match.
    //123123 produces 12.
    //12131415 produces 4.

    [<Test>]
    member this.Test_1212() = Test2 "1212" 6

    [<Test>]
    member this.Test_1221() = Test2 "1221" 0

    [<Test>]
    member this.Test_123425() = Test2 "123425" 4

    [<Test>]
    member this.Test_123123() = Test2 "123123" 12

    [<Test>]
    member this.Test_12131415() = Test2 "12131415" 4

    [<Test>]
    member this.Answer2() =
        // TODO: move to common location for use by other tests.
        let dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)
        let path = System.IO.Path.Combine(dir, "Visual Studio 2017", "Projects", "Advent", "Advent2017", "Resources", "Day01.txt")
        let input = System.IO.File.ReadAllText(path)
        Test2 input 1076

