module SolidHex.Demo

open SolidHex
open Fable.Core
open Browser.Dom
open Browser.Types

let canvas =
  document.getElementById "root"
  :?> HTMLCanvasElement

let mutable mousePosition = 0.0, 0.0

let mutable selected = Set.empty<_>

let layout =
  {
    Orientation = Hex.flatOrientation
    Size = 32.0, 32.0
    Origin = 0.0, 0.0
  }

canvas.onmousemove <- fun e ->
  let bounds = canvas.getBoundingClientRect ()

  mousePosition <-
    e.clientX - bounds.left,
    e.clientY - bounds.top

  ()

canvas.onmousedown <- fun e ->
  let h = Hex.toHex layout mousePosition

  selected <-
    if selected |> Set.contains h
    then
      selected |> Set.remove h
    else
      selected |> Set.add h

  ()

let ctx =
  canvas.getContext_2d ()

let drawHex hex (ctx : CanvasRenderingContext2D) =
  ctx.beginPath ()
  ctx.moveTo <| (Hex.pixelCorners layout hex |> Seq.head)

  for (x, y) in Hex.pixelCorners layout hex do
    ctx.lineTo (x, y)

  ctx.closePath ()
  ctx.fill ()

let rec loop previousTime time =

  ctx.fillStyle <- U3.Case1 "#004643"
  ctx.fillRect (0.0, 0.0, canvas.width, canvas.height)

  // Draw edge
  ctx.fillStyle <- U3.Case1 "#abd1c6"

  let fringe =
    selected
    |> Seq.collect (fun h ->
      Hex.directions
      |> Seq.map ((Hex.direction 1) >> (+) h))
    |> Set.ofSeq

  for h in fringe do
    ctx |> drawHex h

  // Draw selected
  ctx.fillStyle <- U3.Case1 "#e8e4e6"

  for h in selected do
    ctx |> drawHex h

  // Draw hex cursor
  let h = Hex.toHex layout mousePosition

  ctx.fillStyle <- U3.Case1 "#f9bc60"
  ctx |> drawHex h

  // Loop
  window.requestAnimationFrame (loop time)
  |> ignore

window.requestAnimationFrame (fun t ->
  loop t t
)
|> ignore
