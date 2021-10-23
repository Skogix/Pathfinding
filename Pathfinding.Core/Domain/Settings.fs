module Pathfinding.Core.Domain.Settings


///
/// Settings
///
let mutable runTimer = true
type GridSizeChange = Increment | Decrement
type ChangeSettingsCommand =
  | Diagonal
  | Position
  | Cost
  | Arrow
  | RunTimer
  | Width of GridSizeChange
  | Height of GridSizeChange
type Settings = {
  diagonal: bool
  cost: bool
  arrow: bool
  position: bool
  width: int
  height: int
}
let updateSettings state command =
  let gridSizeChange (command:GridSizeChange) int =
    match command with
    | Increment -> int + 1
    | Decrement -> int - 1
  match command with
  | Diagonal -> {state with diagonal = not state.diagonal}
  | Cost -> {state with cost = not state.cost}
  | Arrow -> {state with arrow = not state.arrow}
  | Position -> {state with position = not state.position}
  | Width cmd -> {state with width = (gridSizeChange cmd state.width) }
  | Height cmd -> {state with height = (gridSizeChange cmd state.height) }
  | _ -> state