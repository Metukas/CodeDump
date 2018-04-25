// Mon... *pliaukšt... elevation functions test
module MyResult

type Result<'TSuccess, 'TFailure> =
    | Success of 'TSuccess
    | Failure of 'TFailure

// bind : M<'a> -> ('a -> M<'b>) -> M<'b>
let bindR x f = 
    match x with 
    | Success s -> f s
    | Failure e -> Failure e

let (>>=) = bindR


let zero () = Failure

// return : 'a -> M<'a>
let returnR x =
    Success x

// returnFrom : M<'a> -> M<'a>
let returnFromR x = 
    match x with    
    | Success s -> Success s
    | Failure f -> Failure f

// map : ('a -> 'b) -> M<'a> -> M<'b>
let mapR f x =
    match x with 
    | Success s -> Success (f s)
    | Failure e -> Failure e

//  apply :  M<'a -> 'b> -> M<'a> -> M<'b>
// apply galima sakyti yra map, tiktai transformacijos funckija ('a -> b') irgi yra "pakelta" 
// (elevated (wrapped in enclosing type)) (M<'a -> 'b>)
let applyR f x =
    match f with 
    | Success f' ->
        match x with
        | Success x' -> Success (f' x')
        | Failure e -> Failure e
    | Failure e -> Failure e

let (<!>) = mapR
let (<*>) = applyR

let lift2 f a b = f <!> a <*> b
let lift3 f a b c = f <!> a <*> b <*> c
let lift4 f a b c d = f <!> a <*> b <*> c <*> d
//... ir t.t.

// kombainas:
let toTuple x y = x, y
let combine a b = lift2 toTuple a b
let doSthUsefulAfterCombine f a b = combine a b |> mapR f

// vienpusiai kombainai:
let ( <* ) x y = lift2 (fun left _right -> left) x y
let ( *> ) x y = lift2 (fun _left right -> right) x y
///////////////////////

// map from return and apply
let mapR_ra f x =
    let f' = returnR f
    applyR f' x

// apply from lift2
// fail:
let lift2R f a b =
    match a with
    | Success a' -> 
        match b with
        | Success b' -> Success (f a' b')
        | Failure e -> Failure e
    | Failure e -> Failure e

//better
let applyR_l2 a b =
    lift2R (fun f x -> f x) a b

// map from bind and return
let mapR_br f x =
    bindR x (fun a -> returnR (f a))


let rev f b a = f a b
// map defined in terms of bind and return (the fancy one) :)
let map f = 
    rev bindR (f >> Success)

// test with list 
let bindL f ( x : 'a list) =
    [for x' in x do
        for i in f x' do
            yield i]

let mapL f = 
    bindL ( f >> fun a -> [a] )

// apply from bind and return
// (M<a -> b>) -> M<a> -> M<b>
let applyR_br f x =
    match f with
    | Success f' -> bindR x (fun a -> returnR (f' a))
    | Failure e -> Failure e

// vo tep:
let applyR_br2 f x =
    f >>= fun f' ->
    x >>= fun x' ->
        returnR (f' x')

// apply defined in terms of bind and return (Option) (the fancy one) :)
let apply fOpt xOpt = 
    fOpt |> Option.bind (fun f -> 
        let map = Option.bind (f >> Some)
        map xOpt)

////////////////////////////
// Tokiame bind workflowe galima mąstyti taip:
// f kintamojo elevuota vertė išpakuojama kaip paprasta vertė į f' kintamąjį,
// paskui tas pats atliekama su x kintamojo verte (į x' kintamąjį)
// ir galiausiai atliekamas kažkoks skaičiavimas (normaliame pasaulyje),
// kuris vėl grąžina elevuotą vertę:
let returnOpt x = Some x
let applyOpt_br f x =
    let (>>=) x f = Option.bind f x
    f >>= fun f' ->
    x >>= fun x' ->
        returnOpt (f' x') // zis is the same...:

// as zis:
let applyOneMore f x = 
    Option.bind ( fun f' -> Option.bind ( fun x' -> returnOpt (f' x') ) x ) f


let test x =
    x >>= fun x' -> Success (x' + 1)
//////////////////////
    
let add x y = x + y
let addR x y = Success (x + y)

//apply applicationas :)
let addResultsTest() =
    applyR   (Success ((/) 10.)) (Success 456.)

let addResultChain () = (addR 5 5) >>= addR 5

// map testing
let add1 a = a + 1
let add1' a = map add1 a
let add1ToListElements = List.map add1
let addListsTogether = List.map2 add 

// aprėptoji funkcija : M<'a -> 'b>
// elevuota funkcija : M<'a> -> M<'b>
// it makes sense kodėl apply vadinas apply!! : 
// nes su apply applyinam ELEVUOTĄ VERTĘ APRĖPTAJAI FUNKCIJAI (taip pat kaip f x ,
// kuris vadinas tiesiog function APPLICATION!!) *fhwuoooo!!! pvz:
let add_retap a b = returnR add <*> a <*> b
//////////////

// "overloaded whitespace" :)
let add'' x y = add <!> x <*> y // == applyR(mapR add x) y

let add''' x y= applyR(mapR add x) y
let elevatedAdd x y = lift2 add x y

// traverse, sequence 
module ListyBoi =
    //traverse: ('a -> M<'b>) -> 'a collection -> M<'b collection>
    let rec traverseResultA f list =
        let cons head tail = head::tail
        match list with
        | [] -> returnR []
        | head::tail -> 
            let ret = returnR cons
            //ret <*> (f head) <*> (traverseResultA f tail)
            cons <!> (f head) <*> (traverseResultA f tail)
//cons : d -> d list -> d list
// mapR cons = R<d> -> R<d list -> d list>
// then apply R<d list -> d list> = R<d list> -> R<d list>
    
    let rec traverseResultM f list =
        let cons head tail = head::tail
        match list with
        | [] -> returnR []
        | head::tail -> f head >>= fun h ->
                        traverseResultM f tail >>= fun t ->
                        returnR (cons h t) 

    let traverseResultAFold f list =
        let cons head tail = head::tail
        let folder head tail = cons <!> f head <*> tail
        List.foldBack folder list (returnR [])
        
    let rec mapList f list =
        match list with 
        | [] -> []
        | head::tail -> (f head) :: (mapList f tail)
    let rec mapListFold f list = 
        let folder head tail = f head :: tail
        List.foldBack folder list []
        //List.fold folder [] list

    // sequence: M<'a> collection -> M<'a collection>
    let sequenceResultA list = traverseResultA id list
    let sequenceResultM list = traverseResultM id list

    // traverse yra tas pats, kas padaryt map ir po to sequence
    // map f |> sequence

    let rec sequenceResultLol list =
        let cons head tail = head::tail
        match list with
        | [] -> returnR []
        | head::tail -> cons <!> head <*> (sequenceResultLol tail)
    let traverseResultLol f list =
        mapL f list |> sequenceResultLol


// (mano) sąvokos : 
// M<'a> :       paaukštinta vertė (tipas)
// 'a    :       žema vertė (tipas)
// 'a -> 'b :    transformacijos funkcija
// 'a -> M<'b> : transformacijos bei elevacijos funkcija (world-crossing function) 
//                                                             (čia sutrumpintai: elevacijos funkcija)
// ('a -> M<'a> : (tikroji elevacijos funkcija [return])
//
// Mon... *pliaukšt... ?? funckijos:
// map : priima TRANSFORMACIJOS FUNKCIJĄ ir PAAUKŠTINTĄ VERTĘ ir grąžina transformuotą PAAUKŠTINTĄ VERTĘ
// 
// bind : priima pirmą PAAUKŠTINTĄ VERTĘ ir ELEVACIJOS FUNKCIJĄ ir grąžina kitą PAAUKTŠTINTĄ VERTĘ
// (pažemina pirmą PAAUKŠTINTĄ VERTĘ, ją transformuoja, ir grąžina transformuotą PAAUKŠTINTĄ VERTĘ)
//
// apply : priima PAAUKTŠTINTĄ TRANSFORMACIJOS FUNKCIJĄ ir kitą PAAUKŠTINTĄ VERTĘ ir grąžina 
// kitą PAAUKŠTINTĄ VERTĘ
// (pažemina PAAUKTŠTINTĄ TRANSFORMACIJOS FUNKCIJĄ, pažemina PAAUKŠTINTĄ VERTĘ, atlieka transformaciją 
// su pažeminta verte ir grąžiną transformuotą PAAUKŠTINTĄ VERTĘ
   
// viršuj bullshit for realz now :) :
// note, kai sakau vertė, tai turiu omeny tipą (kaip int, string ir pan.)
// tipai gali būti ir funkcijos tipai (tokie kaip string -> int ir pan.)
//
// paprasta vertė : 'a
// elevuota vertė : M<'a>
//
// (transformacijos)
//    paprastoji funkcija : 'a -> b'
// aprėptoji funkcija     :  M<'a -> 'b>
// elevuota funkcija      : M<'a> -> M<'b>
// elevacijos funkcija    : 'a -> M<'b>
//
// monadinės funckijos: 
// return ('a -> M<'a>):
// tiesiog paprastą vertę verčia į elevuotą vertę
// bind : (('a -> M<'b>) -> M<'a> -> M<'b>):
// suriša elevuotą ir papratą 'pasaulį' per elevacijos funkciją TODO: better explanation
// aplikatyvinės funkcijos:
// map (('a -> 'b) -> M<'a> -> M<'b>):
// paverčia paprastą funkciją į elevuotą funkciją
// apply (M<'a -> 'b> -> M<'a> -> M<'b>):
// verčia aprėptą funckiją į elevuotą funkciją (arba applyina elevuotą vertę 
// aprėptai funkcijai (duoda elevuotą inputą aprėptai funkcijai)