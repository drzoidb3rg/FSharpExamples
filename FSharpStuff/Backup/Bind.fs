module Bind

let strToInt str = 
    if System.Text.RegularExpressions.Regex.IsMatch(str,@"^[0-999]") 
    then str |> int |> Some
    else None


let p1 = printfn "%A" ("44" |> strToInt |> Option.get )

type MaybeBuilder() =
     member this.Bind(m, f) = Option.bind f m
     member this.Return(x) = Some x

let yourWorkflow = new MaybeBuilder()

let stringAddWorkflow x y z = 
    yourWorkflow 
        {
        let! a = strToInt x
        let! b = strToInt y
        let! c = strToInt z
        return a + b + c
        }    

let good = stringAddWorkflow "12" "3" "2"
let bad = stringAddWorkflow "12" "xyz" "2"

let printGood = printfn "%A" good
let printbad = printfn "%A" bad



type DbResult<'a> = 
    | SuccessX of 'a
    | Error of string


let getCustomerId name =
    if (name = "") 
    then Error "getCustomerId failed"
    else SuccessX "Cust42"
