module UI.Views.GridView

open Avalonia.Controls.Primitives
open Pathfinding.Core.Domain.BreadthFirst
open Pathfinding.Core.Domain.Grid
open Pathfinding.Core.State
open Pathfinding.Core.Common
open Route.Domain
open Avalonia.Controls
open Avalonia.Layout
open Pathfinding.Core.Domain.Settings
open Avalonia.FuncUI.DSL
open SLibrary.RailWay
/// sets the background for a uniformgrid node
let background pos (state: State) =
    let isInsideGrid pos =
        match state.grid.ContainsKey(pos) with
        | false -> failwith "position is outside the grid"
        | true -> Data pos

    let startOrTarget pos =
        match state.getStart = pos, state.getTarget = pos with
        | true, _ -> Result "pink"
        | _, true -> Result "magenta"
        | _, _ -> Data pos

    let solution pos =
        match state.solutionsContainsPos pos with
        | true -> Result "orange"
        | false -> Data pos

    let openClosedNodes pos =
        let isClosed =
            state.closedNodesPositionList
            |> List.contains (pos)

        let isOpen =
            state.openNodesPositionList |> List.contains (pos)

        match isOpen, isClosed with
        | true, _ -> Result "blue"
        | _, true -> Result "gray"
        | _, _ -> Data pos

    let restOfTerrain pos =
        match state.grid.TryFind(pos) with
        | Some terrain ->
            match terrain with
            | Blocked -> Result "black"
            | Walkable -> Result "green"
            | _ -> Data pos
        | None -> Data pos

    (Data pos)
    |>> isInsideGrid
    |>> startOrTarget
    |>> solution
    |>> openClosedNodes
    |>> restOfTerrain
    |<< "purple"

let arrow (pos: Position) (state: State) =
    let getParent (node: Node) = node.parent

    let applyDefault (def: string) (str: string option) =
        match str with
        | Some s -> s
        | None -> def

    let getArrow parent =
        match (parent.x - pos.x), (parent.y - pos.y) with
        | 1, 0 -> Some "→"
        | 0, 1 -> Some "↓"
        | -1, 0 -> Some "←"
        | 0, -1 -> Some "↑"
        | 1, 1 -> Some "↘"
        | -1, -1 -> Some "↖"
        | 1, -1 -> Some "↗"
        | -1, 1 -> Some "↙"
        | _, _ -> None

    match state.settings.arrow with
    | false -> ""
    | true ->
        match state.breadthFirstData.closedNodes
              |> List.tryFind (fun x -> x.position = pos) with
        | Some node ->
            node
            |> getParent
            |> Option.bind getArrow
            |> applyDefault "?"
        | None -> " "

let position (pos: Position) (state: State) =
    match state.settings.position with
    | false -> " "
    | true -> $"Pos: {pos.x}, {pos.y}"

let cost (pos: Position) (state: State) =
    match state.settings.cost with
    | false -> " "
    | true ->
        match state.breadthFirstData.closedNodes
              |> List.tryFind (fun x -> x.position = pos) with
        | Some node -> $"Cost: {node.cost}"
        | None -> ""
/// creates the grid
let view (state: Output) dispatch =
    let settings = state.settings

    UniformGrid.create [ UniformGrid.rows settings.height
                         UniformGrid.columns settings.width
                         UniformGrid.dock Dock.Top
                         UniformGrid.children [ for y in [ 0 .. settings.height - 1 ] do
                                                    for x in [ 0 .. settings.width - 1 ] do
                                                        let pos = createPosition x y

                                                        StackPanel.create [ StackPanel.background (background pos state)
                                                                            StackPanel.margin 2.
                                                                            StackPanel.onTapped
                                                                                (fun _ ->
                                                                                    dispatch (Input.ToggleTerrain pos))
                                                                            StackPanel.children [ TextBlock.create [ TextBlock.text
                                                                                                                         $"{position pos state}"
                                                                                                                     TextBlock.horizontalAlignment
                                                                                                                         HorizontalAlignment.Left ]
                                                                                                  TextBlock.create [ TextBlock.text
                                                                                                                         $"{cost pos state}"
                                                                                                                     TextBlock.horizontalAlignment
                                                                                                                         HorizontalAlignment.Left ]
                                                                                                  TextBlock.create [ TextBlock.text
                                                                                                                         $"{arrow pos state}"
                                                                                                                     TextBlock.fontSize
                                                                                                                         40.
                                                                                                                     TextBlock.horizontalAlignment
                                                                                                                         HorizontalAlignment.Center
                                                                                                                     TextBlock.verticalAlignment
                                                                                                                         VerticalAlignment.Center ] ] ] ] ]
