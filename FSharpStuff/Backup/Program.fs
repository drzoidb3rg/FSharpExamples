﻿module Program
open System
open System.Configuration
open FSharp.Configuration
//open DataStructures
//open Curry
//open Operators
//open Play
//open PatternMatching
//open Bind


open Html

[<EntryPoint>]
let main argv = 
    printfn "Hi"

   

    let authors =EBSCOProvider.getAuthors

    Console.ReadLine() |> ignore
    0 