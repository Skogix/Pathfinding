module Pathfinding.BreadthFirst

open Pathfinding.Core.Domain.BreadthFirst
open Pathfinding.Core.Domain.Grid
open Pathfinding.Core.State
open Pathfinding.Core.Common

/// breadth first pseudo
/// open, closed
///   newOpen:
///     getneighbors open.first
///     |> removeAlreadyInOpen
///     |> removeAlreadyInClosed
///     |> setCost
///   newClosed:
///     open.first :: closed
/// newOpen, newClosed
///
 
/// runs the pathfinding one step
let runOnce (openNodes:OpenNodes) (closedNodes:ClosedNodes) grid: OpenNodes * ClosedNodes =
  let outputOpen, outputClosed =
    match openNodes with
    | first::rest ->
      let newOpen =
        getNeighbors first.position grid 
        |> List.filter(filterInList openNodes)
        |> List.filter(filterInList closedNodes)
        |> List.map(createNode (Some first.position) (first.cost + 1))
      (rest @ newOpen, first :: closedNodes)
    | [] -> (openNodes, closedNodes)
  (outputOpen, outputClosed)
/// runs the pathfinding until no more open nodes
let run (data:Data) grid =
  let rec loop (openNodes:OpenNodes) closedNodes =
    match openNodes with
    | [] -> closedNodes
    | list ->
      let newOpen, newClosed = runOnce list closedNodes grid
      loop newOpen newClosed
  loop data.openNodes data.closedNodes
/// if the target exists in closed nodes, get the parent position chain to the closest start
let tryFindBackHome (target:Position) (closedNodes:Node list) =
  match closedNodes |> List.exists(fun n -> n.position = target) with
  | true -> 
    let getNode pos = closedNodes |> List.find(fun node -> node.position = pos)
    let rec loop (currentNode:Node) path =
      match currentNode.parent with
      | Some parentPos -> loop (getNode parentPos) (parentPos::path)
      | None -> Some path
    loop (getNode target) [target]
  | false -> None
  
// will probably be expanded alot later
let tryGetSolution start target (closedNodes: Node list): Solution option =
  match (tryFindBackHome target closedNodes) with
  | Some solution ->
    Some {
      start = start
      target = target
      path = solution
    }
  | None -> None
// will probably be expanded alot later
let getSolutions start target (closedNodes:ClosedNodes): Solution list =
  match (tryGetSolution start target closedNodes) with
  | Some solution -> [solution]
  | None -> []
