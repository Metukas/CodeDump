module StateMonad

// kiti panašūs monadai :
////////
type Reader<'TCompute, 'TState> = Reader of ('TState -> 'TCompute)
type Writer<'TCompute, 'TState> = Writer of ('TState * 'TCompute)
type Update<'TCompute, 'TUpdate ,'TState> = Update of ('TState -> 'TUpdate * 'TCompute)
///////

type State<'TCompute, 'TState> = State of ('TState -> 'TCompute * 'TState)

let runS s = 
    let (State func) = s
    func

let bindS f x =
    State (fun s0 -> 
        let a, s = runS x s0
        let s2 = f a
        runS s2 s)

let (>>=) x f = bindS f x

let returnS a = 
    State(fun s -> a, s)

let mapS f = bindS (f >> returnS)
let (<!>) = mapS

let applyS f x = 
    f >>= fun f' ->
    x >>= fun x' ->
        returnS (f' x')

let (<*>) = applyS

let tap (*tee*) sideEffect =
    fun x ->
        do sideEffect x
        x

let tapS sideEffect sFunc = 
    let inner state = 
        let a, s = runS sFunc state
        do sideEffect (a, s)
        a, s
    State inner

let (|>!) s sideEff = tapS sideEff s

let combineS (s1 : State<'c1, 'state>) (s2 : State<'c2, 'state>)  =
    State (
        fun initialState ->
        let _, itermediateState = runS s1 initialState
        runS s2 itermediateState
    )

let zeroS () = State(fun s -> (), s)

let rec whileS f x = 
    if f () then combineS x ( whileS f x)
    else zeroS()

//seq<'T> * ('T -> M<'U>) -> M<'U>
let forS seq ( f : 'a -> State<'b,'c>) : State<'b,'c> = 
    let seqElevatedFunc = Seq.map f
    let stateSeq = seqElevatedFunc seq
    Seq.reduceBack (fun s1 s2 -> combineS s1 s2) stateSeq


// test
let add a b = a + b
let addS a b = add <!> a <*> b

let addOne s = 
    State (fun ( list : int list )-> 
        match list with
        | [] -> s, []
        | head::tail -> (head + s), tail
    )

let addOneS s =
    State ( fun accumulator ->
        match s with
        | [] -> [], accumulator
        | head::tail -> tail, (head + accumulator)
    )

let someNums = [1..5]


let test () =
    let printState state = printfn "intermediate state: %A" state
    let wat = 0 |> addOne |> tapS printState >>= addOne >>= addOne
    let wat2 = 0 |> addOne |>! printState >>= addOne >>= addOne |>! printState
               >>= addOne >>= addOne >>= addOne
    runS wat2 someNums


let testWithTee () =
    let tee a b = (tap >> mapS) b a
    let printState state = printfn "intermediate state: %A" state
    let (|>!) = tee
    let wat = 0 |> addOne |>! printState >>= addOne >>= addOne |>! printState
               >>= addOne >>= addOne >>= addOne
    runS wat someNums

let testOtherWay() = 
    let printState state = printfn "intermediate state: %A" state
    let wat = [1..10] |> addOneS |>! printState >>= addOneS >>= addOneS |>! printState
               >>= addOneS >>= addOneS >>= addOneS
    runS wat 0

let addToState i =
    State( fun s -> i, s + i)

let testFor() =
    runS (forS [1..10] addToState) 0
    