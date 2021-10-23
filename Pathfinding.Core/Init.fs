module Pathfinding.Core.Init

open System
open Pathfinding.Core.Domain.Grid
open Pathfinding.Core.State
open Pathfinding.Core.Domain.Settings

open Domain.BreadthFirst
let initSettings = {
  Diagonal = false
  Width = 10
  Height = 10
  Cost = false
  Position = false
  Arrow = false }
let initStart = {x=0;y=0}
let initTarget = {x=initSettings.Width-1;y=initSettings.Height-1}
let initRandomGrid state: Grid =
  let rand = Random()
  let grid =
    [ for x in [0..state.settings.Width-1] do
      for y in [0..state.settings.Height-1] do
        let terrain =
          match rand.Next(0, 3) with
          | 0 -> Blocked
          | _ -> Walkable
        ({x=x;y=y}, terrain) ]
    |> Map.ofList
  grid
    .Add(state.getStart, Start)
    .Add(state.getTarget, Target)
let initGrid settings: Grid =
  let grid =
    [ for x in [0..settings.Width-1] do
      for y in [0..settings.Height-1] do 
        ({x=x;y=y}, Walkable) ]
    |> Map.ofList
  grid
    .Add(initStart, Start)
    .Add({x=settings.Width-1;y=settings.Height-1}, Target)
let initData: Domain.BreadthFirst.Data = {
  ClosedNodes = []
  OpenNodes = [createNode None 0 initStart] }
let initState(): State = {
    Grid = initGrid initSettings
    Solutions = []
    BreadthFirstData = initData
    View = GridView
    settings = initSettings }
