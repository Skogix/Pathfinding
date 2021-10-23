module Pathfinding.Core.State

open Pathfinding.Core.Domain
open Pathfinding.Core.Domain.Grid

type Solution = {
  start: Position
  target: Position
  path: Position list }
type View =
  | GridView
  | SettingsView
type State = {
  Grid: Grid
  Solutions: Solution list
  BreadthFirstData: BreadthFirst.Data
  View: View
  settings: Settings.Settings
} with
  member this.updateGrid pos state = {this with Grid = this.Grid.Add(pos, state)}
  member this.getStart = this.Grid |> Map.filter(fun pos terrain -> terrain = Start) |> List.ofSeq |> List.map(fun x -> x.Key) |> List.head
  member this.getTarget = this.Grid |> Map.filter(fun pos terrain -> terrain = Target) |> List.ofSeq |> List.map(fun x -> x.Key) |> List.head
  member this.openNodesPositionList = this.BreadthFirstData.OpenNodes |> List.map(fun n -> n.Position)
  member this.closedNodesPositionList = this.BreadthFirstData.ClosedNodes |> List.map(fun n -> n.Position)
  member this.solutionsContainsPos pos =
    this.Solutions
    |> List.map(fun sol -> sol.path)
    |> List.concat
    |> List.contains pos