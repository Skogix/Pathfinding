module Pathfinding.Core.Common

open Pathfinding.Core.Domain.BreadthFirst
open Pathfinding.Core.Domain.Grid
open Pathfinding.Core.State

let createPosition x y = {x=x;y=y}
let upFrom pos = { x = pos.x; y = pos.y - 1 }
let downFrom pos = { x = pos.x; y = pos.y + 1 }
let leftFrom pos = { x = pos.x + 1; y = pos.y }
let rightFrom pos = { x = pos.x - 1; y = pos.y }
let getNeighbors (pos:Position) (state:State) =
  let output =
    [ pos |> downFrom
      pos |> rightFrom
      pos |> leftFrom
      pos |> upFrom
      if state.settings.Diagonal then
        pos |> downFrom |> rightFrom
        pos |> downFrom |> leftFrom
        pos |> upFrom |> rightFrom
        pos |> upFrom |> leftFrom ] 
  output
  |> List.filter(state.Grid.ContainsKey)
  |> List.filter(fun pos ->
    state.Grid.[pos] = Walkable ||
    state.Grid.[pos] = Target)
let filterInList (nodes:Node list) pos =
  nodes
  |> List.map(fun node -> node.Position)
  |> List.contains(pos)
  |> not
