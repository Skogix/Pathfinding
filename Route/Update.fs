module Route.Update

open Route.Domain
open Pathfinding.Core
open Pathfinding.Core.Domain.BreadthFirst
open Pathfinding.Core.Domain.Grid
open Pathfinding.Core.Domain.Settings
open Pathfinding.Core.State
///
/// MVU-version of a viewmodel, right now we only send back a new state but can return a tuple of state and a function/command.
/// 
let update (command:Input) (state:Output) =
  // todo; separate into modules/smaller functions, this will get messy if we have more than 5+ modules
  match command with
  | ChangeView view -> {state with view = view}, None
  | ChangeSetting command ->
    {state with settings = (updateSettings state.settings command)}, Some Reset
  | ToggleTerrain pos ->
    let toggle =
      match state.grid.[pos] with
      | Walkable -> Blocked
      | Blocked -> Walkable
      | x -> x
    {state with grid = state.grid.Add(pos, toggle)}, None
  | RandomTerrain ->
    let newGrid = Init.initRandomGrid state
    {state with grid = newGrid}, None
  | Reset ->
    {state with breadthFirstData = Init.initData;solutions = [];grid = Init.initGrid state.settings}, None
  | RunBreadthFirst ->
    let closedNodes = Pathfinding.BreadthFirst.run Init.initData state
    let newData = {state.breadthFirstData with closedNodes = closedNodes}
    let newSolutions = Pathfinding.BreadthFirst.getSolutions state.getStart state.getTarget closedNodes
    {state with breadthFirstData = newData;solutions = newSolutions}, None
  | RunBreadthFirstOnce ->
    let openNodes, closedNodes = Pathfinding.BreadthFirst.runOnce state.breadthFirstData.openNodes state.breadthFirstData.closedNodes state
    {state with breadthFirstData = { openNodes = openNodes;closedNodes = closedNodes }}, None
  | ToggleRunTimer ->
    runTimer <- not runTimer
    state, Some Reset
