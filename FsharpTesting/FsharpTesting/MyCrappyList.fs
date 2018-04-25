module MyCrappyList
open Unchecked

type MyCrappyList<'T> (item1, item2, item3, item4, item5) =
    let _1 = item1
    let _2 = item2
    let _3 = item3
    let _4 = item4
    let _5 = item5
    
    new () = MyCrappyList<'T> (defaultof<'T>, defaultof<'T>, defaultof<'T>, defaultof<'T>, defaultof<'T>)
    new (item1) = MyCrappyList<'T> (item1, defaultof<'T>, defaultof<'T>, defaultof<'T>, defaultof<'T>)
    new (item1, item2) = MyCrappyList<'T> (item1, item2, defaultof<'T>, defaultof<'T>, defaultof<'T>)
    new (item1, item2, item3) = MyCrappyList<'T> (item1, item2, item3, defaultof<'T>, defaultof<'T>)
    new (item1, item2, item3, item4) = MyCrappyList<'T> (item1, item2, item3, item4, defaultof<'T>)
    new (crappyList : MyCrappyList<'T>) = MyCrappyList<'T> (crappyList.Item1, crappyList.Item2, 
                                             crappyList.Item3, crappyList.Item4, crappyList.Item5)

    member this.Item1 = item1
    member this.Item2 = item2
    member this.Item3 = item3
    member this.Item4 = item4
    member this.Item5 = item5


let Map (x : 'T) f =
    let newItem1 : 'T1 = f x
    MyCrappyList<'T1> (newItem1)

let MapL (x : MyCrappyList<'T>) f =
    let newItem1 : 'T1 = f x.Item1
    let newItem2 : 'T1 = f x.Item2
    let newItem3 : 'T1 = f x.Item3
    let newItem4 : 'T1 = f x.Item4
    let newItem5 : 'T1 = f x.Item5
    MyCrappyList<'T1> (newItem1, newItem2, newItem3, newItem4, newItem5)

let Bind (x: MyCrappyList<'T>) (f : 'T -> MyCrappyList<'T1>) =
    let newItem1 = f x.Item1
    let newItem2 = f x.Item2
    let newItem3 = f x.Item3
    let newItem4 = f x.Item4
    let newItem5 = f x.Item5
    MyCrappyList<'T1> (newItem1.Item1, newItem2.Item1, newItem3.Item1, newItem4.Item1, newItem5.Item1)

let Apply (x : MyCrappyList<'T>) (f : MyCrappyList<'T -> 'T1>) =
    let fun1 = f.Item1
    let fun2 = f.Item2
    let fun3 = f.Item3
    let fun4 = f.Item4
    let fun5 = f.Item5
    let newItem1 = fun1 x.Item1
    let newItem2 = fun2 x.Item2
    let newItem3 = fun3 x.Item3
    let newItem4 = fun4 x.Item4
    let newItem5 = fun5 x.Item5
    MyCrappyList<'T1> (newItem1, newItem2, newItem3, newItem4, newItem5)


let bindList (f: 'a->'b list) (xList: 'a list)  = 
        [ for x in xList do 
          for y in f x do 
              yield y ]


