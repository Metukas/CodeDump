module Vector3'

type Vector3<'T> = Vector3 of x : 'T * y : 'T * z : 'T

let unwrapVector3 vec = 
    let (Vector3 (x, y, z) ) = vec
    (x, y, z)

let add vec1 vec2 =
    let x1, y1, z1 = unwrapVector3 vec1
    let x2, y2, z2 = unwrapVector3 vec2
    Vector3 (x1 + x2, y1 + y2, z1 + z2)

let multiply (Vector3 (x, y ,z)) num =
    Vector3 (x * num, y * num, z * num)

let dotProduct vec1 vec2 =
    let x1, y1, z1 = unwrapVector3 vec1
    let x2, y2, z2 = unwrapVector3 vec2
    x1 * x2 + y1 * y2 + z1 * z2