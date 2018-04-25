module FileSystemTest

open System.IO

type File = {Name : string ; Size : int64 ; Attributes : FileAttributes ; FullDir : string}
type Directory = {Name : string ; Path : string ; SubItems : FileSystemItem []}
and FileSystemItem = File of File | Directory of Directory


let fileInfoToFile (fileInfo : FileInfo)=
    File { 
    Name = fileInfo.Name
    Size = fileInfo.Length 
    Attributes = fileInfo.Attributes
    FullDir = fileInfo.FullName
    }

let fileNameToFile fileName =
    let fileInfo = new FileInfo(fileName)
    fileInfoToFile fileInfo

let rec dirInfoToDirectory (dirInfo : DirectoryInfo) =
    Directory {
    Name = dirInfo.Name
    Path = dirInfo.FullName 
    SubItems = Array.append 
        (dirInfo.GetFiles() |> Array.map fileInfoToFile) 
        (dirInfo.GetDirectories() |> Array.map dirInfoToDirectory)  
    }

let dirPathToDirectory dir = 
    let dirInfo = DirectoryInfo(dir)
    dirInfoToDirectory dirInfo

// call this:
let getFileSystem root = dirPathToDirectory root



let rec fsCata fFile fDir item : 'r =
    let recurse = fsCata fFile fDir
    match item with 
    | File f -> fFile f
    | Directory d -> 
        let listOfSubItems = d.SubItems |> Array.map recurse
        //fDir d.Name d.Path listOfSubItems // curried ar tupled?
        fDir (d.Name, d.Path, listOfSubItems)

