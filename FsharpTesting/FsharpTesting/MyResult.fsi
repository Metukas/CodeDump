module MyResult
type Result<'TSuccess,'TFailure> =
  | Success of 'TSuccess
  | Failure of 'TFailure
val bindR : x:Result<'a,'b> -> f:('a -> Result<'c,'b>) -> Result<'c,'b>
val ( >>= ) : (Result<'a,'b> -> ('a -> Result<'c,'b>) -> Result<'c,'b>)
val zero : unit -> arg0:'a -> Result<'b,'a>
val returnR : x:'a -> Result<'a,'b>
val returnFromR : x:Result<'a,'b> -> Result<'a,'b>
val mapRRR : f:('a -> 'b) -> x:'a -> Result<'b,'c>
val mapR : f:('a -> 'b) -> x:Result<'a,'c> -> Result<'b,'c>
val applyR : f:Result<('a -> 'b),'c> -> x:Result<'a,'c> -> Result<'b,'c>
val ( <!> ) : (('a -> 'b) -> Result<'a,'c> -> Result<'b,'c>)
val ( <*> ) : (Result<('a -> 'b),'c> -> Result<'a,'c> -> Result<'b,'c>)
val lift2 :
  f:('a -> 'b -> 'c) -> a:Result<'a,'d> -> b:Result<'b,'d> -> Result<'c,'d>
val lift3 :
  f:('a -> 'b -> 'c -> 'd) ->
    a:Result<'a,'e> -> b:Result<'b,'e> -> c:Result<'c,'e> -> Result<'d,'e>
val lift4 :
  f:('a -> 'b -> 'c -> 'd -> 'e) ->
    a:Result<'a,'f> ->
      b:Result<'b,'f> -> c:Result<'c,'f> -> d:Result<'d,'f> -> Result<'e,'f>
val toTuple : x:'a -> y:'b -> 'a * 'b
val combine : a:Result<'a,'b> -> b:Result<'c,'b> -> Result<('a * 'c),'b>
val doSthUsefulAfterCombine :
  f:('a * 'b -> 'c) -> a:Result<'a,'d> -> b:Result<'b,'d> -> Result<'c,'d>
val ( <* ) : x:Result<'a,'b> -> y:Result<'c,'b> -> Result<'a,'b>
val ( *> ) : x:Result<'a,'b> -> y:Result<'c,'b> -> Result<'c,'b>
val mapR_ra : f:('a -> 'b) -> x:Result<'a,'c> -> Result<'b,'c>
val lift2R :
  f:('a -> 'b -> 'c) -> a:Result<'a,'d> -> b:Result<'b,'d> -> Result<'c,'d>
val applyR_l2 : a:Result<('a -> 'b),'c> -> b:Result<'a,'c> -> Result<'b,'c>
val mapR_br : f:('a -> 'b) -> x:Result<'a,'c> -> Result<'b,'c>
val rev : f:('a -> 'b -> 'c) -> b:'b -> a:'a -> 'c
val map : f:('a -> 'b) -> (Result<'a,'c> -> Result<'b,'c>)
val bindL : f:('a -> #seq<'b>) -> x:'a list -> 'b list
val mapL : f:('a -> 'b) -> ('a list -> 'b list)
val applyR_br : f:Result<('a -> 'b),'c> -> x:Result<'a,'c> -> Result<'b,'c>
val apply : fOpt:('a -> 'b) option -> xOpt:'a option -> 'b option
val add : x:int -> y:int -> int
val addR : x:int -> y:int -> Result<int,'a>
val addResultsTest : unit -> Result<float,'a>
val addResultChain : unit -> Result<int,'a>
val add1 : a:int -> int
val add1' : a:Result<int,'a> -> Result<int,'a>
val add1ToListElements : (int list -> int list)
val addListsTogether : (int list -> int list -> int list)
val add_retap : a:Result<int,'a> -> b:Result<int,'a> -> Result<int,'a>
val add'' : x:Result<int,'a> -> y:Result<int,'a> -> Result<int,'a>
val add''' : x:Result<int,'a> -> y:Result<int,'a> -> Result<int,'a>
val elevatedAdd : x:Result<int,'a> -> y:Result<int,'a> -> Result<int,'a>

