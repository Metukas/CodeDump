module Monoids

type OrderLine = {
    ProductCode : string
    Qty : int
    Total : float 
    }

let calculateOrderTotal lines = 
    let accumulateTotal total lines =
        total + lines.Total
    lines |> List.fold accumulateTotal 0.0

//List.fold
//List.fold2
//List.foldBack
//List.foldBack2

let sum nums =
    nums |> List.fold (+) 0

let first = function
    | [] -> None
    | head::_tail -> Some head

let firstAndSecond = function
    | [] -> None, None
    | head::tail -> Some head, first tail