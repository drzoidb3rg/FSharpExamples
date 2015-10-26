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
      | JsonValue.String s -> GenericDynamicObject<string>(s |> string).DynamicObject |> Some
      | JsonValue.Array elements -> GenericDynamicObject<JsonValue[]>(elements).DynamicObject |> Some
      | _ -> None

  type ebscoProvider = JsonProvider<"json/ebsco.json">
  let ebscoSample = ebscoProvider.GetSample()


  type MaybeBuilder () = 
    member __.Bind (m,f) = 
       match m with
       | Some x -> f x
       | None -> None
    member __.ReturnFrom x = x
    member __.Return x = Some x
  let maybe = MaybeBuilder ()

  let inline (|?) (a: 'a option) b = if a.IsSome then a.Value else b
  let nullDynamic = GenericDynamicObject<string>(null).DynamicObject


  let getGenericJsonValue (x:JsonValue) (key:string) = maybe {
    let! p = recordProperties x
    let! r = Map.tryFind key p
    return itemValue r
  }

  let getSafeJsonValue x key = 
    let optionResult = getGenericJsonValue x key
    optionResult.Value |? nullDynamic
    
    
   




     