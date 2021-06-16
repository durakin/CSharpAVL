namespace global

open System

[<AutoOpen>]
module Utils =
    let inline (^) a b = a b

    let inline unimplemented a =
        match a with
        | "" -> raise ^ NotImplementedException()
        | _ -> raise ^ NotImplementedException a

    let (|Less|Equal|Greater|) n =
        if n < 0 then Less
        elif n > 0 then Greater
        else Equal

    let inline unreachable (_: unit) =
        raise ^ InvalidOperationException("Unreachable")
