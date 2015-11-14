module Play

open FSharp.Configuration
open System.Text




//open System.Configuration

    type Settings = AppSettings<"app.config">

    let pdw = Settings.Oviduser

//let add a b = a + b

//let add = (fun x y -> x + y)
let add x = (fun y -> x + y)

let increment = add 1

let printIncrement =
 printfn "%d" (increment 100)



let loop =
 [10..11]
 |> Seq.map(fun x-> increment x)
 |> Seq.iter(fun x -> printfn "%d" x) 


let listOfThings = [5..100]




let product n =
 let initialValue =1
 let action productSoFar x = productSoFar * x
 [1..n] |> List.fold action initialValue


let printProduct =
 printfn "result is %d" (product 3)

let incrementByOne (x:int) : int = x + 1

let DoSomething strategy x =
 strategy x

let result = DoSomething incrementByOne 2

printfn "result is %d" result


let isEven x = ( x % 2 = 0)

3
|> incrementByOne 
|> DoSomething isEven
|> printfn "is this even is %b"


let hello = printfn "hello %s"

["bob"; "jane"; "dave"] 
|> List.iter hello

let divide ifZero ifSuccess top bottom =
  if (bottom = 0)
  then ifZero()
  else ifSuccess  (top/bottom)



let ifZero () = printfn "this is zero"

let ifSuccess x = printfn "ok"

let specialisedDivide = divide ifZero ifSuccess

let result3 = specialisedDivide 10 5

//let xxx = printfn "divide is %d" result3



let bind nextFunction optionInput =
 match optionInput with
 | Some s -> nextFunction
 | None -> None


let add42 x = x + 42

1 |> add42


let add42ToOption = Option.map add42

Some 1 |> add42ToOption


Some 1 |> Option.map add42

printfn "list reduct : %s" (["a";"b";"c"] |> List.reduce(+))


type OrderLine = {Qty:int; Total:float}

let orderLines = [ {Qty=2; Total=19.8};
 {Qty=3; Total=1.99};
 {Qty=4; Total=12.5}]

let addPair line1 line2 =
  let newQty = line1.Qty + line2.Qty
  let newTotal = line1.Total + line2.Total
  {Qty=newQty; Total=newTotal}
 
let orderLineReduce = orderLines |> List.reduce addPair

printfn "Qty is %d; Totalmis %f" orderLineReduce.Qty orderLineReduce.Total

type intPair = int * int * string


type CardType = CardType of string

type CardNumber = int

type PaymentMethod =
 | Cash
 | Cheque of int
 | Card of CardType * CardNumber

let payment = CardType "f"


type OrderLineQty = | OrderLineQty of int

let createOrderLineQty qty =
 if qty > 0 && qty < 100
 then Some (OrderLineQty qty)
 else None


let goodQty = createOrderLineQty 10 |> Option.get
let goodQty2 = createOrderLineQty 10 |> Option.get

let lessThan x (OrderLineQty q) = q < x

let inline (+) (OrderLineQty a) (OrderLineQty b) = OrderLineQty(a + b) 

printfn "good quantity is %A" (goodQty + goodQty2)


type MyInput = Name of string 

type Result <'TEntity> =
 | Failure of string
 | Success of 'TEntity

let nameNotBlank input  =
 if input = "" 
 then Failure "name is blank"
 else Success input



type Point =
  | TwoD of int * int
  | ThreeD of int * int * int

let P1 = TwoD(3,4)

let P2 = ThreeD(3,4,5)

let generatePowerOfFunc baseValue =
  (fun exponent -> baseValue ** exponent)

[ [1]; []; [4;5;6]; [3;4]; []; []; []; [9] ] |> List.filter(not << List.isEmpty)


type Shape = Circle of int | Rectangle of int | Triangle of int

let shapeList = [ Circle(1); Circle(2); Rectangle(9)]

let circles =
    shapeList |> List.choose(fun x ->
        match x with 
        | Circle l -> Some l
        | _ -> None)

let rectangles =
    shapeList |> List.choose(fun x ->
        match x with 
        | Rectangle l -> Some l
        | _ -> None)

let genList (list:Shape list) =
  list |> List.choose(fun x ->
       match x with
       | Circle l -> Some l
       | _ -> None)

for x in circles do
  printf "circle %d" x


type AuthorInitials = string
type AuthorSurname = string

type OvidAuthorColumn =
  | Simple of string
  | Compound of AuthorInitials * AuthorSurname


  