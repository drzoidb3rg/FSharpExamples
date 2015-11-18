module Program
open System
open System.Configuration
open FSharp.Configuration
open Chessie

open HttpWorkflow

[<EntryPoint>]
let main argv = 
    printfn "Hi"

   
    let goodResult = urlData |> simpleWorkflow

    //let ret = goodResult.

    
    //let badResult1 = "" |> simpleWorkflow

    //let badResult2 = "abc" |> simpleWorkflow

    //let timeout = "http://httpstat.us/504" |> simpleWorkflow

    printfn "%s"Environment.NewLine
    printfn "%s"Environment.NewLine

    printfn "%A" goodResult

    //printfn "%A" badResult1

   // printfn "%A" timeout

    Console.ReadLine() |> ignore
    0 