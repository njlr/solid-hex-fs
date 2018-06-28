module Hex

open System

type Hex = 
  struct
    val Q : float
    val R : float

    new(q: float, r: float) = { Q = q; R = r }

    member this.S = 0.0 - this.Q - this.R

    override this.ToString () = "{" + (string this.Q) + ", " + (string this.R) + "}"
  end

let zero = new Hex(0.0, 0.0)

let add (a : Hex) (b : Hex) = 
  new Hex(a.Q + b.Q, a.R + b.R)

let subtract (a : Hex) (b : Hex) = 
  new Hex(a.Q - b.Q, a.R - b.R)

let scale (a : Hex) (k : float) = 
  new Hex(a.Q * k, a.R * k)

let rotateLeft (a : Hex) = new Hex(-a.S, -a.Q)

let rotateRight (a : Hex) = new Hex(-a.R, -a.S)

type Direction = 
| NorthEast
| East
| SouthEast
| SouthWest
| West
| NorthWest

let direction (direction : Direction) = 
  match direction with 
  | NorthEast -> new Hex(1.0, -1.0)
  | East -> new Hex(1.0, 0.0)
  | SouthEast -> new Hex(0.0, 1.0)
  | SouthWest -> new Hex(-1.0, 1.0)
  | West -> new Hex(-1.0, 0.0)
  | NorthWest -> new Hex(0.0, -1.0)

let neighbor (d : Direction) (h : Hex) = 
  h |> add(direction(d))

let length (h : Hex) = 
  (Math.Abs(h.Q) + Math.Abs(h.R) + Math.Abs(h.S)) / 2.0
