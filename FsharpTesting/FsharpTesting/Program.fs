open System.Collections.Generic
open Monoids
open System
open RecursiveTypesAndFolds
open MutualTypes
open FileSystemTest
open FileRecognizerFromHeaderTest
open ParserCombinators
open MyResult
open MyCrappyList
open Leizy
open ActivePatterns


[<EntryPoint>]
let main argv = 
    let actions = List<unit->int>()
    let mutable num = 0
    while num < 10 do
        actions.Add( fun () -> num * 2)
        num <- num + 1

    for action in actions do
        printfn "%i" (action())

    let actions = List<unit->int>()
    for i = 0 to 10 do
        actions.Add(fun () -> i * 2)

    for action in actions do
        printfn "%i" (action())

    let arr = Array.singleton 0

    let orderLines = [
        {ProductCode="AAA"; Qty=2; Total=19.98}
        {ProductCode="BBB"; Qty=1; Total=1.99}
        {ProductCode="CCC"; Qty=3; Total=3.99}
        ]
            
    orderLines 
    |> calculateOrderTotal 
    |> printfn "Total is %g"

    let sum = sum [1..1000]
    printfn "%i" sum

    // recursive types
    let book = Book {title = "Sample" ; price = 0m}
    let gift = Boxed(Wrapped(WithACard(Book {title = "Some Title" ; price = 10m},"Happy whatever")
        , HappyBirthday))

    let watafak = sprintf "čia yra: %s" (describe gift)
    Console.WriteLine(watafak)
    gift |> describe  |> printfn "%s"
    gift |> cataGiftDescribe |> printfn "%s"
    gift |> describeFoldWrong |>  printfn "%s"
    gift |> describeFoldBack |> printfn "%s"
    gift |> totalPrice |> printfn "%f"
    gift |> totalPriceFold 0m |> printfn "%f"

    let exceptLast (list : 'a list)=
        //list |> List.rev |> List.tail |> List.rev
        list.[..list.Length - 2]
    let switchArgs f c a b = f a b c
    let list = [1.0..3.0]
    let folder = (+)
    let initial = 0.0
    printfn "Fold test:"
    
    //foldai:
    //list |> List.fold folder initial |> printfn "%f"
    list |> fold initial folder |> printfn "%f"
    //foldbackai:
    //initial |> List.foldBack folder list |> printfn "%f"
    //list |> (switchArgs List.foldBack) initial folder |> printfn "%f"
    list |> foldBack initial folder |> printfn "%f"

    //\recursive types
    
    // OOP test
    let vectors3 = [for i in [1..100] -> new Vector3OO()]
    Vector3OO.Count |> printfn "there were %i vectors created"

    // Vector3 functional test:
    let vectoriukasVienas = {
        Vector3.X = 123.
        Vector3.Y = 654.
        Vector3.Z = 789.
    }

    let vectoriukasDu = {
        Vector3.X = 10.
        Vector3.Y = 10.
        Vector3.Z = 98.
    }

    let dot = Vector3.dotProduct vectoriukasVienas vectoriukasDu
    printfn "%f" dot
    let normalized = Vector3.getNormalized({X = 1. ; Y = 1.; Z = 0.})
    printfn "%s" (Vector3.formatVector normalized)

    //File system:
    let directory = """C:\Users\Matas\Desktop"""
    //let fs = getFileSystem directory

    //File parsing test:
   // makeFileTypeHeaderList (directory + """\TestParse.txt""") |> printfn "%s"

    //array head tail:
    printfn "array head tail:" 
    arrayHeadTailTest () |> Array.iter (printfn "%i")
    let someArr = [|1..10|]
    let toList list = 
        let rec loop acc array =
            match array with 
            | [||] -> acc
            | Array [Array.head array] tail -> loop (acc @ [array.[0]]) tail
            | [|_|] -> failwith "Wat?"
        loop [] list
    toList someArr |> List.iter (printfn "%i")

    // parser combinator test:
    let stringas = "ABCDEFG"
    let resultAnd = runParser parseAThenB stringas
    let resultOr = runParser parseAOrB stringas
    let parseAThenBOrA = parseAThenB .>>. parseAOrB
    let resultThat = runParser parseAThenBOrA stringas
    match resultAnd with 
    | Ok r -> printfn "%A" r 
    | Error (e, input) -> printfn "Error: %s. Rest of input: %s" e input

    match resultOr with
    | Ok r -> printfn "%A" r 
    | Error (e, input) -> printfn "Error: %s. Rest of input: %s" e input

    match resultThat with
    | Ok r -> printfn "%A" r 
    | Error (e, input) -> printfn "Error: %s. Rest of input: %s" e input

    printResult (runParser parseThreeDigits "123456")
    printResult (runParser parseThreeDigitsAsString "123456")
    printResult (runParser parseThreeDigitsAsInt "123456")

    // M funcs:
    let someNormalWorldThing = 10.0
    let theSameElevatedWorldThing = Success someNormalWorldThing
    // map'as
    let transformFunction = fun x -> x ** 2.0
    let someElevatedWorldThing = mapR transformFunction theSameElevatedWorldThing
    // bind'as
    let elevationTransformFunction = fun x -> Success (x ** 2.0)
    let someElevatedWorldThing = bindR theSameElevatedWorldThing elevationTransformFunction
    // apply
    let elevatedTransformFunc = Success (fun x -> x ** 2.0)
    let someElevatedWorldThing = applyR elevatedTransformFunc theSameElevatedWorldThing


    let wat = bindList (fun x -> [x + 1]) [1..10] 
    let crap = MyCrappyList(1,2,3,4,5)
    let wat = Bind crap ( fun x -> MyCrappyList(x + 1))
    let someFunc = fun x -> x + 1
    let crappyListFuncs = MyCrappyList(someFunc, someFunc, someFunc, someFunc, someFunc)
    let wat = Apply crap crappyListFuncs

    match addResultsTest() with
    | Success s -> printfn "Success! %A" s
    | Failure f -> printfn "Failure! %A" f

    let elAddResult = add_retap (Success 100) (Success 12)
    match elAddResult with
    | Success s -> printfn "%i" s
    | Failure f -> printfn "%s" f

    for i in (addListsTogether [1..10] [for i=1 to 10 do yield 2]) do
        printfn "!: %i" i

    let someSet = set [1..10]
    someSet.Count |> printfn "%i"

    //traverse,sequence
    let someList = [for i in 1..100 -> Success i]
    let someList2 = [1..100]
    let traversed = MyResult.ListyBoi.traverseResultA (fun i -> Success i) someList2
    let alsoTraversed = List.map (fun i -> Success i) someList2 
                            |> MyResult.ListyBoi.sequenceResultA
    let listResult = MyResult.ListyBoi.sequenceResultA someList
    match traversed with
    | Success list -> List.iter (printfn "%i") list
    | Failure f -> printfn "%s" f
    AgentTest.printerAgent.Post "test1"
    AgentTest.printerAgent.Post "test2"
    AgentTest.printerAgent.Post "test3"

    // Vector2 map apply
    Vector2.testVec() |> printfn "%A"

    // state monad test
    printfn "State monad test"
    printfn "Before (0, %A)" StateMonad.someNums
    printfn "After %A" (StateMonad.test())

    printfn "State monad for test"
    printfn "After %A" (StateMonad.testFor())

    printfn "State monad tee"
    printfn "After %A" (StateMonad.testWithTee())

    printfn "State monad other way test"
    printfn "After %A" (StateMonad.testOtherWay())

    // lazy test 
    printfn "Lazy test"
    let someHeavyFunc list =
        List.fold (+) 0 list

    let leizy = getLeizy CollatzSequence.calculateCollatzSequence 100UL
    let someFunc a = a * 2UL
    let elevatedFunc = mapLz someFunc
    let transformedLazy = elevatedFunc leizy

    let _ = transformedLazy //|> Leizy.forceLz |> printfn "%i" // map: OK lazy as fuk
    let _ = testLz () //|> Leizy.forceLz |> printfn "%i" 

    // test traverse su leizy:
    let lazyWorldCrossingFunc num = getLeizy CollatzSequence.calculateCollatzSequence num
    let lazyList = Leizy.traverseLeizyA lazyWorldCrossingFunc [1UL..10UL]
    let lazyTransformedList = Leizy.mapLz (fun list -> List.map(fun i -> i * 2UL) list) lazyList
    //let collatzs = Leizy.forceLz lazyTransformedList |> List.iter (printfn "double: %i")

    // sequence test
    let someListOfLazies = List.map (lazyWorldCrossingFunc) [1UL..10UL]
    let sameAsLazyList = sequenceLeizyA someListOfLazies
    //let collatzs = Leizy.forceLz sameAsLazyList |> List.iter (printfn "%i")

    // lazy su librariniu F# lazy 
    let (<!>) = LazyF.map
    let (<*>) = LazyF.apply
    let lazyAdd a b = (+) <!> a <*> b
    let lazyCollatz1 = lazy CollatzSequence.calculateCollatzSequence 100UL
    let lazyCollatz2 = lazy CollatzSequence.calculateCollatzSequence 10UL
    let lazyResult = lazyAdd lazyCollatz1 lazyCollatz2
    let lazyResul2 = lazyAdd lazyCollatz1 lazyCollatz2
    let lazyResul3 = lazyAdd lazyCollatz1 lazyCollatz2

    lazyResult.Force() |> printfn "%i"
    lazyResul2.Force() |> printfn "%i"
    lazyResul3.Force() |> printfn "%i"

    // Active patterns:
    testSilly 1

    let list = [1..100]
    //for (i, item) in Seq.indexed list do
    //    printfn "%i: %i" i item
    (+) 1 2 |> printfn "%i"

    //IO monad:
    printfn "IO Monad"
    Haskell.Prelude.Test.test() |> ignore

    let inline addy (a : ^a) (b : ^b) = a + b
    addy 1 2
    addy 1.0 2.0

    printfn "Everything done"
    
    0 // return an integer exit code
    