module ParserCombinators

type Parser<'T, 'TInput, 'TErr> = Parser of ('TInput -> Result<'T * 'TInput, 'TErr>) 

let parseChar charToExpect =
    let parser input =
        if input = "" then Error ("No more input", "")
        else 
            let firstChar = input.[0]
            if firstChar = charToExpect then 
                let remaining = input.[1..]
                Ok (firstChar, remaining)
            else Error (sprintf "Expected %c, got %c" charToExpect firstChar, input)
    Parser parser

let unwrapParser parser =
    match parser with
    | Parser p -> p

let runParser parser input =
    let (Parser p) = parser
    p input


let glue p1 p2 f2 input = runParser p1 input |> f2 p2 

let returnP x = 
    let innerFn input =
        // ignore the input and return x
        Ok (x,input )
    // return the inner function
    Parser innerFn 

let bindP p f =
    let innerFn input =
        match runParser p input with
        | Error e -> Error e
        | Ok (value, remaining) -> runParser (f value) remaining
    Parser innerFn

let bindPE p f =
    let innerFn input = 
        let result = runParser p input
        match result with
        | Error e -> runParser (f input) input
        | Ok _ -> result
    Parser innerFn

let (>>=) = bindP

type ParserBuilder() =
    member this.Bind(x, f) =
        let innerFn input =
            let result1 = runParser x input
            match result1 with
            | Ok (input, remaining) -> runParser (f input) remaining
            | Error e -> Error e
        Parser innerFn
    member this.Return x =
        let innerFn input =
            Ok(x, input)
        Parser innerFn

let parser = new ParserBuilder()

// this:
let andThen_ p1 p2 =         
    p1 >>= (fun p1Result -> 
    p2 >>= (fun p2Result -> 
        returnP (p1Result,p2Result) ))

// vs this: (tas pats)
let andThen_B p1 p2 =
    parser{
        let! result1 = p1 //in
        let! result2 = p2 //in
        return (result1, result2)
    }


let andParse nextParser input =
    match input with
    | Ok (first, rest1) -> 
        match runParser nextParser rest1 with
        | Ok (second, rest2) -> Ok ((first,second), rest2)
        | Error e -> Error e
    | Error e -> Error e

let andThen p1 p2 = Parser (glue p1 p2 andParse)

let orParse nextParser result =
    match result with 
    | Ok r -> Ok r
    | Error (_, input) -> runParser nextParser input

let orElse p1 p2 = Parser (glue p1 p2 orParse)

let (.>>.) = andThen
let (<|>) = orElse
let choice listOfParsers = List.reduce orElse listOfParsers
let anyOfChar listOfChars = listOfChars |> List.map parseChar |> choice
//more generic 
let anyOf inputs parser = inputs |> List.map parser |> choice

let toString (list : char list) =
    let rec loop acc next =
        match next with
        | [] -> acc
        | head::tail ->  loop (acc + string head) tail
    loop "" list

let mapP f parser = 
    let innerFn input = 
        let result = runParser parser input
        match result with
        | Ok (value, remaining) -> Ok (f value, remaining)
        | Error e -> Error e
    Parser innerFn

let parseDigit = anyOf ['0'..'9'] parseChar

let parseThreeDigits =
    parseDigit .>>. parseDigit .>>. parseDigit

let parseThreeDigitsAsString =
    let tupleTransform ((c1 , c2), c3) = toString [c1 ; c2 ; c3]
    mapP tupleTransform parseThreeDigits

let parseThreeDigitsAsInt =
    let transform input = int input
    mapP transform parseThreeDigitsAsString

let printResult result = 
    match result with
    | Ok value -> printfn "Success: %A" value
    | Error e -> printfn "Error: %A" e

let parseA = parseChar 'A'
let parseB = parseChar 'B'

let parseAThenB = parseA .>>. parseB
let parseAOrB = parseA <|> parseB

let mapP2 f parser = 
    let innerFn input = 
        let result = runParser parser input
        match result with
        | Ok (value, remaining) -> Ok (f value, remaining)
        | Error e -> Error e
    Parser innerFn

let applyP2 x f = 
    // create a Parser containing a pair (f,x)
    (f .>>. x) 
    // map the pair by applying f to x
    |> mapP (fun (f,x) -> f x)



