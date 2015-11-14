module Program
open System
open System.Configuration
open FSharp.Configuration
//open DataStructures
//open Curry
//open Operators
//open Play
//open PatternMatching
//open Bind


open Railway

[<EntryPoint>]
let main argv = 
    printfn "Hi"

   
    Railway.input1

    Console.ReadLine() |> ignore
    0 