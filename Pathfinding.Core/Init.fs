module Pathfinding.Core.Init

open System
open Pathfinding.Core.Domain.Grid
open Pathfinding.Core.State
open Pathfinding.Core.Domain.Settings

open Domain.BreadthFirst
let initSettings = {
  diagonal = false
  width = 10
  height = 10
  cost = false
  position = false
  arrow = false
}
let initStart = {x=0;y=0}
let initTarget = {x=initSettings.width-1;y=initSettings.height-1}
let initRandomGrid state: Grid =
  let rand = Random()
  let grid =
    [
      for x in [0..state.settings.width-1] do
        for y in [0..state.settings.height-1] do
          let terrain =
            match rand.Next(0, 3) with
            | 0 -> Blocked
            | _ -> Walkable
          ({x=x;y=y}, terrain)
    ]
    |> Map.ofList
  grid
    .Add(state.getStart, Start)
    .Add(state.getTarget, Target)
let initGrid settings: Grid =
  let grid =
    [
      for x in [0..settings.width-1] do
        for y in [0..settings.height-1] do 
          ({x=x;y=y}, Walkable)
    ]
    |> Map.ofList
  grid
    .Add(initStart, Start)
    .Add({x=settings.width-1;y=settings.height-1}, Target)
let initData: Domain.BreadthFirst.Data = {
  closedNodes = []
  openNodes = [createNode None 0 initStart]
  }
let initState(): State =
  {
    grid = initGrid initSettings
    solutions = []
    breadthFirstData = initData
    view = GridView
    settings = initSettings
  }
