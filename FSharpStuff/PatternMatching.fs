module PatternMatching

let input = [ (1., 2., 3.); (2., 1., 1.); (3., 0., 1.)]

let rec search lst =
 match lst with
 | (1., _, z) :: tail ->
   printfn "found x=1. and z=%f" z; search tail
 | (2., _, _) :: tail -> 
   printfn "found x=2."; search tail
 | _ :: tail -> search tail
  | [] -> printfn "done."


//let printPattern1 = search input

let (|Norm|) (a:float, b:float, c:float) =
  sqrt(a*a + b*b + c*c)

let v = (1., 0., 0.)

let printPattern2 = 
match v with
 | Norm(1.) -> printfn "Versor found!"
 | Norm(n)  -> printfn "Simple vector with norm %f" n



let rec isPalindrome (s:string) 
    (fromidx:int) (toidx:int) =
  if s = null then false
  elif toidx - fromidx < 2 then true
  elif s.[fromidx] = s.[(toidx - 1)] then 
      isPalindrome s (fromidx + 1) (toidx - 1)
  else false
let (|PALINDROME|_|) (s:string) = 
    if isPalindrome s 0 s.Length then Some s 
        else None

match "aba" with
| PALINDROME(v) -> printfn "The string %s is palindrome" v
| "Antonio" -> printfn "Hello Antonio"

