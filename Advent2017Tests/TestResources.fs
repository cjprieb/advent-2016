module TestResources

    let ReadAllLines fileName =
        let dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)
        let path = System.IO.Path.Combine(dir, "Visual Studio 2017", "Projects", "Advent", "Advent2017", "Resources", fileName)
        Array.toList (System.IO.File.ReadAllLines(path))
        |> List.map (fun line -> line.Trim())
