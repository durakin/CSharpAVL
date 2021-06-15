open FSharpAVL.AvlTree

let trees numbers =
    let mutable tree = ofItem (numbers |> Seq.head)

    seq {
        yield tree

        for i in numbers do
            tree <- insertUnbalanced tree i
            yield tree
    }

[<EntryPoint>]
let main _ =
    let inputs =
        seq {
            while true do
                match System.Int32.TryParse(System.Console.ReadLine()) with
                | true, a -> yield a
                | _ -> eprintfn "Wrong"
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
