module public Client
  open System.Runtime.CompilerServices
  open FSharp.Data
        
  type GenericDynamicObject<'a>(x:'a) = 
      [<Dynamic([|true|])>]
        member this.DynamicObject  : obj = box x

  let recordProperties (x:JsonValue) = 
      match x with
      | JsonValue.Record x ->  Map.ofArray x |> Some
      | _ -> None

  
  let itemValue (x:JsonValue) =
      match x with 
      | JsonValue.String s -> GenericDynamicObject<string>(s |> string).DynamicObject
      | JsonValue.Array elements -> GenericDynamicObject<JsonValue[]>(elements).DynamicObject
      | _ -> GenericDynamicObject<System.Object>(null).DynamicObject

  type ebscoProvider = JsonProvider<"json/ebsco.json">
  let ebscoSample = ebscoProvider.GetSample()


  //think we need an Option.Bind here, don't just take the value
  let getGenericJsonValue (x:JsonValue) (key:string) =  
     recordProperties(x).Value.Item(key) |> itemValue





     