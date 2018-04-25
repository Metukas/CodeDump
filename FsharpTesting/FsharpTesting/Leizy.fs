module Leizy

// M<'b>
type Leizy<'a, 'b> = Leizy of ('a -> 'b) * 'a

let getLeizy func arg =
    Leizy(func, arg)

let forceLz lazyFunc =
    let (Leizy ( f, a)) = lazyFunc
    f a

let returnLz x = Leizy(id, x)

// bind : ('a -> M<'b>) -> M'<a> -> M<'b>
let bindLz f x =
    match x with
    | Leizy (xFun, xArg) -> Leizy ( (fun a -> forceLz (f (xFun a))) , xArg )

let (>>=) x f = bindLz f x

// apply ir map iš bindo ir returno 
let mapLz' f =  bindLz (f >> returnLz)
let applyLz' f x =
    x >>= fun x' ->
    f >>= fun f' ->
        returnLz(f' x')
//////////////////////////////////////

// map : ('a -> 'b) -> M<'a> -> M<'b>
let mapLz f x =
    match x with
    | Leizy (func, arg) -> Leizy ( ( fun a -> f (func a) ) , arg)
let (<!>) = mapLz

// apply : M<'a -> b'> -> M<a'> -> M<b'>
let apply_NotReallyLazy f x =
    match f with
    | Leizy (func, arg) ->
        match x with
        | Leizy (f', a') -> 
            let unwrappedFunc = func arg // šitas let padaro apply nelabai lazy, nes let by default 
                                         // apskaičiuoja rezultatą iš karto
            Leizy ( (fun a ->  unwrappedFunc (f' a)), a')

// šitas apply nuo viršutinio skiriasi tik tuo, kad vietoj let, išraiška yra iš karto įdėta į fun a ->...
// ir to užtenka, kad apply būtų lazy
let applyLz f x =
    match f with
    | Leizy (fFunc, fArg) ->
        match x with
        | Leizy (xFunc, xArg) -> Leizy( (fun a -> fFunc fArg (xFunc a) ), xArg)
let (<*>) = applyLz

let testLz () =
    let lazyCollatz1 = getLeizy CollatzSequence.calculateCollatzSequence 100UL
    let lazyCollatz2 = getLeizy CollatzSequence.calculateCollatzSequence 10UL
    let add a b = a + b
    let lazyAdd a b = add <!> a <*> b 
    lazyAdd lazyCollatz1 lazyCollatz2
let testLz2() =
    let lzCollatz = getLeizy CollatzSequence.calculateCollatzSequence 100UL
    let times3 = fun i -> i * 3UL
    let elFunc = mapLz times3
    elFunc lzCollatz

// traverse, sequence:
let rec traverseLeizyA f list =
        let cons head tail = head::tail
        match list with
        | [] -> returnLz []
        | head::tail -> 
            cons <!> (f head) <*> (traverseLeizyA f tail)
let sequenceLeizyA list = traverseLeizyA id list
    




type ('a, 'b) lazyT = Lz of 'a * ('a -> 'b)

let force (Lz (a, e)) = e a
let pack x = Lz(x, (fun i -> i))

type MyLazyBuilder =
  | Mylazy
  member this.Bind(x, f) =
      match x with
       | Lz(xa, xe) -> 
             Lz(xa, fun x -> force (f (xe x)))
  member this.Return(x) = pack x

// Lazy<'a> funkcijos su f# librario lazy
module LazyF =
    let force lz = match lz with Lazy l -> l
    let ret x = lazy x
    let bind (f : 'a -> Lazy<'b>) (x : Lazy<'a>) =
         lazy ( (f (x.Force())).Force() )
    let bind'' f x = //nicer bind
        lazy ( x |> force |> f |> force)
    let (>>=) x f = bind f x

    let map f = bind (f>>ret)
    
    let apply f x =
        f >>= fun f' ->
        x >>= fun x' ->
            ret(f' x')
    
    let rec traverseListA f list =
        let (<!>) = map
        let (<*>) = apply
        let cons head tail = head::tail
        match list with
        | [] -> ret []
        | head::tail -> 
            cons <!> (f head) <*> (traverseListA f tail)

    let sequenceListA list = traverseLeizyA id list
    
    
    

    //apply iš lift2, map iš apply ir ret
    //let lift2 f (a : Lazy<'a>) (b : Lazy<'b>) = 
    //    lazy ( f (a.Force()) (b.Force()) )
    //
    //let apply f x = 
    //    lift2 (fun f x -> f x) f x
    //
    //let map f (x : Lazy<'a>) = lazy (f (x.Force()))
    
        //match x with
        //| Lazy (xFun, xArg) -> Lazy ( (fun () -> (f (xFun ())).Force() ) , xArg )