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
  diagonal = false
  cost = false 
  arrow =  false
  position =  false
  width = 2
  height = 2
}
let initWidth = 2
let initHeight = 2

let initStartNode = ({x=0;y=0}, Start)
let initBlockedNode = ({x=1;y=0}, Blocked)
let initWalkableNode = ({x=0;y=1}, Walkable)
let initTargetNode = ({x=initWidth-1;y=initHeight-1}, Target)
let initGrid: Grid =
  let grid =
    [
      for x in [0..initWidth-1] do
        for y in [0..initHeight-1] do 
          ({x=x;y=y}, Walkable)
    ]
    |> Map.ofList
  grid
    .Add(initStartNode)
    .Add(initWalkableNode)
    .Add(initBlockedNode)
    .Add(initTargetNode)
let initState: State =  {
  grid = initGrid
  solutions = []
  breadthFirstData = Pathfinding.Core.Init.initData
  view = GridView
  settings = initSettings
}
let newClosedNodes = Pathfinding.BreadthFirst.run initState.breadthFirstData initState
let newData = {initState.breadthFirstData with closedNodes = newClosedNodes}
let state = {initState with breadthFirstData = newData}
