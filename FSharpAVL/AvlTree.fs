module FSharpAVL.AvlTree

open FSharpAVL.Types

let ofItem item = Nil item

let rootItem tree =
    match tree with
    | Nil item -> item
    | Left (item, _) -> item
    | Right (item, _) -> item
    | Both (item, _, _) -> item

let rec height tree =
    match tree with
    | Nil _ -> 1
    | Left (_, leftSubtree) -> 1 + height leftSubtree
    | Right (_, rightSubtree) -> 1 + height rightSubtree
    | Both (_, leftSubtree, rightSubtree) ->
        (height leftSubtree, height rightSubtree)
        ||> max
        |> (+) 1

let leftChild tree =
    match tree with
    | Nil _ -> None
    | Left (_, l) -> Some l
    | Right _ -> None
    | Both (_, l, _) -> Some l

let rightChild tree =
    match tree with
    | Nil _ -> None
    | Left _ -> None
    | Right (_, r) -> Some r
    | Both (_, _, r) -> Some r

let balanceFactor tree =
    match tree with
    | Nil _ -> 0
    | Left (_, l) -> -(height l)
    | Right (_, r) -> height r
    | Both (_, l, r) -> height r - height l

let ofOptions item (a, b) =
    match a, b with
    | None, None -> Nil item
    | Some l, None -> Left(item, l)
    | None, Some r -> Right(item, r)
    | Some l, Some r -> Both(item, l, r)

let rec rebuild tree =
    match tree with
    | Nil a -> Nil a
    | Left (a, l) -> Left(a, rebuild l)
    | Right (a, r) -> Right(a, rebuild r)
    | Both (a, l, r) -> Both(a, rebuild l, rebuild r)

let replace fn tree =
    let rec rebuild' tree =
        match tree with
        | Nil a -> Nil a
        | Left (a, l) -> Left(a, fn l |> Option.defaultValue (rebuild' l))
        | Right (a, r) -> Right(a, fn r |> Option.defaultValue (rebuild' r))
        | Both (a, l, r) -> Both(a, fn l |> Option.defaultValue (rebuild' l), fn r |> Option.defaultValue (rebuild' r))

    rebuild' tree

let replace' fn tree =
    let rec rebuild' tree =
        fn tree
        |> Option.defaultValue (
            match tree with
            | Nil a -> Nil a
            | Left (a, l) -> Left(a, rebuild' l)
            | Right (a, r) -> Right(a, rebuild' r)
            | Both (a, l, r) -> Both(a, rebuild' l, rebuild' r)
        )

    rebuild' tree

let rotateLeft z x =
    let t1 = leftChild x
    let t23 = leftChild z
    let t4 = rightChild z

    let newX = ofOptions (rootItem x) (t1, t23)
    let newZ = ofOptions (rootItem z) (Some newX, t4)

    newZ

let rotateRight z x =
    let t4 = leftChild z
    let t23 = rightChild z
    let t1 = rightChild x

    let newX = ofOptions (rootItem x) (t23, t1)
    let newZ = ofOptions (rootItem z) (t4, Some newX)

    newZ

let rotateRightLeft (y, z) x =
    let t1 = leftChild x
    let t2 = leftChild y
    let t3 = rightChild y
    let t4 = rightChild z

    let newX = ofOptions (rootItem x) (t1, t2)
    let newZ = ofOptions (rootItem z) (t3, t4)

    let newY =
        ofOptions (rootItem y) (Some newX, Some newZ)

    newY

let rotateLeftRight (y, z) x =
    let t4 = leftChild z
    let t3 = leftChild y
    let t2 = rightChild y
    let t1 = rightChild x

    let newZ = ofOptions (rootItem z) (t4, t3)
    let newX = ofOptions (rootItem x) (t2, t1)

    let newY =
        ofOptions (rootItem y) (Some newZ, Some newX)

    newY

let rotateTree fn (x, z) parent =
    replace
        (fun subtree ->
            if subtree = parent then
                Some ^ fn (x, z) parent
            else
                None)

//let rec insertUnbalanced tree newItem =
//    match tree with
//    | Nil item ->
//        match compare newItem item with
//        | Less -> Left(item, ofItem newItem)
//        | Equal -> tree
//        | Greater -> Right(item, ofItem newItem)
//    | Left (item, l) ->
//        match compare newItem item with
//        | Less -> Left(item, insertUnbalanced l newItem)
//        | Equal -> tree
//        | Greater -> Both(item, l, ofItem newItem)
//    | Right (item, r) ->
//        match compare newItem item with
//        | Less -> Both(item, ofItem newItem, r)
//        | Equal -> tree
//        | Greater -> Right(item, insertUnbalanced r newItem)
//    | Both (item, l, r) ->
//        match compare newItem item with
//        | Less -> Both(item, insertUnbalanced l newItem, r)
//        | Equal -> tree
//        | Greater -> Both(item, l, insertUnbalanced r newItem)

type private Violation =
    | RightRight
    | LeftLeft
    | RightLeft
    | LeftRight

let rec insert tree newItem =
    let newTree =
        match tree with
        | Nil item ->
            match compare newItem item with
            | Less -> Left(item, ofItem newItem)
            | Equal -> tree
            | Greater -> Right(item, ofItem newItem)
        | Left (item, l) ->
            match compare newItem item with
            | Less -> Left(item, insert l newItem)
            | Equal -> tree
            | Greater -> Both(item, l, ofItem newItem)
        | Right (item, r) ->
            match compare newItem item with
            | Less -> Both(item, ofItem newItem, r)
            | Equal -> tree
            | Greater -> Right(item, insert r newItem)
        | Both (item, l, r) ->
            match compare newItem item with
            | Less -> Both(item, insert l newItem, r)
            | Equal -> tree
            | Greater -> Both(item, l, insert r newItem)

    let x = newTree

    if abs (balanceFactor x) = 2 then
        let lZ = leftChild x
        let rZ = rightChild x

        let newX =
            match lZ, rZ with
            | _, Some z when balanceFactor z >= 0 -> rotateLeft z x
            | Some z, _ when balanceFactor z <= 0 -> rotateRight z x
            | _, Some z when balanceFactor z < 0 ->
                let y = leftChild z
                rotateRightLeft (y.Value, z) x
            | Some z, _ when balanceFactor z > 0 ->
                let y = rightChild z
                rotateLeftRight (y.Value, z) x
            | _ -> x

        newX
    else
        x

let rec toSeq tree =
    seq {
        match tree with
        | Nil item -> yield item
        | Left (item, l) ->
            yield! toSeq l
            yield item
        | Right (item, r) ->
            yield item
            yield! toSeq r
        | Both (item, l, r) ->
            yield! toSeq l
            yield item
            yield! toSeq r
    }
