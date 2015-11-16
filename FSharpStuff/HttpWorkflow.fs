module HttpWorkflow

open Chessie
open Chessie.ErrorHandling
open FSharp.Data

type NugetStats = HtmlProvider<"https://www.nuget.org/packages/FSharp.Data">


type ResultSet =
  {
    FirstVersion : string
  }
  static member from (x:NugetStats) = 
    {FirstVersion =  x.Tables.``Version History``.Rows.[0].Version}


let urlData = "https://www.nuget.org/packages/FSharp.Data"
let urlCore = "https://www.nuget.org/packages/FSharp.Core"


let getHttpStringResponse url = Http.RequestString( url, httpMethod = "GET", headers = [ "Accept", "text/xml" ]) 

let getSafeHttpStringResponse uri =
    trial {
        
         try
           let req = Http.AsyncRequest (uri ,silentHttpErrors = true)

           let r = Async.StartChild (req,millisecondsTimeout = 60000 ) 
                   |> Async.RunSynchronously 
                   |> Async.RunSynchronously
           return r
         with
           | e -> 
                return! fail((sprintf "Http request failed %s" (e.Message)) )
    }



let validateResoponseStatusCode x =
  match x.StatusCode with
   | 200 -> ok x
   | _ -> fail((sprintf "Invaid status code %A %A" x.StatusCode x.Body))

let getResponseString x = 
  match x.Body with
    | Text text -> ok text
    | _ -> fail "no body text"

let data html  = NugetStats.Parse html


let validate1 url =
  match url with
    | "" -> fail "url is empty"
    | _ -> ok url

let validate2 url =
  match url with
    | "https://www.nuget.org/packages/FSharp.Data" -> ok url
    | "https://www.nuget.org/packages/FSharp.Core" -> ok url
    | _ -> fail "url is not one I was expecting"



let combinedValidation = 
    // connect the two-tracks together
    validate1
   // >> bind validate2


let simpleWorkflow url =
  url |> combinedValidation
      >>= getSafeHttpStringResponse
      >>= validateResoponseStatusCode
      >>= getResponseString
      |> lift data
      |> lift ResultSet.from












