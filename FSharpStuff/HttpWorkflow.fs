module HttpWorkflow

open Chessie
open Chessie.ErrorHandling
open FSharp.Data

type NugetStats = HtmlProvider<"https://www.nuget.org/packages/FSharp.Data">

type Error =
      | RequestValidationError of string
      | HttpResponseError of string
      | HttpException of string
      | ValidationError of string
      | GenericError of string

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

           return! ok(r)

         with
           | e -> 
                //return! fail((sprintf "Http request failed %s" (e.Message)) )
                return! fail(( HttpException (sprintf "Http request failed %s" (e.Message))) )
    }



let validateResoponseStatusCode x =
  match x.StatusCode with
   | 200 -> ok x
   | _ -> fail(HttpResponseError (sprintf "Invaid status code %A %A" x.StatusCode x.Body))

let getResponseString x = 
  match x.Body with
    | Text text -> ok text
    | _ -> fail (RequestValidationError "no body text")


let validate1 url =
  match url with
    | "" -> fail (RequestValidationError "url is empty")
    | _ -> ok url

let data html  = NugetStats.Parse html



let combinedValidation = 
    // connect the two-tracks together
    validate1
    //>> bind validate2
    //>> bind validate3



let simpleWorkflow url =
  url |> combinedValidation
      >>= getSafeHttpStringResponse
      >>= validateResoponseStatusCode
      >>= getResponseString
      |> lift data
      |> lift ResultSet.from












