module FunAndProfit

let add1 = (+) 1
let equals2 = (=) 2

[1..100] |> List.map add1 |> List.filter equals2





let divide ifGood ifBad bottom top =
  if bottom = 0
  then ifBad()
  else ifGood(top/bottom)


//let bad () = printfn "bad"
//let good i = printfn "good %i" i

let five =5 
let optionFive = Some 5


let add2 x = "sdsd"

let bad () = None
let good (i:int) = Some i

let usefulDivide bottom top = divide good bad bottom top

let ten = 10


let divideByZero = usefulDivide 0 1


//let divide bottom top =
//  if bottom = 0
//  then None
//  else Some (top/bottom)

//------------------------

let add42 x = x + 42

let result = 1 |> add42

let add42Option x = Option.map add42

let result2 = Some 1 |> add42Option

let result3 i = Option.map i


