module Pathfinding.Core.Domain.Grid

type Position = {x:int;y:int} 
type Terrain =
  | Walkable
  | Blocked
  | Start
  | Target
type Grid = Map<Position, Terrain>
