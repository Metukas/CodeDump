module Vector3

type Vector3 = {X : float; Y : float ; Z : float}

let getX vec = vec.X
let getY vec = vec.Y
let getZ vec = vec.Z

let length vec = (pown vec.X 2) + (pown vec.Y 2) + (pown vec.Z 2) |> sqrt

let getNormalized vec = 
    let vecLength = length vec
    {
        Vector3.X = vec.X / vecLength 
        Vector3.Y = vec.Y / vecLength
        Vector3.Z = vec.Z / vecLength  }

let dotProduct vec1 vec2 =
    vec1.X * vec2.X + vec1.Y * vec2.Y + vec1.Z * vec2.Y

let formatVector vec = sprintf "(%f, %f, %f)" vec.X vec.Y vec.Z

//let inline (*) vec1 vec2 = dotProduct vec1 vec2

