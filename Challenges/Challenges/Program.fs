open System
open System.Linq
open MaximalSquare
open KaprekarsConstant
open System.Reflection
open Ascii85
open CollatzSequence
open CollatzSequenceImperative
open System.Diagnostics

let ToCharsLinq (list:'a list) =
    list.Select(fun x -> char x) |> Seq.cast<char> |> Seq.toList

let ToChars list =
    seq{for i in list -> char i} |> Seq.toList

// map
let rec map f = function
    | [] -> []
    | x::xs -> f x::map f xs //šitą galima parašyti...

//...šitaip
let prepend first toList = first::toList
let rec map_ f = function
    | [] -> []
    | head::tail -> prepend (f head) (map f tail)

let mapTailRec f list =
    let rec inner acc restOfList =
        match restOfList with
        | [] -> acc
        | head::tail -> inner (acc @ [f head]) (tail)
    inner [] list
//\map

// pvz. /////////////////////////////
let rec factorialStackOverflowy x =
    if x <= 1 then
        1
    else
        x * factorialStackOverflowy (x - 1)
 
let factorialTailRecursiveAccumulator x =
    // Keep track of both x and an accumulator value (acc)
    let rec tailRecursiveFactorial x acc =
        if x <= 1 then 
            acc
        else 
            tailRecursiveFactorial (x - 1) (acc * x)
    tailRecursiveFactorial x 1

let factorialTailRecursiveContinuation x =
    let rec contTailRecursiveFactorial x f =
        if x <= 1 then
            f()
        else
            contTailRecursiveFactorial (x - 1) (fun () -> x * f())
 
    contTailRecursiveFactorial x (fun () -> 1)

let factorialContFoldBack x =
    let rec inner x k =
        if x <= 1 then
            k x
        else 
            inner (x - 1) (fun a -> k(x * a))
    inner x (fun a -> a)
    // x: 100,      99,          98,                 97 ...
    // k: a -> a,  a -> 100 * a,  a -> 100 * 99 * a, a -> 100 * 99 * 98 * a ...
    //             a = 100 * a    a = 99 * a         a = 98 * a ...


let rec fibonacciStackOverflowy x =
    if x <= 1 then
        x
    else
        fibonacciStackOverflowy (x - 1) + fibonacciStackOverflowy (x - 2)  

//let fibonacciTailRecursive2 x =
//    let rec fibTailRec x a b =
//        if x = 0 then
//            0
//        elif x <= 1 then
//            a
//        else 
//            fibTailRec (x - 1) b (a + b) 
//    
//    fibTailRec x 1 1

let fibonacciTailRecursive x =
    let rec fibTailRec x a b =
        if x = 0 then
            a
        elif x = 1 then
            b
        else 
            fibTailRec (x - 1) b (a + b) 
    
    fibTailRec x 0 1

let printableChars = 
    seq{for i in 0x20..0x7E do yield char i}

let notTrue = not true

let testy (a:int) (b:string) = string a + b

/////////////////////////////////////////////////////
// val contains  : string -> bool
let contains =
    let people = set ["Juliet"; "Joe"; "Bob"; "Jack"]
    fun person -> people.Contains(person)

// val contains2 : string -> bool
let contains2 person =
    let people = set ["Juliet"; "Joe"; "Bob"; "Jack"]
    people.Contains(person)
    //Both functions produce identical results, contains creates its people set on declaration and reuses it,
    //whereas contains2 creates its people set everytime you call the function.
    //End result: contains is slightly faster. So knowing the distinction here can help you write faster code.
/////////////////////////////////////////////////////

// kadangi F# neleidžia for loopuose turėti kitokio tipo iteratoriaus kintamąjį nei int (Int32), tai...
let inline longFor low high f = 
  let rec loop n =
    if n < high then f n; loop (n + 1L)
  loop low
// toks workaroundas :)

// \pvz. ////////////////////////////

[<EntryPoint>]
let main argv = 
    ToDigitArray 123456789 |> List.iter (printfn "%i")
    let digitArr = ToDigitArray 123456
    let intFromDigArr = FromDigitArray digitArr
    let kaprekarsConstant = KaprekarsConstant 3458
    printfn "%c" '\n'
    printfn "%i" kaprekarsConstant
    let mMaker = new MatrixMaker()
    printfn ""
    let matrix = mMaker.MakeMatrix("111111", "111111", "111111", "111111", "111111", "111111")
    matrix |> PrintMatrix
    let kMatrix = matrix |> MakeKMatrix
    printfn ""
    kMatrix |> PrintMatrix 
    printfn ""
    kMatrix |> FindMaxArea |> printfn "Max area: %f"

    contains "Juliet"     |> ignore
    contains "Juliet"     |> ignore
    contains "Juliet"     |> ignore
    contains2 "Juliet"    |> ignore
    contains2 "Juliet"    |> ignore
    contains2 "Juliet"    |> ignore
    
    let prepended = prepend 1 [4;5;6]

    //[0x30..0x39] |>  Seq.cast<char> |> Seq.toList |> List.iter (printf "%c ")
    let toEncode = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In eleifend maximus augue tincidunt ullamcorper. In ut nisi vel metus sollicitudin mattis. Morbi scelerisque lorem massa, id mollis magna egestas at. In ultricies eros vel massa commodo mattis. Nulla facilisi. Fusce ultricies augue efficitur erat bibendum facilisis. Integer id elit eu enim ornare aliquet. Duis iaculis lacus pharetra condimentum cursus. Aenean semper purus at sagittis tincidunt. Fusce ut bibendum magna. Etiam nec urna nisl."
    //printf "%s%s" toEncode "->"
    //Ascii85.Encode (toEncode) |> ToChars |> List.iter (printf "%c")
    for i in (match toEncode |> ToByteArray |> PadByteArray with (b, _) -> b |> FromByteArrToIntArr) do
        i |> DoEncodingTail |> ToChars //|> List.iter(printf "%c")

    //https://stackoverflow.com/questions/10091185/piping-composition-and-currying

    // test //
    let _ = factorialTailRecursiveContinuation 10
    for i = 0 to 100 do
        fibonacciTailRecursive i |> printfn "%i"

    for i in 0..2..10 do
        printfn "%i" i

    for c in printableChars do
        printfn "%c" c
    //\test //
    printfn "Done"
    printfn "Not really"

    //printfn "Collatz:"
    //let collatzList = collatz 2UL 1000UL
    //let maxCollatzIndex, maxCollatzCount = List.maxBy snd collatzList // max pagal collatz sequnce countą
    //printfn "(%i, %i)" maxCollatzIndex maxCollatzCount

    printfn "Collatz dictionary:"
    let collatzDic = collatzMutArr 2UL 1000_000UL
    let max = collatzDic.Aggregate(fun currentMax next -> if currentMax.Value > next.Value then currentMax else next)
    printfn "(%i, %i)" max.Key max.Value

    //printfn "Collatz Imperative:"
    //let dictionary = new System.Collections.Generic.Dictionary<int64, int64>()
    //[2L..4000_000L] |> List.iter (fun i -> dictionary.Add(i, getCollatzEnumerator(i).Count()))
    //let max = dictionary.Aggregate(fun currentMax next -> if currentMax.Value > next.Value then currentMax else next)
    //printfn "(%i, %i)" max.Key max.Value

    //var max = numToCollatzSeqMap.Aggregate((currentMax, next) => currentMax.Value > next.Value ? currentMax : next);
    //     Console.WriteLine($"({max.Key}, {max.Value})");
    
    // log 2 lookup table test:
    let sw = new Stopwatch()
    sw.Start()
    // Math.Log
    for i in base2Map do
        Math.Log(float i, 2.) |> ignore
    sw.Stop()
    printfn "Math.Log Elapsed: %A" sw.Elapsed
    sw.Reset()
    sw.Start()
    // Lookup
    for i in base2Map do
        base2LogLookup.[i] |> ignore
    sw.Stop()
    printfn "Lookup Elapsed: %s" (sw.Elapsed.ToString())

    printfn "Probability: %f" (CircleCenterInTriangleProblem.calculateProbability 1000_000)

    // map test
    printfn "map test"
    map  (fun x -> x * 2) [1..10] |> List.iter (printfn "%i")
    map_ (fun x -> x * 2) [1..10] |> List.iter (printfn "%i")
    mapTailRec (fun x -> x * 2) [1..10] |> List.iter (printfn "%i")

    // factorials
    printfn "factorials:"
    let factorialToCalc = 100000
    //factorialStackOverflowy factorialToCalc             |> printfn "%i"
    //factorialTailRecursiveAccumulator factorialToCalc   |> printfn "%i"
    //factorialTailRecursiveContinuation factorialToCalc  |> printfn "%i"
    factorialContFoldBack factorialToCalc                 |> printfn "%i"

    let length_cps lst = 
        let rec length_cont l cont = 
            match l with 
            | [] -> cont 0 
            | _::t -> length_cont t (fun x -> cont(1 + x)) 
        length_cont lst (fun x -> x)

    length_cps [1..1000] |> printfn "%i"

    0
