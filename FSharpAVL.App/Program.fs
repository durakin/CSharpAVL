open FSharpAVL.AvlTree

let trees numbers =
    let mutable tree = ofItem (numbers |> Seq.head)

    seq {
        yield tree

        for i in numbers do
            tree <- insert tree i
            yield tree
    }

[<EntryPoint>]
let main _ =
    let inputs =
        seq {
            while true do
                match System.Double.TryParse(System.Console.ReadLine()) with
                | true, a -> yield a
                | _ -> eprintfn "Wrong"
        }

    let trees = trees inputs

    for tree in trees do
        printfn "----"

        tree
        |> dfs
            (fun subTree ->
                printfn $"   %A{rootItem subTree} -> %i{balanceFactor subTree}"
                None)
        |> ignore

        printfn $"  %A{toSeq tree |> Seq.toList}"

        printfn $" %A{tree}"
        printfn "----"

    0
