module Ascii85

open System

let ToByteArray (arg:string) =
    System.Text.Encoding.ASCII.GetBytes(arg)


let FromByteArrToIntArr (byteArr:byte[]) : uint32[] = 
    let wtf = ((int)(byteArr.[0]) <<< 8) &&& 0x0000FF00 //test
    Array.init (byteArr.Length / 4) 
        (fun x -> (((uint32)(byteArr.[x*4]) <<< 24) &&& 0xFF000000u) + 
                    ((((uint32)byteArr.[x*4 + 1])   <<< 16) &&& 0x00FF0000u) + 
                    ((((uint32)byteArr.[x*4 + 2])   <<<  8) &&& 0x0000FF00u) +
                    ((((uint32)byteArr.[x*4 + 3])         ) &&& 0x000000FFu))


let PadByteArray (byteArr:byte[]) = 
    let padCount = (4 - (byteArr.Length % 4))
    if byteArr.Length % 4 <> 0 then
        List.append (List.ofArray byteArr) [for i in [0..(4 - (byteArr.Length % 4)) - 1] do yield (byte)0] |> List.toArray, padCount
    else
        byteArr, padCount


let DoEncodingTail bit32Number =
    let rec DoEncodingHelper bit32Number acc =
        if bit32Number < 85u then
            ([bit32Number + 33u], acc) ||> List.append
        else
            DoEncodingHelper (bit32Number / 85u) (([(bit32Number % 85u) + 33u], acc) ||> List.append)

    DoEncodingHelper bit32Number []

let Encode (arg:string)= 
    let bytes, padCount = match arg |> ToByteArray |> PadByteArray with (b, c) -> b, c
    let ints = bytes |> FromByteArrToIntArr |> Array.toList

    let rec DoEncoding bit32Number =
        if bit32Number < 85u then
            [bit32Number + 33u]
        else
            (DoEncoding (bit32Number / 85u), [(bit32Number % 85u) + 33u] ) ||> List.append

    let rec ConcatEncoding (a: uint32 list) i : uint32 list = 
        if i = a.Length - 1 then
            DoEncoding (a.[i])
        else
            (DoEncoding a.[i] , ConcatEncoding a (i + 1)) ||> List.append

    let result = ConcatEncoding ints 0
    //result    // Pilnas encodinimas su encodintais paddinimo bitais
    result.GetSlice(None, Some(result.Length - padCount - 1)) // be encodintų paddinimo bitų
