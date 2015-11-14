module Http

open FSharp.Data


type HttpResponseWrapper =
  {
    ErrorMessage : string option
    StatusCode : int
    Response : HttpResponse option
  }
  with
    static member empty  =
       {ErrorMessage = None; Response = None; StatusCode = 0}
    static member from(x:HttpResponse) =
      let unhappy m = {ErrorMessage = Some m; Response = None; StatusCode = x.StatusCode}
      match x.StatusCode with 
        | 200 -> {ErrorMessage = None; Response = Some x; StatusCode = x.StatusCode}
        | 404 -> unhappy "Not found"
        | 500 -> unhappy "Server error"
        | _ -> unhappy "Unknown error"
     static member simpleRequest url =
           async {
                  let r = Http.AsyncRequest( url, httpMethod = "GET",headers = [ "Accept", "text/html" ],silentHttpErrors = true)
                  try
                      let! resp = Async.StartChild(r, millisecondsTimeout = 60000)
                                  |> Async.RunSynchronously

                      return HttpResponseWrapper.from(resp)
                  with
                  |  e -> return {HttpResponseWrapper.empty with ErrorMessage = Some e.Message}
                 }

let simpleRequest u =  Http.RequestString( u, httpMethod = "GET",headers = [ "Accept", "text/html" ])


let url = "http://nice.org.uk"
let badurl = "http://nice.org.uk/xx"


let response = url |> HttpResponseWrapper.simpleRequest |> Async.RunSynchronously

let badResponse = badurl |> HttpResponseWrapper.simpleRequest |> Async.RunSynchronously



