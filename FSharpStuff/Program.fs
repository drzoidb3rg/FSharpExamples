module Program
open System
open System.Configuration
open FSharp.Configuration
open Chessie

open OVIDConnect


[<EntryPoint>]
let main argv = 
    printfn "Hi"

    let x = OVIDConnect.html


    Console.ReadLine() |> ignore
    0 