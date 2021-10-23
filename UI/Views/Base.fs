module UI.Views.Base

open Avalonia.Controls
open Pathfinding.Core.State
open Pathfinding.Core
open Pathfinding.Core.Domain
open Route.Domain
open Avalonia.FuncUI.DSL

/// creates the buttons at the bottom
let createButtons state dispatch =
  StackPanel.create [
    StackPanel.dock Dock.Bottom
    StackPanel.children [
      Button.create [
        Button.dock Dock.Bottom
        Button.height 50.
        Button.isVisible (state.view = GridView)
        Button.content "SettingsView"
        Button.onClick (fun _ -> dispatch (Input.ChangeView State.View.SettingsView))
      ]
      Button.create [
        Button.dock Dock.Bottom
        Button.height 50.
        Button.isVisible (state.view = SettingsView)
        Button.content "GridView"
        Button.onClick (fun _ -> dispatch (Input.ChangeView State.View.GridView))
      ]
      match Settings.runTimer with
      | false -> 
        Button.create [
          Button.dock Dock.Bottom
          Button.height 50.
          Button.content "Reset"
          Button.onClick (fun _ -> dispatch Input.Reset)
        ]
        Button.create [
          Button.dock Dock.Bottom
          Button.height 50.
          Button.content "Random Terrain"
          Button.onClick (fun _ -> dispatch Input.RandomTerrain)
        ]
        Button.create [
          Button.dock Dock.Bottom
          Button.height 50.
          Button.content "Run"
          Button.onClick (fun _ -> dispatch Input.RunBreadthFirst)
        ]
        Button.create [
          Button.dock Dock.Bottom
          Button.height 50.
          Button.content "Run Once"
          Button.onClick (fun _ -> dispatch Input.RunBreadthFirstOnce)
        ]
        Button.create [
          Button.dock Dock.Bottom
          Button.height 50.
          Button.content "Turn on Autorun/Debug"
          Button.onClick (fun _ -> dispatch Input.ToggleRunTimer)
        ]
      | true -> 
        Button.create [
          Button.dock Dock.Bottom
          Button.height 50.
          Button.content "Random Terrain"
          Button.onClick (fun _ -> dispatch Input.RandomTerrain)
        ]
        Button.create [
          Button.dock Dock.Bottom
          Button.height 50.
          Button.content "Turn off Autorun/Debug"
          Button.onClick (fun _ -> dispatch Input.ToggleRunTimer)
        ]
    ]
  ]


///
/// the main view controller
/// input: the current state and a back-channel/dispatch to respond/send commands
/// output: avalonia iview
///
let view (state:Output) dispatch =
  DockPanel.create [
    DockPanel.children [
      createButtons state dispatch
      match state.view with
      | SettingsView ->
        SettingsView.view state dispatch
      | GridView ->
        GridView.view state dispatch
    ]
  ]