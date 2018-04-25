module Vector2

type Vector2<'T> = {X : 'T ; Y : 'T}


let mapV f v =
    let x = v.X
    let y = v.Y
    let x' = f x
    let y' = f y
    {X = x' ; Y = y'}

let applyV f v =
    let f1 = f.X
    let f2 = f.Y
    let x = v.X
    let y = v.Y
    let x' = f1 x
    let y' = f2 y
    {X = x' ; Y = y'}

let add a b = a + b
let testVec() =
    let (<!>) = mapV
    let (<*>) = applyV
    let addVecs a b = add <!> a <*> b
    let someVector = {X = 1; Y = 4}
    let result = addVecs someVector someVector
    result

//let bindV (f : 'a -> Vector2<'b>) (v : Vector2<'a>) = 
//    let x = v.X
//    let y = v.Y
//    let x' = f x
//    let y' = f y 
//    

let returnV x = {X = x; Y = x}
