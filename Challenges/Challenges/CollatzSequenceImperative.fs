module CollatzSequenceImperative

type CollatzSequenceEnumerator(current : int64) =
    let mutable _current = current //lolol :)
    member theInstanceOfThisParticularObject.current with get() = _current and set(value) = _current <- value
    //member val pvz = 0 with get, set // sintaksės pvz.
    member this.MoveNext() : bool =
        if this.current = 1L then
            false
        else if ((this.current &&& 1L) = 0L) then
            this.current <- this.current / 2L
            true
        else
            this.current <- this.current * 3L + 1L
            true

    member this.Count() : int64 =
        let counter  = ref 1
        while this.MoveNext() do
            incr counter
        int64 counter.Value
    
let getCollatzEnumerator num = new CollatzSequenceEnumerator(num)


// pvz:
//type Region() =
//  let mutable t = 0.0f
//  member val Width = 0.0f
//  member x.Height = 0.0f
//  member val Left = 0.0f with get,set
//  member x.Top with get() = 0.0f and set(value) = t <- value

// COMPILES TO:

//public class Region
//{
//    internal float t;
//
//    internal float Width@;
//
//    internal float Left@;
//
//    public float Width
//    {
//        get
//        {
//            return this.Width@;
//        }
//    }
//
//    public float Height
//    {
//        get
//        {
//            return 0f;
//        }
//    }
//
//    public float Left
//    {
//        get
//        {
//            return this.Left@;
//        }
//        set
//        {
//            this.Left@ = value;
//        }
//    }
//
//    public float Top
//    {
//        get
//        {
//            return 0f;
//        }
//        set
//        {
//            this.t = value;
//        }
//    }
//
//    public Region() : this()
//    {
//        this.t = 0f;
//        this.Width@ = 0f;
//        this.Left@ = 0f;
//    }
//}

//\pvz