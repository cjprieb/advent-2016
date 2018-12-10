module TestResources

    let getResource filename =
        let dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)
        let path = System.IO.Path.Combine(dir, "Visual Studio 2017", "Projects", "Advent", "Advent2018", "Resources", filename)
        path


    let ReadAllLines fileName =
        let path = getResource fileName
        Array.toList (System.IO.File.ReadAllLines(path))
        |> List.map (fun line -> line.Trim())

    let ReadAllLinesRaw fileName =
        let path = getResource fileName
        System.IO.File.ReadAllLines(path)

    let ReadAllText fileName =
        let path = getResource fileName
        System.IO.File.ReadAllText(path)

    let WriteAllLines fileName lines =
        let path = getResource fileName
        System.IO.File.WriteAllLines(path,lines |> Seq.toArray)

