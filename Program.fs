open System
open Hex

[<EntryPoint>]
let main argv =
  Console.WriteLine (new Hex(1.0, 2.0))
  0 // return an integer exit code
