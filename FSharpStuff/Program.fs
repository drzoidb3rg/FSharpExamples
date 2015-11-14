module Program
open System
open System.Configuration
open FSharp.Configuration


open HttpWorkflow

[<EntryPoint>]
let main argv = 
    printfn "Hi"

   
    let result = HttpWorkflow.answer

    printfn "%A" result

    Console.ReadLine() |> ignore
    0 