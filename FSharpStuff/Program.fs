module Program
open System
open System.Configuration
open FSharp.Configuration
open Chessie
//open TurleRobotParser
//open FParsecTutorial
open JsonParser

[<EntryPoint>]
let main argv = 
    printfn "Hi"

    let x = JsonParser.x


    Console.ReadLine() |> ignore
    0 