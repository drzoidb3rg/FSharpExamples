module public WorldBankProvider
open FSharp.Data
open Microsoft.FSharp.Reflection
open System.Reflection

type Simple = JsonProvider<""" { "name":"John", "age":94, "bday":"01 01 2015" } """>
let simple = Simple.GetSample()


simple.Age
simple.Name

let getValue (data:Simple) (fieldName:string) =
  "Sdd" 


type Numbers = JsonProvider<""" [1, 2, 3, 3.14] """>
let nums = Numbers.Parse(""" [1.2, 45.1, 98.2, 5] """)
let total = nums |> Seq.sum


type WorldBank = JsonProvider<""" [ { "page": 1, "pages": 1, "total": 53 },
  [ { "indicator": {"value": "Central government debt, total (% of GDP)"},
      "country": {"id":"CZ","value":"Czech Republic"},
      "value":null,"decimal":"1","date":"2000"},
    { "indicator": {"value": "Central government debt, total (% of GDP)"},
      "country": {"id":"CZ","value":"Czech Republic"},
      "value":"16.6567773464055","decimal":"1","date":"2010"} ] ]""">

let doc = WorldBank.GetSample()


let getCountry (x:WorldBank.Record) = x.Country

let getCountryId (x:WorldBank.Country) = x.Id

let getString (x:string) = x


let recordProperties (x:JsonValue) = 
  match x with
  | JsonValue.Record x ->  Map.ofArray x |> Some
  | _ -> None

let recordValue (x:JsonValue) key =
  match recordProperties(x) with
  | Some x-> x.Item(key) |> Some
  | None -> None

let itemValue (x:JsonValue) =
  match x with 
  | JsonValue.String s -> s |> string
  | _ -> ""

let getGeneric (x:FSharp.Data.Runtime.BaseTypes.IJsonDocument) (key:string) =
 //let temp = JsonValue.Parse(""" { "name":"John", "age":94, "bday":"01 01 2015" } """)

 let value = x.JsonValue

 let prop = recordProperties(value).Value

 let answer = prop.Item(key)

 let str = itemValue(answer)

 str




let docAsync countryCode = WorldBank.Load(sprintf "http://api.worldbank.org/country/%s/indicator/GC.DOD.TOTL.GD.ZS?format=json" countryCode)
//
//
//let info = doc.Record
//printfn "Showing page %d of %d. Total records %d" 
//  info.Page info.Pages info.Total
//
//
let records code = docAsync(code).Array

////let PrintWorldBank =
//// for record in docAsync.Array do
////  record.Value |> Option.iter (fun value ->
////    printfn "%d: %f" record.Date value)