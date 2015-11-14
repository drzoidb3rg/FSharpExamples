module public Client
  open System.Runtime.CompilerServices
  open FSharp.Data

  type GenericDynamicObject<'a>(x:'a) = 
      [<Dynamic([|true|])>]
        member this.DynamicObject  : obj = box x

  let (>>=) m f = Option.bind f m

  let inline (|?) (a: 'a option) b = if a.IsSome then a.Value else b

  let nullDynamic = GenericDynamicObject<string>(null).DynamicObject

  let recordProperties (x:JsonValue) = 
      match x with
      | JsonValue.Record x ->  Map.ofArray x |> Some
      | _ -> None

  
  let itemValue (x:JsonValue) =
      match x with 
      | JsonValue.String s -> GenericDynamicObject<string>(s |> string).DynamicObject |> Some
      | JsonValue.Array elements -> GenericDynamicObject<JsonValue[]>(elements).DynamicObject |> Some
      | _ -> None

  type ebscoProvider = JsonProvider<"json/ebsco.json">
  let ebscoSample = ebscoProvider.GetSample()

  let getBetterGenericJsonValue (x:JsonValue) (key:string) =
     x |> recordProperties >>= Map.tryFind key >>= itemValue |? nullDynamic
    
    
    
   




     