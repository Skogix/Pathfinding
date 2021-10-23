module Tests.Init

open System
open Pathfinding.Core
open Route
open UI
open Common
///
/// basic tests
/// 
module InitTests =
    open Expecto

    let state = Tests.State.state
    let rand() = Random()
    let randPos =
        let x = rand().Next(1, state.settings.width-1)
        let y = rand().Next(1, state.settings.height-1)
        createPosition x y
    [<Tests>]
    let tests =
        testList "basic tests"
            [
              test "init grid" {
                  let actual = Pathfinding.Core.Init.initGrid state.settings
                  let expected = state.settings.width * state.settings.height
                  Expect.equal actual.Count expected $"should be {expected}"
              }
              test "toggle terrain input" {
                  let prePos, preTerrain = State.initWalkableNode
                  let post, _ = Update.update (Domain.ToggleTerrain prePos) state
                  Expect.notEqual preTerrain post.grid.[prePos] "should be different"
              }
              test "run once breadth first" {
                  ()
              }
            ]
