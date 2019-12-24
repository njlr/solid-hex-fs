namespace SolidHex

[<Struct>]
type Hex<'T> =
  | Hex of 'T * 'T
  with
    static member inline (+) (Hex (q1, r1), Hex (q2, r2)) =
      Hex (q1 + q2, r1 + r2)

    static member inline (~+) (h : Hex<_>) =
      h

    static member inline (~-) (Hex (q, r)) =
      Hex (-q, -r)

    static member inline (-) (Hex (q1, r1), Hex (q2, r2)) =
      Hex (q1 - q2, r1 - r2)

    static member inline (*) (Hex (q, r), k) =
      Hex (q * k, r * k)

    static member inline (*) (k, Hex (q, r)) =
      Hex (q * k, r * k)

type HexI = Hex<int>

type HexF = Hex<float>

type Orientation =
  {
    F0 : float
    F1 : float
    F2 : float
    F3 : float
    B0 : float
    B1 : float
    B2 : float
    B3 : float
    StartAngle : float
  }

type Layout =
  {
    Orientation : Orientation
    Size : float * float
    Origin : float * float
  }

module Hex =

  let inline s (Hex (q, r) : Hex<_>) =
    LanguagePrimitives.GenericZero - q - r

  let inline hex q r =
    Hex (q, r)

  let inline toPixel layout (Hex (q, r)) =
    let m = layout.Orientation
    let size = layout.Size
    let origin = layout.Origin
    let x = (m.F0 * (float q) + m.F1 * (float r)) * (fst size)
    let y = (m.F2 * (float q) + m.F3 * (float r)) * (snd size)
    (x + (fst origin), y + (snd origin))

  let flatOrientation =
    {
      F0 = 3.0 / 2.0
      F1 = 0.0
      F2 = (sqrt 3.0) / 2.0
      F3 = sqrt 3.0
      B0 = 2.0 / 3.0
      B1 = 0.0
      B2 = -1.0 / 3.0
      B3 = (sqrt 3.0) / 3.0
      StartAngle = 0.0
    }

  open System

  let inline private cornerOffset layout corner =
    let m = layout.Orientation
    let (x, y) = layout.Size
    let angle = 2.0 * Math.PI * (m.StartAngle - (float corner)) / 6.0
    (x * (cos angle), y * (sin angle))

  let inline pixelCorners layout hex =
    let (cx, cy) = toPixel layout hex
    [0..6]
    |> Seq.map (fun i ->
      let (ox, oy) = cornerOffset layout i
      (cx + ox, cy + oy)
    )
    |> Seq.toList

  let inline toHex layout (x, y) =
    let m = layout.Orientation;
    let (sx, sy) = layout.Size;
    let (ox, oy) = layout.Origin;
    let (ptx, pty) = ((x - ox) / sx, (y - oy) / sy)
    let q = m.B0 * ptx + m.B1 * pty;
    let r = m.B2 * ptx + m.B3 * pty;
    hex (int (round q)) (int (round r))

  let inline zero () =
    Hex (LanguagePrimitives.GenericZero, LanguagePrimitives.GenericZero)

  let inline rotateLeft (Hex (q, r) : Hex<_>) : Hex<_> =
    hex (q + r) -q

  let inline rotateRight (Hex (q, r) : Hex<_>) : Hex<_> =
    hex -r (q + r)

  type Direction =
  | NorthEast
  | East
  | SouthEast
  | SouthWest
  | West
  | NorthWest

  let directions =
    [
      NorthEast
      East
      SouthEast
      SouthWest
      West
      NorthWest
    ]

  let inline direction magnitude (direction : Direction) =
    let zero = LanguagePrimitives.GenericZero
    let one = magnitude
    let minusOne = -magnitude

    match direction with
    | NorthEast -> hex one minusOne
    | East -> hex one zero
    | SouthEast -> hex zero one
    | SouthWest -> hex minusOne one
    | West -> hex minusOne zero
    | NorthWest -> hex zero minusOne

  let inline length (Hex (q, r) : _) =
    let zero = LanguagePrimitives.GenericZero
    let one = LanguagePrimitives.GenericOne

    (abs q + abs r + abs (zero - q- r)) / (one + one)

  let inline round (Hex (q, r) : _) =
    hex (round q) (round r)

  let inline abs (Hex (q, r) : _) =
    hex (abs q) (abs r)

  let inline toHexI (Hex (q, r)) =
    Hex (int q, int r)

  let inline toHexF (Hex (q, r)) =
    Hex (float q, float r)
