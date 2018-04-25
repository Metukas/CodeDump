module MaximalSquare

open System.Collections.Generic
open System
open Microsoft.FSharp.Collections
open System.Linq
open System.Globalization

// https://www.youtube.com/watch?v=_Lf1looyJMU
//              
//          ->  0|_0_0_0_0_
// 0 1 1 1  ->  0| 0 1 1 1
// 0 1 1 1  ->  0| 0 1 2 2
// 1 1 1 1  ->  0| 1 1 2 3

// 1 1 1    ->  1 1 1
// 1 1 1    ->  1 2 2
// 1 1 1    ->  1 2 3

type MatrixMaker () =
    member this.MakeMatrix ([<ParamArray>] strArr : string[]) =  
        let maxCount : int = strArr.Select(fun x -> x).Max(fun i -> i.Length) // TODO: gal be linq?
        let matrix = Array2D.init strArr.Length maxCount (fun x y -> (int)strArr.[x].[y] - 0x30)
        matrix
     
    
let MakeKMatrix (matrix:int[,]) =
    //let rec kMatrix:int[,] = Array2D.init (matrix.GetLength (0)) (matrix.GetLength (1)) (fun x y -> 
    //        if(x = 0 || y = 0 || matrix.[x,y] = 0) then
    //            matrix[x,y]
    //        else
    //            let intermediateMin : int = Math.Min(kMatrix.[x, y - 1], kMatrix.[x - 1, y - 1])
    //            let minimum : int = Math.Min(kMatrix.[(x - 1), y], intermediateMin)
    //            minimum + matrix.[x, y])

    let kMatrix : int[,] = Array2D.create (matrix.GetLength 0) (matrix.GetLength 1) 0
    for x in [0..(matrix.GetLength 0) - 1] do
        for y in [0..(matrix.GetLength 1) - 1] do
            if(x = 0 || y = 0 || matrix.[x,y] = 0) then
                kMatrix.[x,y] <- matrix.[x,y]
            else
                let intermediateMin : int = Math.Min(kMatrix.[x, y - 1], kMatrix.[x - 1, y - 1])
                let minimum : int = Math.Min(kMatrix.[(x - 1), y], intermediateMin)
                kMatrix.[x,y] <- minimum + matrix.[x, y]
    kMatrix

let FindMaxArea (kMatrix:int[,]) = Math.Pow((float)(Enumerable.Cast<int>(kMatrix).Max()), 2.0)

let PrintMatrix (matrix:int[,]) = 
    for i in [0..(matrix.GetLength 0) - 1] do
        for j in [0..(matrix.GetLength 1) - 1] do
            printf "%i " matrix.[i, j]
        printfn ""
    ()

let FindMaximalSquareArea (matrix:int[,]) =
    matrix |> MakeKMatrix |> FindMaxArea