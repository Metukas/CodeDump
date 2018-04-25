module RecursiveTypesAndFolds

// galima defininti rekursyvius record types :)
type Person = { name: string; father: Person; mother: Person }
let rec loopy = { name = "loopy"; father = loopy; mother = loopy }

let rec male = { name = "male"; father = male; mother = female }
  and female = { name = "female"; father = male; mother = female}


type Book = {title : string ; price : decimal}

type ChocolateType = Dark | Milk | SeventyPercent
type Chocolate = {chocType : ChocolateType; price : decimal}

type WrappingPaperStyle = 
    | HappyBirthday
    | HappyHolidays
    | SolidColor

type Gift =
    | Book of Book
    | Chocolate of Chocolate
    | Wrapped of Gift * WrappingPaperStyle
    | Boxed of Gift
    | WithACard of Gift * message:string

let rec cataGift fBook fChoc fWrapped fBoxed fCard gift =
    let recurse = cataGift fBook fChoc fWrapped fBoxed fCard
    match gift with
    | Book book -> fBook book
    | Chocolate choc -> fChoc choc
    | Wrapped (gift, wrap) -> fWrapped (recurse gift, wrap)
    | Boxed gift -> fBoxed (recurse gift)
    | WithACard (gift, message) -> fCard (recurse gift, message)

// accumulator catamorphism
let rec foldGift fBook fChoc fWrapped fBoxed fCard acc gift : 'r =
    let recurse = foldGift fBook fChoc fWrapped fBoxed fCard
    match gift with
    | Book book -> fBook acc book
    | Chocolate choc -> fChoc acc choc
    | Wrapped (gift, wrap) -> recurse (fWrapped acc wrap) gift
    | Boxed gift -> recurse (fBoxed acc) gift
    | WithACard (gift, message) -> recurse (fCard acc message) gift

let rec describe gift =
    match gift with 
    | Book book -> sprintf "'%s'" book.title 
    | Chocolate choc -> sprintf "%A" choc.chocType
    | Wrapped (gift, wrap) -> sprintf "%s wrapped in %A paper" (describe gift) (wrap)
    | Boxed gift -> sprintf "%s in a box" (describe gift) 
    | WithACard (gift, message) -> sprintf "%s with card that says '%s'" (describe gift) (message)

let cataGiftDescribe gift =
    let fBook book = sprintf "'%s'" book.title 
    let fChoc choc = sprintf "%A" choc.chocType
    let fWrap (innerText, wrap) = sprintf "%s wrapped in %A paper" (innerText) (wrap)
    let fBox innerText = sprintf "%s in a box" (innerText) 
    let fCard (innerText, message) = sprintf "%s with card that says '%s'" (innerText) (message)
    cataGift fBook fChoc fWrap fBox fCard gift

let describeFoldWrong gift =
    let fBook acc book = sprintf "%s '%s'" book.title acc
    let fChoc acc choc = sprintf "%A %s" choc.chocType acc
    let fWrap acc wrap = sprintf "%s wrapped in %A paper" acc wrap
    let fBox acc = sprintf "%s in a box" acc
    let fCard acc message = sprintf "%s with card that says '%s'" acc message
    let initialAcc = ""
    foldGift fBook fChoc fWrap fBox fCard initialAcc gift

let describeFoldBack gift =
    let fBook descriptionGenerator book = 
        descriptionGenerator ("'" + book.title + "'") //(sprintf "'%s'" book.title)
    let fChoc descriptionGenerator choc = 
        descriptionGenerator (sprintf "%A chocolate" choc.chocType)
    let fWrap descriptionGenerator wrap =
        fun innerText -> 
            descriptionGenerator (sprintf "%s wrapped in %A paper" innerText wrap)
    let fBox descriptionGenerator =
        fun innerText ->
            descriptionGenerator (sprintf "%s in a box" innerText)
    let fCard descriptionGenerator message =
        fun innerText ->
            descriptionGenerator (sprintf "%s with card that says '%s'" innerText message)
    let initialAcc = fun innerText -> innerText
    foldGift fBook fChoc fWrap fBox fCard initialAcc gift


let rec totalPrice gift =
    match gift with
    | Book book -> book.price
    | Chocolate choc -> choc.price
    | Wrapped (gift, _) -> totalPrice gift + 0.5m
    | Boxed gift -> totalPrice gift + 1m
    | WithACard (gift, _) -> totalPrice gift + 0.25m

let rec totalPriceFold acc gift =
    match gift with
    | Book book -> acc + book.price
    | Chocolate choc -> acc + choc.price
    | Wrapped (gift, _) -> totalPriceFold (acc + 0.5m) gift
    | Boxed gift -> totalPriceFold (acc + 1m) gift
    | WithACard (gift, _) -> totalPriceFold (acc + 0.25m) gift


let rec fold acc folder list =
    match list with 
    | [] -> acc
    | head::tail -> fold (folder acc head) folder tail


let rec foldBackDivision (acc : float) (list : float list)=
    match list with 
    | [] -> acc
    | head::tail -> foldBackDivision (head / acc) tail


// (!) Kad continuation passing style (fold backinės) funckijos būtų tikrai tail recursive, reikia,
// kad generatoriaus funkcija kviestų pati save ir nedarytų jokių papildomų veiksmų kaip pvz.: f(x) * a
//let foldBack acc folder list =
//    //let initialGen = fun other -> folder other acc
//    let initialGen = fun other -> other
//    let rec inner generator list =
//        match list with 
//        | [] -> acc
//        | head::tail -> 
//            match tail with
//            | [] -> generator head
//            | _::_ -> inner (fun other -> generator (folder head other)) tail
//            //| _::_ -> inner (fun other -> folder other (generator head) ) tail
//    inner initialGen list

let foldBack acc folder list =
    let initialGen = fun a -> a
    let rec inner generator list =
        match list with
        | [] -> generator acc
        | head::tail -> inner (fun other -> generator (folder head other )) tail
    inner initialGen list

    //folder = (+)
    //          ... fun other -> (folder 1 (folder 2 other))  
    // fun other -> (folder 1 (folder 2 (folder 3 other ))) 
    // [] -> generator acc
    // fun other -> (folder 1 (folder 2 (folder 3 other ))) <| 0
    // folder 1 (folder 2 (folder 3 0 ))
    // 0 + 3 + 2 + 1 = 6

    // folder = (/)
    // folder 1 (folder 2 (folder 3 1 ))
    // 3 / 1 --> 2 / 3 --> 1 / (2 / 3) = 1,5 
    

// foldo ir foldBack pvz. kad žinot, kaip skirtingai jie duoda rezultatą:
List.fold     (fun acc x -> x :: acc) [] [1; 2; 3; 4; 5] |> ignore
// val it : int list = [5; 4; 3; 2; 1]
List.foldBack (fun x acc -> x :: acc) [1; 2; 3; 4; 5] [] |> ignore
// val it : int list = [1; 2; 3; 4; 5]
List.foldBack (fun x acc -> x :: (acc.[0] :: acc)) [1;2;3] [5] |> ignore
// val it : int list = [1; 2; 2; 3; 3; 5; 5]
List.fold (fun acc x -> x :: (acc.[0] :: acc)) [5] [1;2;3]     |> ignore
// val it : int list = [3; 2; 2; 1; 1; 5; 5]


//not really
let foldBackFromFold acc folder list =
    let initialGen = fun generator -> generator acc
    fold initialGen folder list

