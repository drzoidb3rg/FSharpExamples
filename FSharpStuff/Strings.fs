module Strings

open System
open System.Text

let items = "keyboard; mouse; ; monitor"
let result = items.Split([|"; "|], StringSplitOptions.RemoveEmptyEntries)


Seq.iter (fun x -> printfn "%A" x) result