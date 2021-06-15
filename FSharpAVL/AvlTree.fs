module FSharpAVL.AvlTree

open FSharpAVL.Types

let create a = Nil a

let rec height tree =
    match tree with
    | Nil _ -> 1
    | Left (_, leftSubtree) -> 1 + height leftSubtree
    | Right (_, rightSubtree) -> 1 + height rightSubtree
    | Both (_, leftSubtree, rightSubtree) -> 1 + height leftSubtree + height rightSubtree

let balanceFactor rightSubtree leftSubtree =
    height rightSubtree - height leftSubtree

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

let insert tree newElement =
    match tree with
    | Nil element ->
        match compare element newElement with
        | Less -> Left(newElement, tree)
        | Equal -> tree
        | Greater -> Right(newElement, tree)
    | Left _ -> unimplemented ""
    | Right _ -> unimplemented ""
    | Both _ -> unimplemented ""
