module CircleCenterInTriangleProblem

open System

let invert angle = (angle + 0.5) % 1.0

let isP3BetweenP1AndP2 p1 p2 p3 = p3 >= p1 && p3 <= p2

let calculateProbability testCount =
    let rand = new Random()
    let angles = [|0.;0.;0.|]

    let rec inner hitCount i = 
        if i >= testCount then 
            float (hitCount) / (float (testCount))
        else
            angles.[0] <- rand.NextDouble()
            angles.[1] <- rand.NextDouble()
            angles.[2] <- rand.NextDouble()
            array.Sort(angles)

            let isBeetween1 = isP3BetweenP1AndP2 (angles.[0]) (angles.[1]) (invert angles.[2])
            let isBeetween2 = isP3BetweenP1AndP2 (angles.[1]) (angles.[2]) (invert angles.[0])
            if isBeetween1 && isBeetween2 then inner (hitCount + 1) (i + 1)
            else inner hitCount (i + 1) // (!) lolol, naudojant pipe operatorių (<|) arba (|>) 
                                        //debug mode meta stack overflow :D (release nemeta)
    
    inner 0 0
