module OVIDConnect

open FSharp.Data
open FSharp.Data
open System
open System.Text
open System.Net
open System.IO
open Chessie
open Chessie.ErrorHandling


let databaseListUrl = "https://ovidsp.tx.ovid.com/ovidws/databases"

let url = "https://ovidsp.tx.ovid.com/ovidws/databases/code/amed"

let headers =["Accept", "text/html";"Authorization",  "[enter basic here"]

let res = Http.AsyncRequest (databaseListUrl, silentHttpErrors = true, headers = headers)

let r = Async.StartChild (res,millisecondsTimeout = 60000 )
        |> Async.RunSynchronously
        |> Async.RunSynchronously


let body = r.Body
let cookies = r.Cookies

let sessionCookie = cookies.Item("Session")



let req = HttpWebRequest.Create(url) :?> HttpWebRequest

req.Method <- "POST"

let q = System.Net.WebUtility.UrlEncode("monkey.ab")

let postBytes = Encoding.ASCII.GetBytes("query=monkey.ab&type=nlp&lifespan=session")

req.ContentType <- "application/x-www-form-urlencoded"
req.ContentLength <- int64 postBytes.Length

try
  let c = new Cookie("Session",sessionCookie,"/","ovidsp.tx.ovid.com")

  req.CookieContainer <- new CookieContainer()

  req.CookieContainer.Add(c)          
    
  let container = req.CookieContainer

  let reqStream = req.GetRequestStream()
  reqStream.Write(postBytes, 0, postBytes.Length);
  reqStream.Close()


        
with
    | exn ->
        printfn "Exception message: %s" exn.Message



let resp = req.GetResponse()
let stream = resp.GetResponseStream()
let reader = new StreamReader(stream)
let html = reader.ReadToEnd()

