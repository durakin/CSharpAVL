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
    | Left (_, l) -> 1 + height l
    | Right (_, r) -> 1 + height r
    | Both (_, l, r) -> (height l, height r) ||> max |> (+) 1

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

let dfs replacer tree =
    let rec dfs' tree =
        replacer tree
        |> Option.defaultValue
           ^ match tree with
             | Nil a -> Nil a
             | Left (a, l) -> Left(a, dfs' l)
             | Right (a, r) -> Right(a, dfs' r)
             | Both (a, l, r) -> Both(a, dfs' l, dfs' r)

    dfs' tree

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

let rec insertUnbalanced tree newItem =
    match tree with
    | Nil item ->
        match compare newItem item with
        | Less -> Left(item, ofItem newItem)
        | Equal -> tree
        | Greater -> Right(item, ofItem newItem)
    | Left (item, l) ->
        match compare newItem item with
        | Less -> Left(item, insertUnbalanced l newItem)
        | Equal -> tree
        | Greater -> Both(item, l, ofItem newItem)
    | Right (item, r) ->
        match compare newItem item with
        | Less -> Both(item, ofItem newItem, r)
        | Equal -> tree
        | Greater -> Right(item, insertUnbalanced r newItem)
    | Both (item, l, r) ->
        match compare newItem item with
        | Less -> Both(item, insertUnbalanced l newItem, r)
        | Equal -> tree
        | Greater -> Both(item, l, insertUnbalanced r newItem)

let rec insert tree newItem =
    let x =
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

    if abs (balanceFactor x) = 2 then
        match leftChild x, rightChild x with
        | _, Some z when balanceFactor z >= 0 -> rotateLeft z x
        | Some z, _ when balanceFactor z <= 0 -> rotateRight z x
        | _, Some z when balanceFactor z < 0 ->
            match leftChild z with
            | Some y -> rotateRightLeft (y, z) x
            | None -> unreachable ()
        | Some z, _ when balanceFactor z > 0 ->
            match rightChild z with
            | Some y -> rotateLeftRight (y, z) x
            | None -> unreachable ()
        | _ -> unreachable ()
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

let rec count tree =
    match tree with
    | Nil _ -> 1
    | Left (_, l) -> 1 + count l
    | Right (_, r) -> 1 + count r
    | Both (_, l, r) -> 1 + count l + count r

open System.Collections.Immutable
open System.Collections.Generic
open System.Collections

let rec toImmutableSet tree =
    { new IImmutableSet<_> with
        member this.Count = count tree

        member this.Add(a) = insert tree a |> toImmutableSet

        member this.Clear() = unimplemented ""

        member this.Contains(a) = unimplemented ""

        member this.Except(a) = unimplemented ""

        member this.GetEnumerator() : IEnumerator<_> = (toSeq tree).GetEnumerator()

        member this.GetEnumerator() : IEnumerator =
            (toSeq tree :> IEnumerable).GetEnumerator()

        member this.Intersect(a) = unimplemented ""

        member this.IsProperSubsetOf(a) = unimplemented ""

        member this.IsProperSupersetOf(a) = unimplemented ""

        member this.IsSubsetOf(a) = unimplemented ""

        member this.IsSupersetOf(a) = unimplemented ""

        member this.Overlaps(a) = unimplemented ""

        member this.Remove(a) = unimplemented ""

        member this.SetEquals(a) = unimplemented ""

        member this.SymmetricExcept(a) = unimplemented ""

        member this.TryGetValue(a, b) = unimplemented ""

        member this.Union(a) = unimplemented "" }
