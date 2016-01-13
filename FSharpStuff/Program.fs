module Program
open System
open System.Configuration
open FSharp.Configuration
open Chessie
open TurleRobotParser
//open FParsecTutorial
//open JsonParser

open Catamorphism

[<EntryPoint>]
let main argv = 
    printfn "Hi"

    let x = TurleRobotParser.answer

    printfn "%A" x

    Console.ReadLine() |> ignore
    0 