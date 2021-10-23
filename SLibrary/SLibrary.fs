namespace SLibrary

open System.Runtime.InteropServices

///
/// generic functions that i want to include in other projects later
///

module Monad =
  type SkogixMonad<'result, 'data> =
    | Result of 'result
    | Data of 'data
  let bind func monad =
    match monad with
    | Result result -> Result result
    | Data data -> func data
  let (|>>) monad func =
    match monad with
    | Result result -> Result result
    | Data data -> func data
  let (|<<) monad string =
    match monad with
    | Result result -> result
    | Data _ -> string
module Map =
  /// Wrapper type, Monadic type
  /// map, lift, select
  /// <$> <!>
  /// lifts a function
  /// (a-b) -> X<a> -> X<b>
  let mapOption f opt =
    match opt with
    | None -> None
    | Some x -> Some (f x)
  let rec mapList f list =
    match list with
    | [] -> []
    | first::rest -> (f first)::(mapList f rest)
module Return =
  /// return, pure, unit, yield, point
  /// lifts a single value
  /// a -> X<a>
  let returnOption x = Some x
  let returnList x = [x]
module Apply =
  /// Applicative Functor
  /// apply, ap
  /// <*>
  /// unpacks a lifted function from a lifted value to a lifted function
  /// X<(a->b> -> X<a> -> X<b>
  let applyOption fOpt xOpt =
    match fOpt, xOpt with
    | Some f, Some x -> Some (f x)
    | _ -> None
  let applyList (fList: ('a -> 'b) list) (xList: 'a list) =
    [
      for f in fList do
      for x in xList do
        yield f x
    ]
  let add x y = x + y
  let applyListTest1 =
    let (<*>) = applyList
    [add] <*> [1;2] <*> [10;20]
    // [11;21;12;22]
  let applyResultOption1 =
    let (<*>) = applyOption
    (Some add) <*> (Some 2) <*> (Some 3)
    // Some 5
  let applyResultOption2 =
    let (<!>) = Map.mapOption
    let (<*>) = applyOption
    add <!> (Some 2) <*> (Some 3)
  let applyListTest2 =
    let (<!>) = Map.mapList
    let (<*>) = applyList
    add <!> [1;2] <*> [10;20]
  let testStringConcat =
    let (<!>) = Map.mapList
    let (<*>) = applyList
    (+) <!> ["foo";"bar";"baz"] <*> ["!";"!?!"]
    // foo!; foo!?"; bar!; bar!?!; baz!; baz!?!
module Lift =
  /// lift2, lift2... (lift1 = map)
  /// combine x lifter values using a function
  /// lift2: (a->b->c) -> X<a> -> X<b> -> X<c>
  let (<*>) = Apply.applyList
  let (<!>) = List.map
  let lift2 f x y = f <!> x <*> y
  let lift3 f x y z = f <!> x <*> y <*> z
  let lift4 f x y z w = f <!> x <*> y <*> z <*> w
  
  let add x y = x + y
  let addPairList = lift2 add
  //  addPairOption (Some 1) (Some 2) |> ignore
  // Some 3
  
  
  let lift2FromScratch f xOpt yOpt =
    match xOpt, yOpt with
    | Some x, Some y -> Some (f x y)
    | _ -> None
  let applyFromLift2 fOpt xOpt =
    lift2FromScratch id fOpt xOpt
  let ( <* ) x y = lift2 (fun left right -> left) x y
  let ( *> ) x y = lift2 (fun left right -> left) x y
  [1;2] <* [3;4;5] |> ignore
  // [1;1;1;2;2;2]
  [1;2] *> [3;4;5] |> ignore
  // [3;4;5;3;4;5]
  let repeat n pattern = [1..n] *> pattern
  let replicate n x = [1..n] *> [x]
module Zip =
  /// zip,zip3 map2
  /// <*>
  /// combine two enumerables with a function
  /// E<(a->b->c)> -> E<a> -> E<b> -> E<c> // when E is enumerable
  /// E<a> -> E<b> -> E<a,b> // when E is a tuple
  let rec zipList fList xList =
    match fList, xList with
    | [], _ -> []
    | _, [] -> []
    | (f::fTail), (x::xTail) ->
    (f x) (zipList fTail xTail)
//  let test =
//    let add10 x = x + 10
//    let add20 x = x + 20
//    let add30 x = x + 30
//    let (<*>) = zipList
//    [add10;add20;add30] <*> [1;2;3]
module Bind =
  /// like a bind but monadic instead of applicative
  /// bind, flatMap, andThen, collect, selectMany
  /// >>= (left to right) =<< (right to left)
  /// normal to monadic functions
  /// (a -> X<b>) -> X<a> -> X<B>
  let optionBind f xOpt =
    match xOpt with
    | Some x -> f x
    | _ -> None
  let listBind (f: 'a -> 'b list) (xList: 'a list) =
    [
      for x in xList do
      for y in f x do
        yield y
    ]