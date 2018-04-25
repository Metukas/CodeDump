module MutualTypes

type Sheet private (rows) =  
  // The main constructor is 'private' and so users do not see it,
  // it takes columns and calculates the maximal column length
  let width = rows |> Seq.map Seq.length |> Seq.fold max 0

  // The default constructor calls the main one with empty ResizeArray
  new() = Sheet(ResizeArray<_>())

  // An alternative constructor loads data from the file
  new(fileName:string) =
    let lines = System.IO.File.ReadLines fileName 
    Sheet(ResizeArray<_> [ for line in lines -> ResizeArray<_> (line.Split ',') ])

type IPrintable =
    abstract member Print : unit -> unit

type Vector3OO (x, y, z) = 
    let mutable X = x
    let mutable Y = y
    let mutable Z = z 
    let mutable test = 0
    static let mutable vectorCount = 0
    do
        test <- x + y + z
        vectorCount <- vectorCount + 1 
    interface IPrintable with
        member this.Print() = printfn "(%i, %i, %i)" X Y Z

    member this.Test = test
    static member Count = vectorCount
    new(x, y) = Vector3OO(x, y, 0)
    new() = Vector3OO(0,0,0)


