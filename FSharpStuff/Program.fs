module Program
open System
open System.Configuration
open FSharp.Configuration
open Chessie

open HttpWorkflow
open Strings

[<EntryPoint>]
let main argv = 
    printfn "Hi"

    let x = Strings.temp "monkey"
//    let goodResult = urlData |> simpleWorkflow
//    
//    let badResult1 = "" |> simpleWorkflow
//
//    //let badResult2 = "abc" |> simpleWorkflow
//
//    //let timeout = "http://httpstat.us/504" |> simpleWorkflow
//
//    printfn "%s"Environment.NewLine
//    printfn "%s"Environment.NewLine
//
//    printfn "Good %A" goodResult
//
//    printfn "Bad %A" badResult1

   // printfn "%A" timeout

    Console.ReadLine() |> ignore
    0 