module Program
open System
open System.Configuration
open FSharp.Configuration
open Chessie
open TurleRobotParser


[<EntryPoint>]
let main argv = 
    printfn "Hi"

    let x = TurleRobotParser.answer



    Console.ReadLine() |> ignore
    0 