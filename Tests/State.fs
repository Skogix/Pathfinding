module Tests.State
open Pathfinding.Core.Domain.BreadthFirst
open Pathfinding.Core.Domain.Grid
open Pathfinding.Core.Domain.Settings
open Pathfinding.Core.State

///
/// separate state for testing purposes
/// 

let initStart = {x=0;y=0}
let initSettings: Settings = {
  Diagonal = false
  Cost = false 
  Arrow =  false
  Position =  false
  Width = 2
  Height = 2 }
let initWidth = 2
let initHeight = 2
let initStartNode = ({x=0;y=0}, Start)
let initBlockedNode = ({x=1;y=0}, Blocked)
let initWalkableNode = ({x=0;y=1}, Walkable)
let initTargetNode = ({x=initWidth-1;y=initHeight-1}, Target)
let initGrid: Grid =
  let grid =
    [ for x in [0..initWidth-1] do
      for y in [0..initHeight-1] do 
        ({x=x;y=y}, Walkable) ]
    |> Map.ofList
  grid
    .Add(initStartNode)
    .Add(initWalkableNode)
    .Add(initBlockedNode)
    .Add(initTargetNode)
let initState: State =  {
  Grid = initGrid
  Solutions = []
  BreadthFirstData = Pathfinding.Core.Init.initData
  View = GridView
  settings = initSettings }
let newClosedNodes = Pathfinding.BreadthFirst.run initState.BreadthFirstData initState
let newData = {initState.BreadthFirstData with ClosedNodes = newClosedNodes}
let state = {initState with BreadthFirstData = newData}