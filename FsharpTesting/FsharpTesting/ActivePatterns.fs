module ActivePatterns

open MyResult
open System.Runtime.Remoting.Messaging

let (|Even|Odd|) num = if num % 2 = 0 then Even else Odd

// coding horror lol
let (|Zero|One|Two|Three|Four|Five|GreaterThanFive|) num =
    // what is the last digit
    let result = num % 10
    if result = 0 then Zero
    else if result = 1 then One
    else if result = 2 then Two   
    else if result = 3 then Three
    else if result = 4 then Four
    else if result = 5 then Five
    else GreaterThanFive // why active patterns cannot return more than 7 cases? :(

// partial active pattern yra praktiškai alias option tipui (Some ir None)
// šiuo atveju One yra Some, o _ - None
let (|One|_|) num = if num = 1 then Some(1) else None // silly active pattern :)

let testSilly num = 
    match num with
    | One n -> printfn "Got one!! %i" n
    | _ -> printfn "Got no one!! :("