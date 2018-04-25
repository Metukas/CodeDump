module KaprekarsConstant

open System

// Have the function KaprekarsConstant(num) take the num
// parameter being passed which will be a 4-digit number with at least two distinct digits.
// Your program should perform the following routine on the number: 
// Arrange the digits in descending order and in ascending order (adding zeroes to fit it to a 4-digit number),
// and subtract the smaller number from the bigger number. Then repeat the previous step. 
// Performing this routine will always cause you to reach a fixed number: 6174. 
// Then performing the routine on 6174 will always give you 6174 (7641 - 1467 = 6174). 
// Your program should return the number of times this routine must be performed until 6174 is reached. 
// For example: if num is 3524 your program should return 3 because of the following steps: 
// (1) 5432 - 2345 = 3087, (2) 8730 - 0378 = 8352, (3) 8532 - 2358 = 6174. 


let rec ToDigitArray num =
    if num / 10 = 0 then 
        [num]
    else
        (ToDigitArray (num / 10), [num % 10]) ||> List.append
        //List.append (ToDigitArray (num / 10)), [num % 10]
        //List.append (ToDigitArray (num / 10)) [num % 10]


let rec FromDigitArray (num : int list) = 
    if num.Length <= 1 then
        num.[0]
    else
        let lengthMinOne = num.Length - 1
        let floatLenMinOne = float lengthMinOne
        let truncatedList = num.[1..]
        //let truncatedList = num |> Seq.skip 1 |> Seq.take (num.Length - 1) |> Seq.toList
        num.[0] * int (Math.Pow(10.0, floatLenMinOne)) + FromDigitArray truncatedList

let bind (x, f) = f x
let bindy x f = f x
let (>>=) = bindy

let rec FromDigitArrayBinded (num : int list) =
    if num.Length <= 1 then
        num.[0]
    else
        bind(num.Length - 1, fun lengthMinOne -> 
        bind(float lengthMinOne, fun floatLenMinOne ->
        bind(num.[1..], fun truncatedList ->
        num.[0] * int (Math.Pow(10.0, floatLenMinOne)) + FromDigitArray truncatedList)))


// Šita funkcija taip pat yra tail recursive funckija. Aš parašiau tokią funkciją net to nežinodamas :D
let public KaprekarsConstant num =
    let rec CountToKaprekarsConstant num count =
        let descendingArray = num |> ToDigitArray |> List.sortByDescending (fun x -> x)
        let descending = descendingArray |> FromDigitArray
        let ascending = descendingArray |> List.rev |> FromDigitArray

        let bigger = Math.Max(descending, ascending)
        let smaller = Math.Min(descending, ascending)
        let numToTest = bigger - smaller
        
        if numToTest = 6174 then
            (count + 1)
        else                                 // tai vadinasi variable shadowing :O
            CountToKaprekarsConstant numToTest (count + 1)

    let numDigitArr = num |> ToDigitArray
    if num < 1000 || num > 9999 then
        -1
    else if numDigitArr.[0] = numDigitArr.[1] && 
        numDigitArr.[1] = numDigitArr.[2] && 
        numDigitArr.[2] = numDigitArr.[3] then
        -1111
    else
        CountToKaprekarsConstant num 0