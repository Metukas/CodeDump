namespace Haskell.Prelude

type IO<'T> = private | Action of (unit -> 'T)

[<AutoOpen>]
module MonadIO =
    let raw  (Action f) = f // standartinis single union case "unwrapinimas"
    let run  io         = raw io () // iškviečia funkciją, "wrapintą" IO tipo Action case'e
    let private eff  g   io     = raw io () |> g // kažkas panašaus į map(??), tik kad "elevuotos" funkcijos outputas nėra pats elevuotas (keistai taip)
    let private bind io  rest   = Action (fun () -> io |> eff rest |> run) // std bind :)
    let private comb io1 io2    = Action (fun () -> run io1; run io2) // std combine (gal)
    
    // šitas tipas kuriamas tik dėl patogios sintaksės, visas funkcionalumas yra defined viršuj
    type IOBuilder() =
        member b.Return(x)              = Action (fun () -> x) // std returnas
        member b.ReturnFrom(io) : IO<_> = io // std returnFrom
        member b.Delay(g) : IO<_>       = g () // std delay
        member b.Bind(io, rest)         = bind io rest // std bind
        member b.Combine(io1, io2)      = comb io1 io2 //std combine
    
    let io = new IOBuilder()
    let (|Action|) io = run io // active pattern (kaip jie veikia????)

[<AutoOpen>]
module PreludeIO =
    // visos outputo funkcijos turi IO<unit> tipą, o inputo - IO<'a> tipo, kur 'a yra tipas, kuris 
    // yra skaitomas (šiuo atveju, visi jie yra string)
    let putChar  (c:char)   = Action (fun () -> stdout.Write(c))
    let putStr   (s:string) = Action (fun () -> stdout.Write(s))
    let putStrLn (s:string) = Action (fun () -> stdout.WriteLine(s))
    let print x             = Action (fun () -> printfn "%A" x)
    let getChar     = Action (fun () -> stdin.Read() |> char |> string)
    let getLine     = Action (fun () -> stdin.ReadLine())
    let getContents = Action (fun () -> stdin.ReadToEnd())
    //
    let getCharC = Action (fun () -> stdin.Read() |> char)

open System

module Test =
    let lines (s:string) = s.Split([|stdout.NewLine|], StringSplitOptions.None) |> Seq.ofArray
    let length xs = Seq.length xs
    
    let test () =
        // get/put two lines
        let (Action ()) = io {
            let! cs1 = getLine
            let! cs2 = getLine
            return! putStrLn cs1
            return! putStrLn cs2
        }
        // cat
        let (Action ()) = io {
            let! cs = getContents
            return! putStr cs
        }
        // wc -l
        let (Action ()) = io {
            let! cs = getContents
            return! cs |> lines |> length |> print
        }

        // mano nesąmonė
        let ioAction = io {
            let! cs = getCharC
            return! putChar cs
        }
        ( ioAction |> raw ) ()
        run ioAction
        // active recognizer'is ( let (Action ()) ) iš karto atlieka IO operaciją,
        // jeigu io {...} bindinam su kokiu nors kintamuoju, tai operacija atliekama tik tada, kai
        // unwrapinam tą kintamąjį ir iškviečiam atwrapintą funkciją ( (ioAction |> raw)() arba tiesiog run ioAction)
        0