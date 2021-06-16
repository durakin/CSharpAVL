open FSharpAVL

let trees numbers =
    let mutable tree = AvlTree.ofItem (numbers |> Seq.head)

    seq {
        yield tree

        for i in numbers do
            tree <- AvlTree.insert tree i
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
        printfn "-----"

        tree
        |> AvlTree.dfs
            (fun subTree ->
                printfn $"   %A{AvlTree.rootItem subTree} -> %i{AvlTree.balanceFactor subTree}"
                None)
        |> ignore

        printfn $"  %A{AvlTree.toSeq tree |> Seq.toList}"

        let _set = AvlTree.toImmutableSet tree
        printfn $" %A{tree}"
        printfn "----"

    0
