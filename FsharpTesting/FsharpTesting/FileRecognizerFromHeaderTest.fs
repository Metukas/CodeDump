module FileRecognizerFromHeaderTest

// very dirty file parser

open System.IO;

type FileType = { Extension : string ; Header : byte list }

let fileTypeHeaders =
    [
        {Extension = "jpg" ; Header = [for b in [0xFF ; 0xD8 ; 0xFF ; 0xDB] -> byte b]} ;
        {Extension = "mp3" ; Header = [for b in [0x49 ; 0x44 ; 0x33] -> byte b]} ;
    ]

let readFileByte fileName index =
    use fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
    fileStream.Seek(index, SeekOrigin.Begin) |> ignore
    let byte = fileStream.ReadByte()
    (byte, index)


/////////////////////////////////////////////////////////////

// test
let (|Array|_|) l a =
    let rec loop i =
        function
        | h :: tail when h = Array.get a i -> loop (i+1) tail
        | [] -> a |> Seq.skip i |> Seq.toArray |> Some
        | _ -> None
    loop 0 l

let arrayHeadTailTest() = 
    match [| 1;2;3;4;5;6 |] with
    | Array [ 1;2] tail -> tail
    | _ -> [||]
//\test


//////////////////////////////////////////////////////////////

let isLetterOrNum ( c : char) = 
    let i = int c
    (i > 0x40 && i <= 0x5A) || (i > 0x60 && i <= 0x7A) || (i >= 0x30 && i < 0x3A)

let isNumber ( c : char) =
    let i = int c
    i >= 0x30 && i <= 0x39

let isHexDigit ( c : char) = 
    let i = int c 
    isNumber c || ( (i > 0x40 && i <= 0x46) || (i > 0x60 && i <= 0x66) )

let getIntFromHex ( c : char) =
    let minusUppercaseA = (int c) - 0x40 + 9
    if isNumber c then (int c) - 0x30
    else if minusUppercaseA <= 0x0F then
        minusUppercaseA
    else minusUppercaseA - 0x20

let fromByteListToListString byteList =
    let rec inner acc list =
        match list with 
        | [] -> acc + "]"
        | head::tail -> inner ( sprintf "%s 0x%0Xuy ;" acc head ) tail
    inner "[" byteList 


let rec parseExtension (line : string) =
    let rec inner nextChars (acc : string) foundFirstQuotes =
        match nextChars with
        | [] -> acc
        | c::tail -> 
            match c with
            | '"' -> 
                if foundFirstQuotes then acc
                else inner tail acc true
            | c when isLetterOrNum c -> inner tail (acc + string c) foundFirstQuotes
            | _ -> inner tail acc foundFirstQuotes
    
    let chars = line.ToCharArray()|> List.ofArray
    inner chars "" false

and parseHeader (line : string) =
    let rec inner nextChars (acc : int list) foundOpenBracket nextNumber foundNumber=
        match nextChars with 
        | []  -> acc
        | c::tail ->
            match c with
                | '[' -> 
                    //if foundFirstQuotes then acc @ [nextNumber]
                    //if foundNumber then acc @ [nextNumber]
                    inner tail acc true 0 false
                | ']' -> acc @ [nextNumber]
                //| c when isNumber c ->
                        //inner tail acc foundFirstQuotes ( nextNumber * 10 + (int c - 0x30) ) true
                | c when foundOpenBracket && isHexDigit c ->
                        inner tail acc foundOpenBracket ( nextNumber * 16 + (getIntFromHex c) ) true
                | _ -> 
                    if foundNumber then 
                        inner tail (acc @ [nextNumber]) foundOpenBracket 0 false
                    else
                        inner tail acc foundOpenBracket nextNumber foundNumber

    let chars = line.ToCharArray()|> List.ofArray
    inner chars [] false 0 false

and parse textArr =
    let rec inner list parsedText =
        match list with 
        | [] -> parsedText + "\t]"
        | nextLine::tail ->
            let extension = parseExtension nextLine
            let headerBytes = parseHeader nextLine
            inner tail (sprintf "%s\t\t{ Extension = \"%s\" ; Header = %s }; \n" 
                parsedText extension (fromByteListToListString headerBytes) )

    let textList = textArr |> List.ofArray
    inner textList "let fileTypeHeaders__ = \n\t[\n"


// info file sintaksė:
// "extensionas" tarpas [headerio baitai]
// "xxx" [FF D8 FF DB]
let rec makeFileTypeHeaderList infoFile =
    use fileStream = new FileStream(infoFile, FileMode.Open, FileAccess.Read, FileShare.Read)
    use streamReader = new StreamReader(fileStream)
    let rec inner arraySoFar isEoS =
        if isEoS then parse arraySoFar
        else
            let line = streamReader.ReadLine()
            inner (Array.append arraySoFar [|line|]) (streamReader.EndOfStream)

    inner [||] streamReader.EndOfStream


