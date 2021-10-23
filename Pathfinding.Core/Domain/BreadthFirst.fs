module Pathfinding.Core.Domain.BreadthFirst

open Pathfinding.Core.Domain.Grid

type Node = {
  position: Position
  parent: Position option
  cost: int
}
let createNode parent cost pos = {
  position = pos
  parent = parent
  cost = cost
}
type OpenNodes = Node list
type ClosedNodes = Node list
type Data = {
  closedNodes: ClosedNodes
  openNodes: OpenNodes
}
