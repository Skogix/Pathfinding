module UI.Views.SettingsView

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Pathfinding.Core.Domain.Settings
open Pathfinding.Core.State
open Route.Domain

let view state dispatch =
  StackPanel.create [
    StackPanel.children [
      Button.create [
        Button.content "Toggle Diagonal"
        Button.height 50.
        Button.onClick(fun _ -> dispatch (Input.ChangeSetting ChangeSettingsCommand.Diagonal))
      ]
      Button.create [
        Button.content "Toggle Arrows"
        Button.height 50.
        Button.onClick(fun _ -> dispatch (Input.ChangeSetting ChangeSettingsCommand.Arrow))
      ]
      Button.create [
        Button.content "Toggle Cost"
        Button.height 50.
        Button.onClick(fun _ -> dispatch (Input.ChangeSetting ChangeSettingsCommand.Cost))
      ]
      Button.create [
        Button.content "Toggle Position"
        Button.height 50.
        Button.onClick(fun _ -> dispatch (Input.ChangeSetting ChangeSettingsCommand.Position))
      ]
      TextBlock.create [
        TextBlock.text $"{state.settings}"
      ]
      StackPanel.create [
        StackPanel.orientation Orientation.Horizontal
        StackPanel.children [
          Button.create [
            Button.content "Height +"
            Button.height 50.
            Button.onClick(fun _ -> dispatch (Input.ChangeSetting (ChangeSettingsCommand.Height Increment)))
          ]
          Button.create [
            Button.content "Height -"
            Button.height 50.
            Button.onClick(fun _ -> dispatch (Input.ChangeSetting (ChangeSettingsCommand.Height Decrement)))
          ]
        ]
      ]
      StackPanel.create [
        StackPanel.orientation Orientation.Horizontal
        StackPanel.children [
          Button.create [
            Button.content "Width +"
            Button.height 50.
            Button.onClick(fun _ -> dispatch (Input.ChangeSetting (ChangeSettingsCommand.Width Increment)))
          ]
          Button.create [
            Button.content "Width -"
            Button.height 50.
            Button.onClick(fun _ -> dispatch (Input.ChangeSetting (ChangeSettingsCommand.Width Decrement)))
          ]
        ]
      ]
    ]
  ]