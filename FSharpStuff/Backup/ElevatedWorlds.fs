module ElevatedWorlds

module Option =

    // The apply function for Options
    let apply fOpt xOpt = 
        match fOpt,xOpt with
        | Some f, Some x -> Some (f x)
        | _ -> None


module List =

    // The apply function for lists
    // [f;g] apply [x;y] becomes [f x; f y; g x; g y]
    let apply (fList: ('a->'b) list) (xList: 'a list)  = 
        [ for f in fList do
          for x in xList do
              yield f x ]

//
//     let doString s = ""
//
//     let optionalString s = Option.map doString s
//     
//     let testMap n = Option.map HtmlNode.innerText
//
//
//     let f1 (s:string) = s |> Some
//
//     let f2 (s:Option<string>) = s
//
//     let bindAble_f1 s = Option.bind f1
//
//     let f3 s =  f2 >> bindAble_f1