module Pathfinding.Core.State

open Pathfinding.Core.Domain
open Pathfinding.Core.Domain.Grid

type Solution = {
  start: Position
  target: Position
  path: Position list
}
type View =
  | GridView
  | SettingsView
type State = {
  grid: Grid
  solutions: Solution list
  breadthFirstData: BreadthFirst.Data
  view: View
  settings: Settings.Settings
} with
  member this.updateGrid pos state = {this with grid = this.grid.Add(pos, state)}
  member this.getStart = this.grid |> Map.filter(fun pos terrain -> terrain = Start) |> List.ofSeq |> List.map(fun x -> x.Key) |> List.head
  member this.getTarget = this.grid |> Map.filter(fun pos terrain -> terrain = Target) |> List.ofSeq |> List.map(fun x -> x.Key) |> List.head
  member this.openNodesPositionList = this.breadthFirstData.openNodes |> List.map(fun n -> n.position)
  member this.closedNodesPositionList = this.breadthFirstData.closedNodes |> List.map(fun n -> n.position)
  member this.solutionsContainsPos pos =
    this.solutions
    |> List.map(fun sol -> sol.path)
    |> List.concat
    |> List.contains pos