module HttpWorkflow

open Chessie
open Chessie.ErrorHandling
open FSharp.Data

type NugetStats = HtmlProvider<"https://www.nuget.org/packages/FSharp.Data">

type Diagnostic =
      | RequestValidationError of string
      | HttpResponseError of string
      | HttpException of string
      | ValidationError of string
      | GenericError of string
      | Message of string


type ResultSet =
  {
    FirstVersion : string
    ErrorMessage : Diagnostic list option
  }
  static member from (x:NugetStats) = 
    {FirstVersion =  x.Tables.``Version History``.Rows.[0].Version; ErrorMessage = None}


let urlData = "https://www.nuget.org/packages/FSharp.Data"
let urlCore = "https://www.nuget.org/packages/FSharp.Core"


let getHttpStringResponse url = Http.RequestString( url, httpMethod = "GET", headers = [ "Accept", "text/xml" ]) 



let getSafeHttpStringResponse uri =
    trial {
        
         try
           let req = Http.AsyncRequest (uri ,silentHttpErrors = true)

           let timer = new System.Diagnostics.Stopwatch()
           timer.Start()

           let r = Async.StartChild (req,millisecondsTimeout = 60000 ) 
                   |> Async.RunSynchronously 
                   |> Async.RunSynchronously


           let m = sprintf "%s : %i seconds" uri timer.ElapsedMilliseconds |> Message
           
           return! warn m r
         with
           | e -> 
                return! fail(sprintf "Http request failed %s" (e.Message) |> HttpException)
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

let validate2 url =
  match url with
    | "" -> fail (RequestValidationError "url is empty again")
    | _ -> ok url

let parseData html  = 
  trial {
      try

         let r = NugetStats.Parse html  
         let m = "some message" |> Message
         return! warn m r
      with
           | e -> 
                return! fail(sprintf "Http request failed %s" (e.Message) |> HttpException)
  }




let combinedValidation = 
    // connect the two-tracks together
    validate1
    >> bind validate2
    //>> bind validate3

let log x = 
    let success(x,msgs) = printf "Debug. %A" msgs
    let failure msgs = printf "ERROR. %A" msgs
    eitherTee success failure x 


let temp x =
  let version = x.FirstVersion
  x


let errorResult x =
 {FirstVersion = "-1"; ErrorMessage = Some x}

let output x = 
    match x with 
      | Bad(y) -> errorResult y
      | Ok(result,msgs) -> result
    


let simpleWorkflow url =
  url |> combinedValidation
      >>= getSafeHttpStringResponse
      >>= validateResoponseStatusCode
      >>= getResponseString
      >>= parseData
      |> lift ResultSet.from
      |> output


     











