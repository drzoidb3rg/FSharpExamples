module Curry


let add x y = x + y

let inc = add 1

let anotherInc x = add 1 x

let printCurry1 =
  printfn "%d is the same as %d" (inc 1) (anotherInc 1)
 


let sub x y = x - y
   
let swapArgs f x y = f y x

let dec = swapArgs sub 1

let printSwapArgs =
  printfn "Before 10 there is %d" (dec 10)

