open FSharpAVL.AvlTree

let trees ints =
    let mutable tree = ofItem (ints |> Seq.head)

    seq {
        yield tree

        for i in ints do
            tree <- insertUnbalanced tree i
            yield tree
    }

[<EntryPoint>]
let main _ =
    let inputs =
        seq {
            while true do
                int ^ System.Console.ReadLine()
        }

    let trees = trees inputs

    for tree in trees do
        printfn "----"

        tree
        |> replace'
            (fun subTree ->
                printfn $"  %A{subTree} -> %i{balanceFactor subTree}"
                None)
        |> ignore

        printfn $" %A{tree}"
        printfn "----"

    0
