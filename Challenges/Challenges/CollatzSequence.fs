module CollatzSequence

open System.Collections.Generic
open System

let base2LogLookup = 
    let dictionary = new Dictionary<uint64, uint64>(64)
    for i in [1UL..63UL] do
        dictionary.Add(uint64 (Math.Pow(2., float i)), i)
    dictionary

let base2Map = 
   let nums = [1UL..63UL]
   let map = List.map(fun i -> uint64 (Math.Pow(2., float i))) (nums)
   map

let isPowerOf2 num = 
    if num = 0UL then false
    else (num &&& (num - 1UL)) = 0UL

let calculateCollatzSequence num =
    let rec inner next count =
        match next with
        | i when i = 1UL -> count
        //| i when i |> isPowerOf2 -> count + base2LogLookup.[next]
        | i when (i &&& 1UL) = 0UL -> inner (next / 2UL) (count + 1UL)
        | _ -> inner (next * 3UL + 1UL) (count + 1UL)
    inner num 1UL
    
let collatz from upTo =
    let rec inner next acc =
        if next > upTo then
            acc
        else
            inner (next + 1UL) (List.append acc [(next, calculateCollatzSequence next)])
    inner from []

let collatzMutArr from upTo =
    let dictionary = new System.Collections.Generic.Dictionary<uint64, uint64>()
    let rec inner next =
        if next > upTo then
            ()
        else
            dictionary.Add(next, calculateCollatzSequence(next))
            inner (next + 1UL)
    inner from
    dictionary