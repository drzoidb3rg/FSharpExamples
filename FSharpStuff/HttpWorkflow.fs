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
    >> bind validate2


let simpleWorkflow url =
  url |> combinedValidation
      |> lift getHttpStringResponse
      |> lift data
      |> lift ResultSet.from



//not sure what trial is, use this for error handling ? how does this fit in with my workflow ???
let doTrial url =
        trial {
          let! a = validate1 url
          return {FirstVersion = "ok"}
        }










