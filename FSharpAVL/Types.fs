namespace FSharpAVL.Types

type AvlTree<'T when 'T: comparison> =
    | Nil of 'T
    | Left of 'T * AvlTree<'T>
    | Right of 'T * AvlTree<'T>
    | Both of 'T * AvlTree<'T> * AvlTree<'T>
