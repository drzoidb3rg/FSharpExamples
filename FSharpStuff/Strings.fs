module Strings

open System
open System.Text

let items = "keyboard; mouse; ; monitor"
let result = items.Split([|"; "|], StringSplitOptions.RemoveEmptyEntries)


Seq.iter (fun x -> printfn "%A" x) result

//http://fedsearch.proquest.com/search/thesaurus?operation=searchRetrieve&version=1.2&x-databases=BritishNursingIndex&x-username=nice&x-password=hiley&query=Contains:*cancer*&x-fields=HeadTerm,ScopeNotes,UseInsteadTerms

//http://fedsearch.proquest.com/search/thesaurus?operation=searchRetrieve&version=1.2&x-databases=BritishNursingIndex&x-username=nice&x-password=hiley&query=ExplodePosition:162

let baseUrl = sprintf "http://base"

let auth u p = sprintf "?something&username=%s&pwd=%s" u p

let query t = sprintf "&term=%s" t

let actualAuth = auth "u" "p"


let url t = sprintf "%s%s%s" baseUrl (auth "c" "f") (query t)

let concat x y =
  sprintf "%s%s" y x

let inline (>>>) x y = concat y x


let temp t =
  baseUrl
  >>> (auth "c" "f")
  >>> (query t)



