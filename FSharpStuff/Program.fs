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
open Maybe

[<EntryPoint>]
let main argv = 
    printfn "Hi"

   

    let x = Maybe.foobar 0

    Console.ReadLine() |> ignore
    0 